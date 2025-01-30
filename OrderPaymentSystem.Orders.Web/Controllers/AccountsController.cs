using Microsoft.AspNetCore.Mvc;
using OrderPaymentSystem.Orders.Application.Abstractions;
using OrderPaymentSystem.Orders.Application.Models.Authentication;

namespace OrderPaymentSystem.Orders.Web.Controllers;

[Route("accounts")]
public class AccountsController(IAuthService authService) : ApiBaseController
{
    [HttpPost("login")]
    public async Task<IActionResult> Login(UserLoginDTO userLoginDto)
    {
        var result = await authService.Login(userLoginDto);
        return Ok(result);
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(UserRegisterDTO userRegisterDto)
    {
        var result = await authService.Register(userRegisterDto);

        return Ok(result);
    }
}
