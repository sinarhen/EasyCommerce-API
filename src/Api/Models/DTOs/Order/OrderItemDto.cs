using ECommerce.Models.DTOs.User;

namespace ECommerce.Models.DTOs.Order;

public class OrderItemDto
{
    public Guid Id { get; set; }
    public OrderItemProductDto Product { get; set; }
    public int Quantity { get; set; }
    public UserDto Customer { get; set; }
    
    public string Status { get; set; }
}