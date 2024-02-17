using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ECommerce.Entities.Enum;
using ECommerce.Models.Enum;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Models.Entities;

[PrimaryKey("Id")]
public class Product : BaseEntity
{
    [Key] public Guid Id { get; set; }

    [Required]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "Name is too long. max 100 characters.")]
    public string Name { get; set; }

    [StringLength(3000, ErrorMessage = "Description is too long. max 3000 characters.")]
    public string Description { get; set; }


    [Key] public Guid? OccasionId { get; set; }

    [StringLength(2000, ErrorMessage = "Size Chart URL is too long")]
    public string SizeChartImageUrl { get; set; }

    public Gender? Gender { get; set; }

    public Season? Season { get; set; }

    public string SellerId { get; set; }

    public Guid? CollectionId { get; set; }

    // Navigation properties
    [ForeignKey("OccasionId")] public Occasion Occasion { get; set; }


    [ForeignKey("CollectionId")] public Collection Collection { get; set; }

    // Using denormalization to avoid joins and improve performance.
    [ForeignKey("SellerId")] public User Seller { get; set; }

    public ICollection<ProductMaterial> Materials { get; set; } = new List<ProductMaterial>();
    public ICollection<ProductImage> Images { get; set; } = new List<ProductImage>();
    public ICollection<ProductStock> Stocks { get; set; } = new List<ProductStock>();

    [Required] public ICollection<Review> Reviews { get; set; } = new List<Review>();

    public ICollection<OrderDetail> Orders { get; set; } = new List<OrderDetail>();
    public ICollection<ProductCategory> Categories { get; set; } = new List<ProductCategory>();

    // public ICollectiion<CartProduct> Carts { get; set; } = new List<CartProduct>();
    // Unnecessary because we don't need to know which carts a product is in and how many of them   
}