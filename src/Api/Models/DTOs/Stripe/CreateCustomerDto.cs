namespace ECommerce.Models.DTOs.Stripe;

public record CreateCustomerDto(
    string Email, 
    string Name, 
    CreateCardDto Card);
    
    