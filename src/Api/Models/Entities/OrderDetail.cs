using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Models.Entities;

[PrimaryKey("Id")]
public class OrderDetail 
{
    [Key]
    public Guid Id { get; set; }

    public Guid OrderId { get; set; }
    public Guid ProductId { get; set; }
    public Guid ColorId { get; set; }
    
    public int Quantity { get; set; }
    [Key]
    public Guid SizeId { get; set; }
    
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public OrderStatus Status { get; set; }

    // Navigation properties
    [ForeignKey("ColorId")]
    public Color Color { get; set; }
    [ForeignKey("ProductId")]
    public Product Product { get; set; }
    [ForeignKey("OrderId")]
    public Order Order { get; set; }
    [ForeignKey("SizeId")]
    public Size Size { get; set; }
}