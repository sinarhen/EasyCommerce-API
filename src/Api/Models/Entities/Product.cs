using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Models.Entities;

[PrimaryKey("Id")]
public class Product
{
    [Key]
    public Guid Id { get; set; }
    
    public string Name { get; set; }
    
    public string Description { get; set; }


    public double? Discount { get; set; }

    [Key]
    public Guid? OccasionId { get; set; }


    public string SizeChartImageUrl { get; set; }
    public Gender? Gender { get; set; }

    public Season? Season { get; set; }

    public Guid MainMaterialId { get; set; }
    
    public Guid? CollectionId { get; set; }
    
    public int? CollectionYear { get; set; }
    
    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    // Navigation properties
    [ForeignKey("OccasionId")]
    public Occasion Occasion { get; set; }

    [ForeignKey("MainMaterialId")]
    public Material MainMaterial { get; set; }
    
    [ForeignKey("CollectionId")]
    public Collection Collection { get; set; }
    public ICollection<ProductMaterial> Materials { get; set; } = new List<ProductMaterial>();
    public ICollection<ProductImage> Images { get; set; } = new List<ProductImage>();
    public ICollection<ProductStock> Stocks { get; set; } = new List<ProductStock>();
    public ICollection<Review> Reviews { get; set; } = new List<Review>();
    public ICollection<OrderDetail> Orders { get; set; } = new List<OrderDetail>();
    public ICollection<ProductCategory> Categories { get; set; } = new List<ProductCategory>();
    
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