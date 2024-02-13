namespace ECommerce.Models.DTOs.Cart;

public class CreateCartItemDto
{
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
    
}