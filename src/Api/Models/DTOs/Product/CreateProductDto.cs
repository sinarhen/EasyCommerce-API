
using ECommerce.Models.DTOs.Material;
using ECommerce.Models.DTOs.Stock;

namespace ECommerce.Models.DTOs.Product;
public class CreateProductDto
{
    public string Name { get; set; }
    public Guid CategoryId { get; set; }
    public string Description { get; set; }
    public double? Discount { get; set; }
    public string SizeChartImageUrl { get; set; }
    public string Gender { get; set; }
    public string Season { get; set; }
    public Guid MainMaterialId { get; set; }
    public Guid OccasionId { get; set; }
    
    public Guid CollectionId { get; set; }
    public List<MaterialDto> Materials { get; set; }
    public List<ProductStockDto> Stocks { get; set; }
}