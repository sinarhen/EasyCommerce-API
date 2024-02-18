using System.ComponentModel.DataAnnotations;
using ECommerce.Models.Validation;

namespace ECommerce.Models.DTOs.Cart;

public class ChangeCartItemDto
{
    [Required] 
    [MinValue(1)] 
    [MaxValue(100)]
    public int Quantity { get; set; }
}