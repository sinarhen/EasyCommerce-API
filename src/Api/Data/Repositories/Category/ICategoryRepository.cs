using ECommerce.Models.DTOs.Category;

namespace ECommerce.Data.Repositories.Category;

public interface ICategoryRepository
{
    Task<List<Models.Entities.Category>> GetCategoriesAsync();
    Task<Models.Entities.Category> GetCategoryAsync(Guid id);
    Task CreateCategoryAsync(WriteCategoryDto categoryDto);
    Task UpdateCategoryAsync(Guid id, WriteCategoryDto categoryDto);
    Task DeleteCategoryAsync(Guid id);
}