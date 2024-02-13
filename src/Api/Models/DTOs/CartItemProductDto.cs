using ECommerce.Models.DTOs.Color;
using ECommerce.Models.DTOs.Size;

namespace ECommerce.Models.DTOs;

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