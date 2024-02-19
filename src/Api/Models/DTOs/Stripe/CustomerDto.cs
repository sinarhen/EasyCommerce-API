namespace ECommerce.Models.DTOs.Stripe;

public record CustomerDto(
    string CustomerId,
    string Email,
    string Name);