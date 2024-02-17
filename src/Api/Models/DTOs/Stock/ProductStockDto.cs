using System.ComponentModel.DataAnnotations;

namespace ECommerce.Models.DTOs.Stock;

public class ProductStockDto
{
    [Required] public Guid ColorId { get; set; }

    [Required] public Guid SizeId { get; set; }

    public decimal Price { get; set; }
    public int Stock { get; set; }
    public double Discount { get; set; }
}