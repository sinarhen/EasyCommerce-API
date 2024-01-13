namespace ECommerce.Models.DTOs.Stock;
public class ProductStockDto
{
    public Guid ColorId { get; set; }
    public List<ProductStockSizeDto> Sizes { get; set; }
    public List<string> ImageUrls { get; set; }
}