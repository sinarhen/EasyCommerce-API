using ECommerce.Models.DTOs.Product;

namespace ECommerce.Models.DTOs;

public class CartDto
{
    public Guid Id { get; set; }
    public ICollection<CartItemDto> Products { get; set; } = new List<CartItemDto>();
    public decimal TotalPrice { get; set; }
    public int TotalQuantity { get; set; }
    public string CustomerId { get; set; }
}


public class CartItemDto
{
    // TODO: Uncomment after implementing CartItemProductDto
    // public CartItemProductDto Product { get; set; }    
    public int Quantity { get; set; }
    public string CustomerId { get; set; }
}