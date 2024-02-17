using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerce.Models.Entities;

// Could make composite key with CartId, ProductId, SizeId, and ColorId
// but it would be bad for performance
public class CartProduct : BaseEntity
{
    public Guid Id { get; set; }
    public Guid CartId { get; set; }
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
    public Guid SizeId { get; set; }
    public Guid ColorId { get; set; }

    // Navigation properties
    [ForeignKey("CartId")] public Cart Cart { get; set; }

    [ForeignKey("ProductId")] public Product Product { get; set; }

    [ForeignKey("SizeId")] public Size Size { get; set; }

    [ForeignKey("ColorId")] public Color Color { get; set; }
}