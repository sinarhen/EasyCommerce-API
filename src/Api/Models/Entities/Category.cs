using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Models.Entities;

[PrimaryKey("Id")]
public class Category
{
    [Key]
    public Guid Id { get; set; }
    public Guid? ParentCategoryId { get; set; } // New field for parent category

    public string Name { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    // Navigation properties
    [ForeignKey("ParentCategoryId")]
    public Category ParentCategory { get; set; } // New navigation property for parent category
    public ICollection<Category> SubCategories { get; set; } = new List<Category>(); // New navigation property for subcategories
    public ICollection<Product> Products { get; set; } = new List<Product>();
}