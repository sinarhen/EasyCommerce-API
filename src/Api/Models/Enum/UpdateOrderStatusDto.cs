using System.ComponentModel.DataAnnotations;
using ECommerce.Models.Validation;

namespace ECommerce.Models.Enum;

public class UpdateOrderStatusDto
{
    [Required]
    [EnumValue(typeof(OrderItemStatus))]
    public string Status { get; set; }
}