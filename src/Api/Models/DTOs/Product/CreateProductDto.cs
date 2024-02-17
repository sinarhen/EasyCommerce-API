using System.ComponentModel.DataAnnotations;
using ECommerce.Entities.Enum;
using ECommerce.Models.DTOs.Material;
using ECommerce.Models.DTOs.Stock;
using ECommerce.Models.Enum;
using ECommerce.Models.Validation;

namespace ECommerce.Models.DTOs.Product;

public class CreateProductDto
{
    [Required]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "Name is too long. max 100 characters.")]
    public string Name { get; set; }

    public Guid CategoryId { get; set; }

    [StringLength(3000, ErrorMessage = "Description is too long. max 3000 characters.")]
    public string Description { get; set; }

    public double? Discount { get; set; }

    [Url]
    [StringLength(2000, ErrorMessage = "Size Chart URL is too long. max 2000 characters.")]
    public string SizeChartImageUrl { get; set; }

    [EnumValue(typeof(Gender))] public string Gender { get; set; }

    [EnumValue(typeof(Season))] public string Season { get; set; }

    public Guid? OccasionId { get; set; }

    [Required] public Guid CollectionId { get; set; }

    public List<MaterialDto> Materials { get; set; }
    public List<ProductStockDto> Stocks { get; set; }
    public List<ProductImageDto> Images { get; set; }
}

public class ProductImageDto
{
    public Guid ColorId { get; set; }
    public List<string> ImageUrls { get; set; }
}