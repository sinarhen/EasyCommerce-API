namespace ECommerce.Models.Entities;

public class Product
{
    public Guid Id { get; set; }

    public Guid CategoryId { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }


    public double? Discount { get; set; }

    public Occasion Occasion { get; set; }

    public string SizeChartImageUrl { get; set; }
    public Gender? Gender { get; set; }

    public Season? Season { get; set; }

    public int? CollectionYear { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    // Navigation properties
    public Category Category { get; set; }
    public Material MainMaterial { get; set; }
    
    public ICollection<ProductMaterial> Materials { get; set; } = new List<ProductMaterial>();
    public ICollection<ProductImage> Images { get; set; } = new List<ProductImage>();
    public ICollection<ProductStock> Stocks { get; set; } = new List<ProductStock>();
}

public enum Gender
{
    Male,
    Female,
    Unisex
}

public enum Season
{
    Winter,
    Spring,
    Summer,
    Autumn,
    All,
    DemiSeason,
}