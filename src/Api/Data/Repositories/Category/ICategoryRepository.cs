using ECommerce.Models.DTOs;
using ECommerce.Models.Entities;


namespace Ecommerce.Data.Repositories.Category;

public interface ICategoryRepository
{
    Task<List<ECommerce.Models.Entities.Category>> GetCategoriesAsync();
    Task<ECommerce.Models.Entities.Category> GetCategoryAsync(Guid id);
    Task CreateCategoryAsync(WriteCategoryDto categoryDto);
    Task UpdateCategoryAsync(Guid id, WriteCategoryDto categoryDto);
    Task DeleteCategoryAsync(Guid id);
    
}