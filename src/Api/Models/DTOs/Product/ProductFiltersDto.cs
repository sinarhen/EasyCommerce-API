using ECommerce.Models.DTOs.Category;
using ECommerce.Models.DTOs.Color;
using ECommerce.Models.DTOs.Material;
using ECommerce.Models.DTOs.Size;

namespace ECommerce.Models.DTOs.Product;

public class ProductFiltersDto
{
    public List<CategoryDto> Categories;
    public List<SizeDto> Sizes;
    public List<ColorDto> Colors;
    public List<IdNameDto> Occasions;
    public List<MaterialDto> Materials;
}