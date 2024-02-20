using System.ComponentModel.DataAnnotations;
using ECommerce.Models.Enum;
using ECommerce.Models.Validation;

namespace ECommerce.Models.DTOs.Order;

public class CreateOrderDto
{
    [EnumValue(typeof(OrderItemStatus))]
    public string Status { get; set; }
    
    [Required]
    public ICollection<CreateOrderItemDto> OrderItems { get; set; } = new List<CreateOrderItemDto>();
}