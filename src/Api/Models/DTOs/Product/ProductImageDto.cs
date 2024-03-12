namespace ECommerce.Models.DTOs.Product;

public class ProductImageDto
{
    public Guid ColorId { get; set; }
    public List<string> ImageUrls { get; set; }
}