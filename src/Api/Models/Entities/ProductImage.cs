using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Models.Entities;

[PrimaryKey("ProductId", "ColorId")]
public class ProductImage : BaseEntity
{

    [Key]
    public Guid ProductId { get; set; }

    [Key]
    public Guid ColorId { get; set; }
    
    public List<string> ImageUrls { get; set; }
    
    // Navigation properties
    [ForeignKey("ProductId")]
    public Product Product { get; set; }

    [ForeignKey("ColorId")]
    public Color Color { get; set; }
}