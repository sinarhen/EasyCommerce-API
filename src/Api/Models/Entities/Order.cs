using System.ComponentModel.DataAnnotations;

namespace ECommerce.Models.Entities;

public class Order
{
    [Key]
    public Guid Id { get; set; }
    
    public Guid CustomerId { get; set; }
    
    public Guid ProductId { get; set; }

    public int Quantity { get; set; }
    public Color Color { get; set; }
    public string Size { get; set; }
    
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public OrderStatus Status { get; set; }
    
    // Navigation properties
    public Customer Customer { get; set; }
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