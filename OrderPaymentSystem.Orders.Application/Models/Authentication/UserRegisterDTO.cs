namespace OrderPaymentSystem.Orders.Application.Models.Authentication;

public record UserRegisterDTO(string Username, string Email, string Phone, string Password);