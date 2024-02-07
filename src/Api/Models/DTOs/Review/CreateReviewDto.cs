using ECommerce.Entities.Enum;

namespace ECommerce.Models.DTOs.Review;


public class CreateReviewDto
{
    public string Title { get; set; }
    public string Content { get; set; }
    public Rating Rating { get; set; }
}