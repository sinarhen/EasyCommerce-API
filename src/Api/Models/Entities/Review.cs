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

    [Required]
    [StringLength(100, ErrorMessage = "Title is too long. max 100 characters.")]
    public string Title { get; set; }

    [StringLength(3000, ErrorMessage = "Content is too long. max 3000 characters.")]
    public string Content { get; set; }

    public Rating Rating { get; set; } = Rating.NoRating;

    // Navigation properties
    [ForeignKey("ProductId")] public Product Product { get; set; }

    [ForeignKey("CustomerId")] public User User { get; set; }
}