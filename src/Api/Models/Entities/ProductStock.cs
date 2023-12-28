using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Models.Entities;

public class ProductStock
{
    [Key]
    public Guid ProductId { get; set; }
    
    [Key]
    public Guid ColorId { get; set; }
    
    public Size Size { get; set; }
    
    public int Stock { get; set; }

    // Navigation properties
    public Product Product { get; set; }
    public Color Color { get; set; }
    
    
}

public enum Size
{
    XS,
    S,
    M,
    L,
    XL,
    XXL
}