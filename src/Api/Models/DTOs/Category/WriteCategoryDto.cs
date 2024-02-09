namespace ECommerce.Models.DTOs.Category;

public class WriteCategoryDto
{
    public string Name { get; set; }
    public Guid? ParentCategoryId { get; set; }
    public string ImageUrl { get; set; }
}