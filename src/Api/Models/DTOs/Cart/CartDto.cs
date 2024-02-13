namespace ECommerce.Models.DTOs;

public class CartDto
{
    public Guid Id { get; set; }
    public ICollection<CartItemDto> Products { get; set; } = new List<CartItemDto>();
    public decimal TotalPrice { get; set; }
    public int TotalQuantity { get; set; }
    public string CustomerId { get; set; }
}