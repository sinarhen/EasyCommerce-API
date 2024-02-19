namespace ECommerce.Models.DTOs.Stripe;

public record CreateCardDto(
    string Name, 
    string Number, 
    string ExpiryYear, 
    string ExpiryMonth, 
    string Cvc);