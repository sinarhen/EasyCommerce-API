using Ecommerce.Models.DTOs;
using Ecommerce.RequestHelpers;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Data.Repositories.Category;

public interface ICategoryRepository
{
    Task<ActionResult> GetCategoriesAsync(SearchParams searchParams);
    Task<ActionResult> GetCategoryAsync(Guid id, Guid? sizeId = null, Guid? colorId = null);
    Task<ActionResult> CreateCategoryAsync(WriteCategoryDto categoryDto);
    Task<ActionResult> UpdateCategoryAsync(Guid id, WriteCategoryDto productDto);
    Task<ActionResult> DeleteCategoryAsync(Guid id);
    
}

public class WriteCategoryDto
{
    public string Name { get; set; }
    public Guid? ParentCategoryId { get; set; }
    public string ImageUrl { get; set; }
}