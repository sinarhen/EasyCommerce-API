using ECommerce.Models.DTOs.Color;
using ECommerce.Models.DTOs.Size;

namespace ECommerce.Models.DTOs.Order;

public class OrderItemProductDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public string ImageUrl { get; set; }
    public ColorDto Color { get; set; }
    public SizeDto Size { get; set; }
    public string SellerName { get; set; }
    public string SellerId { get; set; }
}