using ECommerce.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Data.Repositories.Category;

public class CategoryRepository : ICategoryRepository
{
    private readonly ProductDbContext _dbContext;

    public CategoryRepository(ProductDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<ActionResult<List<ECommerce.Models.Entities.Category>>> GetCategoriesAsync()
    {
        return await _dbContext.Categories.ToListAsync();
    }

    public Task<ActionResult> GetCategoryAsync(Guid id, Guid? sizeId = null, Guid? colorId = null)
    {
        throw new NotImplementedException();
    }

    public Task<ActionResult> CreateCategoryAsync(WriteCategoryDto categoryDto)
    {
        throw new NotImplementedException();
    }

    public Task<ActionResult> UpdateCategoryAsync(Guid id, WriteCategoryDto productDto)
    {
        throw new NotImplementedException();
    }

    public Task<ActionResult> DeleteCategoryAsync(Guid id)
    {
        throw new NotImplementedException();
    }
}