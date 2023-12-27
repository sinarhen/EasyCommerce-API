using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Models.Entities;

[PrimaryKey("ProductId", "ColorId")]
public class ProductColorQuantity
{
    [Key]
    [Column(Order = 1)]
    public Guid ProductId { get; set; }

    [Key]
    [Column(Order = 2)]
    public Guid ColorId { get; set; }

    public int Quantity { get; set; }

    // Navigation properties
    public Product Product { get; set; }
    public Color Color { get; set; }
}