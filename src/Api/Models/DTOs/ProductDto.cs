
namespace ECommerce.Models.DTOs;


public class ProductDto 
{
    
    public Guid Id { get; set; }
    public CategoryDto[] Categories { get; set; }
    
    public string Name { get; set; }
    public string Description { get; set; }
    public double? Discount { get; set; }
    public string OccasionName { get; set; }
    public string OccasionId { get; set; }
    public string CollectionName { get; set; }
    public string CollectionId { get; set; }
    public string MainMaterialName { get; set; }
    public string Gender { get; set; }
    public string Season { get; set; }

    public int OrdersCount { get; set; }
    public int OrdersCountLastMonth { get; set; }
    public int ReviewsCount { get; set; }
    public double AvgRating { get; set; }
    public decimal MinPrice { get; set; }
    public decimal DiscountPrice { get; set; }
    public bool IsNew { get; set; }
    public bool IsOnSale { get; set; }
    
    public bool IsAvailable { get; set; }
    public bool IsBestseller { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    
    public List<MaterialDto> Materials { get; set; }
    public List<SizeDto> Sizes { get; set; }
    public List<ColorDto> Colors { get; set; }
}

public class CategoryDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    
    public int Order { get; set; }
}

