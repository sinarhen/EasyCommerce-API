namespace ECommerce.Models.DTOs.Cart;

public class CartItemDto
{
    public Guid Id { get; set; }
    public CartItemProductDto Product { get; set; }
    public int Quantity { get; set; }
}