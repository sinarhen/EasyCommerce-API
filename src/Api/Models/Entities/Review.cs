namespace ECommerce.Models.Entities;

public class Review
{
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }
    public Guid CustomerId { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public int Rating { get; set; } // 1 - 5
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    
    // Navigation properties
    public Product Product { get; set; }
    public Customer Customer { get; set; }
}