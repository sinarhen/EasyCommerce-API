using System.ComponentModel.DataAnnotations;

namespace ECommerce.Models.DTOs.Category;

public class WriteCategoryDto
{
    [Required]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "Name must be between 3 and 100 characters.")]
    public string Name { get; set; }
    
    public Guid? ParentCategoryId { get; set; }
    
    [Url]
    public string ImageUrl { get; set; }
}