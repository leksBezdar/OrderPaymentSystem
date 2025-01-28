namespace OrderPaymentSystem.Orders.Application.Models.Authentication;

public record UserLoginDTO(string Username, string Email, string Phone, string Password);