using System.ComponentModel.DataAnnotations;
using ECommerce.Entities.Enum;
using ECommerce.Models.Enum;

namespace ECommerce.Models.DTOs.Review;

public class CreateReviewDto
{
    [Required]
    [StringLength(100, ErrorMessage = "Title is too long. max 100 characters.")]
    public string Title { get; set; }

    [StringLength(3000, ErrorMessage = "Content is too long. max 3000 characters.")]
    public string Content { get; set; }

    public Rating Rating { get; set; }
}