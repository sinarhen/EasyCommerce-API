using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Models.Entities;

[PrimaryKey("ProductId", "ColorId")]
public class ProductStock
{
    [Key] public Guid ProductId { get; set; }

    [Key] public Guid ColorId { get; set; }

    public Size? Size { get; set; }

    public decimal Price { get; set; }
    public int Stock { get; set; }
    
    public string CustomSize { get; set; }
    
    // Navigation properties
    public Product Product { get; set; }
    public Color Color { get; set; }
}
