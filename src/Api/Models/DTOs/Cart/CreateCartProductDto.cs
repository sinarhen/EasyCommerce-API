namespace ECommerce.Models.DTOs.Cart;

public class CreateCartProductDto
{
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
    
}