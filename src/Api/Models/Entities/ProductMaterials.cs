using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Models.Entities;

[PrimaryKey("ProductId", "MaterialId")]
public class ProductMaterials
{
    [Key]
    public Guid ProductId { get; set; }
    
    [Key]
    public Guid MaterialId { get; set; }
    
    
    public int Percentage { get; set; }
    
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    // Navigation properties
    public Material Material { get; set; }
    public Product Product { get; set; }

}