namespace ECommerce.Models.DTOs.Category;public class CategoryDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string ImageUrl { get; set; }
    public List<CategoryDto> SubCategories { get; set; }
}