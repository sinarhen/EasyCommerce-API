using ECommerce.Entities.Enum;
using ECommerce.Models.DTOs.Review;
using ECommerce.Models.DTOs.User;

namespace ECommerce.Models.DTOs.Product;
public class ReviewDto
{
    public Guid Id { get; set; }
    public UserDto User { get; set; } = null;
    public string Title { get; set; }
    public string Content { get; set; }
    public Rating Rating { get; set; }
    public DateTime CreatedAt { get; set; }

    public ReviewProductDto Product { get; set; } = new ReviewProductDto();
}

public class UserReviewsDto
{
    public UserDto User { get; set; } = new UserDto();
    public List<ReviewDto> Reviews { get; set; } = new List<ReviewDto>();
}