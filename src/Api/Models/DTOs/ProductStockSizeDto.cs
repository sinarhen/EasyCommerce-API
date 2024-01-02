namespace ECommerce.Models.DTOs;

public class ProductStockSizeDto
{
    public Guid SizeId { get; set; }
    public int Stock { get; set; }
    public decimal Price { get; set; }
}