using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using OrderPaymentSystem.Orders.Application.Abstractions;
using OrderPaymentSystem.Orders.Application.Models.Authentication;
using OrderPaymentSystem.Orders.Domain.Entities;
using OrderPaymentSystem.Orders.Domain.Exceptions;
using OrderPaymentSystem.Orders.Domain.Models;
using OrderPaymentSystem.Orders.Domain.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Authentication;
using System.Security.Claims;
using System.Text;

namespace OrderPaymentSystem.Orders.Application.Services;

public class AuthService(IOptions<AuthOptions> authOptions,
    UserManager<UserEntity> userManager) : IAuthService
{
    private readonly AuthOptions _authOptions = authOptions.Value;

    public async Task<UserResponse> Register(UserRegisterDTO userRegisterDTO)
    {
        if (await userManager.FindByEmailAsync(userRegisterDTO.Email) != null)
        {
            throw new DuplicateEntityException("Email", userRegisterDTO.Email);
        }

        var createUserResult = await userManager.CreateAsync(new UserEntity
        {
            Email = userRegisterDTO.Email,
            PhoneNumber = userRegisterDTO.Phone,
            UserName = userRegisterDTO.Username
        }, userRegisterDTO.Password);

        if (createUserResult.Succeeded)
        {
            var user = await userManager.FindByEmailAsync(userRegisterDTO.Email)
                ?? throw new EntityNotFoundException("User", "Email", userRegisterDTO.Email);
            var result = await userManager.AddToRoleAsync(user, RolesEnum.User.Value);

            if (result.Succeeded)
            {
                var response = new UserResponse
                {
                    Id = user.Id,
                    Email = user.Email,
                    Roles = [RolesEnum.User.Value],
                    Username = user.UserName,
                    Phone = user.PhoneNumber
                };
                return GenerateToken(response);
            }

            throw new Exception($"Errors: {string.Join(";", result.Errors
                .Select(x => $"{x.Code} {x.Description}"))}");
        }

        throw new Exception();
    }

    public async Task<UserResponse> Login(UserLoginDTO userLoginDTO)
    {
        var user = await userManager.FindByEmailAsync(userLoginDTO.Email)
            ?? throw new EntityNotFoundException("User", "Email", userLoginDTO.Email);

        var checkPasswordResult = await userManager.CheckPasswordAsync(user, userLoginDTO.Password);

        if (checkPasswordResult)
        {
            var userRoles = await userManager.GetRolesAsync(user);
            var response = new UserResponse
            {
                Id = user.Id,
                Email = user.Email,
                Roles = [.. userRoles],
                Username = user.UserName,
                Phone = user.PhoneNumber
            };
            return GenerateToken(response);
        }

        throw new AuthenticationException();
    }

    public UserResponse GenerateToken(UserResponse userRegisterModel)
    {
        var handler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_authOptions.TokenPrivateKey);
        var credentials = new SigningCredentials(
            new SymmetricSecurityKey(key),
            SecurityAlgorithms.HmacSha256Signature);

        var claims = new Dictionary<string, object>
        {
            {ClaimTypes.Name, userRegisterModel.Email!},
            {ClaimTypes.NameIdentifier, userRegisterModel.Id.ToString()},
            {JwtRegisteredClaimNames.Aud, "test"},
            {JwtRegisteredClaimNames.Iss, "test"}
        };
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = GenerateClaims(userRegisterModel),
            Expires = DateTime.UtcNow.AddMinutes(_authOptions.ExpireIntervalMinutes),
            SigningCredentials = credentials,
            Claims = claims,
            Audience = "test",
            Issuer = "test"
        };

        var token = handler.CreateToken(tokenDescriptor);
        userRegisterModel.Token = handler.WriteToken(token);

        return userRegisterModel;
    }

    private static ClaimsIdentity GenerateClaims(UserResponse userRegisterModel)
    {
        var claims = new ClaimsIdentity();
        claims.AddClaim(new Claim(ClaimTypes.Name, userRegisterModel.Email!));
        claims.AddClaim(new Claim(ClaimTypes.NameIdentifier, userRegisterModel.Id.ToString()));
        claims.AddClaim(new Claim(JwtRegisteredClaimNames.Aud, "test"));
        claims.AddClaim(new Claim(JwtRegisteredClaimNames.Iss, "test"));

        foreach (var role in userRegisterModel.Roles!)
            claims.AddClaim(new Claim(ClaimTypes.Role, role));

        return claims;
    }
}