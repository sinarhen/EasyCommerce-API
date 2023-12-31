
namespace ECommerce.Models.DTOs;

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
    public int? CollectionYear { get; set; }
    public Guid OccasionId { get; set; }
    public List<CreateProductMaterialDto> Materials { get; set; }
    public List<CreateProductStockDto> Stocks { get; set; }
}

public class CreateProductMaterialDto
{
    public Guid Id { get; set; }
    public double Percentage { get; set; }
}

public class CreateProductStockDto
{
    public Guid ColorId { get; set; }
    public List<CreateProductStockSizeDto> Sizes { get; set; }
    public List<string> ImageUrls { get; set; }
    
}

public class CreateProductStockSizeDto
{
    public Guid SizeId { get; set; }
    public int Stock { get; set; }
    public decimal Price { get; set; }
}