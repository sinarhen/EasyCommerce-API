using Microsoft.EntityFrameworkCore;

namespace ECommerce.Models.Entities;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[PrimaryKey("ProductId", "ColorId", "SizeId")]
public class ProductStock : BaseEntity
{
    [Key]
    public Guid ProductId { get; set; }

    [Key]
    public Guid ColorId { get; set; }

    [Key]
    public Guid SizeId { get; set; }

    public decimal Price { get; set; }
    public int Stock { get; set; }
    
    public double Discount { get; set; }
    
    // Navigation properties
    [ForeignKey("ProductId")]
    public Product Product { get; set; }
    [ForeignKey("ColorId")]
    public Color Color { get; set; }

    [ForeignKey("SizeId")]
    public Size Size { get; set; }
    

}