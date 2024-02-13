namespace ECommerce.Models.DTOs.Cart;

public class CartItemDto
{
    public CartItemProductDto Product { get; set; }    
    public int Quantity { get; set; }
}