using OrderPaymentSystem.Orders.Application.Models.Authentication;

namespace OrderPaymentSystem.Orders.Application.Abstractions;

public interface IAuthService
{
    Task<UserResponse> Register(UserRegisterDTO userRegisterModel);
    Task<UserResponse> Login(UserLoginDTO userLoginDto);
}