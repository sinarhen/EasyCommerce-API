namespace ECommerce.Models.Entities;

public class Order
{
    public Guid Id { get; set; }

    // Customer information
    public Customer Customer { get; set; }
    public string CustomerId { get; set; }

    // Product information
    public Product Product { get; set; }
    public Guid ProductId { get; set; } 

    public int Quantity { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public OrderStatus Status { get; set; }
}

public enum OrderStatus
{
    Pending,
    Processing,
    Shipped,
    Delivered,
    Cancelled
}