using ECommerce.Models.DTOs.User;

namespace ECommerce.Models.DTOs.Order;

public class OrderDto
{
    public Guid? Id { get; set; }
    public List<OrderItemDto> Products { get; set; } = new List<OrderItemDto>();
    public decimal TotalPrice { get; set; }
    public int TotalQuantity { get; set; }
    public UserDto Customer { get; set; }
}