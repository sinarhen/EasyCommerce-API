using System.ComponentModel.DataAnnotations;

namespace ECommerce.Models.DTOs.Cart;

public class ChangeCartItemDto
{
    [Required] public int Quantity { get; set; }
}