using ECommerce.Models.DTOs;
using ECommerce.Models.Entities;

namespace Ecommerce.Models.DTOs;

public class UpdateProductDto
{
    public string Name { get; set; }
    public string CategoryId { get; set; }
    public string Description { get; set; }
    public double? Discount { get; set; }
    public string OccasionId { get; set; }
    public string SizeChartImageUrl { get; set; }
    public string Gender { get; set; }
    public string Season { get; set; }
    public string MainMaterialId { get; set; }
    public string CollectionId { get; set; }
    public List<MaterialDto> Materials { get; set; } = new List<MaterialDto>(); // Name property is optional
    public List<ProductStockDto> Stocks { get; set; } = new List<ProductStockDto>();
}