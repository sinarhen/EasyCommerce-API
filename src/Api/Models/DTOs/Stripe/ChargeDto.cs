namespace ECommerce.Models.DTOs.Stripe;

public record ChargeDto(
    string ChargeId, 
    string Currency, 
    long Amount, 
    string CustomerId, 
    string ReceiptEmail,
    string Description);