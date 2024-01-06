using ECommerce.Models.DTOs;
using Microsoft.AspNetCore.Mvc;


namespace Ecommerce.Data.Repositories.Category;

public interface ICategoryRepository
{
    Task<ActionResult<List<ECommerce.Models.Entities.Category>>> GetCategoriesAsync();
    Task<ActionResult> GetCategoryAsync(Guid id, Guid? sizeId = null, Guid? colorId = null);
    Task<ActionResult> CreateCategoryAsync(WriteCategoryDto categoryDto);
    Task<ActionResult> UpdateCategoryAsync(Guid id, WriteCategoryDto productDto);
    Task<ActionResult> DeleteCategoryAsync(Guid id);
    
}