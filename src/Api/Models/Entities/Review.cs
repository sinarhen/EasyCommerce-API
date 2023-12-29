using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Models.Entities;

[PrimaryKey("Id")]
public class Review
{
    [Key]
    public Guid Id { get; set; }
    
    [Key]
    public Guid ProductId { get; set; }
    [Key]
    public string CustomerId { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public Rating Rating { get; set; } // 1 - 5
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    
    // Navigation properties
    [ForeignKey("ProductId")]
    public Product Product { get; set; }
    [ForeignKey("CustomerId")]
    public Customer Customer { get; set; }
}

public enum Rating
{
    NoRating = 0,
    Terrible = 1,
    Bad = 2,
    Average = 3,
    Good = 4,
    Excellent = 5
}