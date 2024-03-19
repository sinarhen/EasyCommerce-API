using ECommerce.Models.DTOs.Category;
using ECommerce.Models.DTOs.Color;
using ECommerce.Models.DTOs.Material;
using ECommerce.Models.DTOs.Size;

namespace ECommerce.Models.DTOs.Product;

public class ProductFiltersDto
{
    public List<Entities.Category> Categories { get; set; } = new List<Entities.Category>();
    public List<SizeDto> Sizes { get; set; } = new List<SizeDto>();
    public List<ColorDto> Colors { get; set; } = new List<ColorDto>();
    public List<IdNameDto> Occasions { get; set; } = new List<IdNameDto>();
    public List<MaterialDto> Materials { get; set; } = new List<MaterialDto>();
}