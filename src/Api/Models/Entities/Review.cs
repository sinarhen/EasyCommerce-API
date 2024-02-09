using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ECommerce.Entities.Enum;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Models.Entities;

[PrimaryKey("Id")]
public class Review : BaseEntity
{
    [Key] public Guid Id { get; set; }

    [Key] public Guid ProductId { get; set; }

    [Key] public string CustomerId { get; set; }

    public string Title { get; set; }
    public string Content { get; set; }
    public Rating Rating { get; set; } = Rating.NoRating;

    // Navigation properties
    [ForeignKey("ProductId")] public Product Product { get; set; }

    [ForeignKey("CustomerId")] public User User { get; set; }
}