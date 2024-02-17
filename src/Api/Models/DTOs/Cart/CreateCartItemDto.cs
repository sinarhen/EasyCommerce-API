using System.ComponentModel.DataAnnotations;
using ECommerce.Models.Validation;

namespace ECommerce.Models.DTOs.Cart;

public class CreateCartItemDto
{
    [Required] public Guid ProductId { get; set; }

    [Required] public Guid ColorId { get; set; }

    [Required] public Guid SizeId { get; set; }

    [Required] [MinValue(1)] public int Quantity { get; set; }
}