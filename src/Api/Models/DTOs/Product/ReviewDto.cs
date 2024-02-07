using ECommerce.Entities.Enum;
using ECommerce.Models.DTOs.Review;

namespace ECommerce.Models.DTOs.Product;
public class ReviewDto
{
    public Guid Id { get; set; }
    public string CustomerName { get; set; } 
    public string Title { get; set; }
    public string Content { get; set; }
    public Rating Rating { get; set; }
    public DateTime CreatedAt { get; set; }

    public ReviewProductDto Product { get; set; } = new ReviewProductDto();
}