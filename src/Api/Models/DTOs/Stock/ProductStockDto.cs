namespace ECommerce.Models.DTOs.Stock;
public class ProductStockDto
{
    public Guid ColorId { get; set; }
    public Guid SizeId { get; set; }
    public decimal Price { get; set; }
    public int Stock { get; set; }
    public double Discount { get; set; }
    
}