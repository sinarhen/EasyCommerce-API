using ECommerce.Models.DTOs;
using ECommerce.Models.Entities;

namespace Ecommerce.Models.DTOs;

public class UpdateProductDto
{
    public string Name { get; set; }
    public Guid? CategoryId { get; set; }
    public string Description { get; set; }
    public double? Discount { get; set; }
    public Guid? OccasionId { get; set; }
    public string SizeChartImageUrl { get; set; }
    public string Gender { get; set; }
    public string Season { get; set; }
    public Guid? MainMaterialId { get; set; }
    public Guid? CollectionId { get; set; }
    public List<MaterialDto> Materials { get; set; } = new List<MaterialDto>();
    public List<ProductStockDto> Stocks { get; set; } = new List<ProductStockDto>();
}