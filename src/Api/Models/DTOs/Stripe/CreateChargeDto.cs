namespace ECommerce.Models.DTOs.Stripe;

public record CreateChargeDto(
    string Currency, 
    long Amount, 
    string CustomerId, 
    string ReceiptEmail, 
    string Description);