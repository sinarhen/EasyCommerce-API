using ECommerce.Models.DTOs;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Data.Repositories.Category;

public class CategoryRepository : BaseRepository, ICategoryRepository
{
    public CategoryRepository(ProductDbContext db) : base(db)
    {
        
    }
    public async Task<List<ECommerce.Models.Entities.Category>> GetCategoriesAsync()
    {
        return await _db.Categories.AsNoTracking()
            .Include(c => c.SubCategories)
            .Where(c => c.ParentCategoryId == null)
            .ToListAsync();
    }
    public async Task<ECommerce.Models.Entities.Category> GetCategoryAsync(Guid id)
    {
        return await _db.Categories
            .Include(c => c.SubCategories)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<bool> CreateCategoryAsync(WriteCategoryDto categoryDto)
    {
        var category = new ECommerce.Models.Entities.Category
        {
            ParentCategoryId = categoryDto.ParentCategoryId,
            Name = categoryDto.Name,
            ImageUrl = categoryDto.ImageUrl
        };
        
        await _db.Categories.AddAsync(category);
        return await _db.SaveChangesAsync() > 0;
    }

    public Task UpdateCategoryAsync(Guid id, WriteCategoryDto productDto)
    {
        throw new NotImplementedException();
    }

    public Task DeleteCategoryAsync(Guid id)
    {
        throw new NotImplementedException();
    }
}