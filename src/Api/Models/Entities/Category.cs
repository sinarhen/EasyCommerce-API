using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Models.Entities;

[PrimaryKey("Id")]
[Index("ParentCategoryId")]
public class Category : BaseEntity
{
    [Key] public Guid Id { get; set; }


    public Guid? ParentCategoryId { get; set; }

    public string Name { get; set; }

    public string ImageUrl { get; set; }

    // Navigation properties
    [ForeignKey("ParentCategoryId")] public Category ParentCategory { get; set; }

    [InverseProperty("ParentCategory")] public ICollection<Category> SubCategories { get; set; } = new List<Category>();

    public ICollection<ProductCategory> Products { get; set; } = new List<ProductCategory>();
    public ICollection<CategorySize> Sizes { get; set; } = new List<CategorySize>();
}