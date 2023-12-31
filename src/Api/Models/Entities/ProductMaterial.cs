using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Models.Entities;

[PrimaryKey("ProductId", "Id")]
public class ProductMaterial
{
    
    [Key]
    public Guid ProductId { get; set; }

    [Key]
    public Guid MaterialId { get; set; }

    public double Percentage { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    // Navigation properties
    [ForeignKey("Id")]
    public Material Material { get; set; }
    
    [ForeignKey("ProductId")]
    public Product Product { get; set; }
}