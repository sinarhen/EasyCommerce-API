using ECommerce.Models.DTOs.Material;
using ECommerce.Models.DTOs.Size;

namespace ECommerce.Models.DTOs.Product;

public class ProductDetailsDto : ProductDto
{
    public string SizeChartImageUrl { get; set; }
    public List<MaterialDto> Materials { get; set; }
    public List<SizeDto> Sizes { get; set; }
    public List<ReviewDto> Reviews { get; set; }
}