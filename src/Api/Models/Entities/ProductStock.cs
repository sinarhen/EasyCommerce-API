using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Models.Entities;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[PrimaryKey("ProductStockId")]
public class ProductStock
{
    [Key]
    public Guid ProductStockId { get; set; }

    public Guid ProductId { get; set; }

    public Guid ColorId { get; set; }

    public Guid SizeId { get; set; }

    public decimal Price { get; set; }
    public int Stock { get; set; }
    
    
    // Navigation properties
    [ForeignKey("ProductId")]
    public Product Product { get; set; }
    [ForeignKey("ColorId")]
    public Color Color { get; set; }

    [ForeignKey("SizeId")]
    public Size Size { get; set; }

}
