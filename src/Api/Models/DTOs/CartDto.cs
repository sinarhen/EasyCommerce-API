using ECommerce.Models.DTOs.Color;
using ECommerce.Models.DTOs.Size;

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
    public CartItemProductDto Product { get; set; }    
    public int Quantity { get; set; }
    public string CustomerId { get; set; }
}

public class CartItemProductDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public string ImageUrl { get; set; }
    public ColorDto Color { get; set; }
    public SizeDto Size { get; set; }
    public string CustomerId { get; set; }
}