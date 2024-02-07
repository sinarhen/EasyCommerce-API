using ECommerce.Entities.Enum;

namespace ECommerce.Models.DTOs.Review;


public class ReviewProductDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string ImageUrl { get; set; }
}