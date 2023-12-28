using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Models.Entities;

[PrimaryKey("Id")]
public class Order
{
    [Key]
    public Guid Id { get; set; }
    
    [Key]
    public string CustomerId { get; set; }
    
    [Key]
    public Guid ProductId { get; set; }

    [Key]
    public Guid ColorId { get; set; }
    
    public int Quantity { get; set; }
    
    
    public string Size { get; set; }
    
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public OrderStatus Status { get; set; }
    
    // Navigation properties
    [ForeignKey("ColorId")]
    public Color Color { get; set; }
    [ForeignKey("CustomerId")]
    public Customer Customer { get; set; }
    [ForeignKey("ProductId")]
    public Product Product { get; set; }
    
}

public enum OrderStatus
{
    Pending,
    Processing,
    Shipped,
    Delivered,
    Cancelled
}