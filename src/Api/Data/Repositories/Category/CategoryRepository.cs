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
        return await _db.Categories.AsNoTracking()
            .Include(c => c.SubCategories)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task CreateCategoryAsync(WriteCategoryDto categoryDto)
    {
        var category = new ECommerce.Models.Entities.Category
        {
            ParentCategoryId = categoryDto.ParentCategoryId,
            Name = categoryDto.Name,
            ImageUrl = categoryDto.ImageUrl
        };
        
        await _db.Categories.AddAsync(category);
        await SaveChangesAsyncWithTransaction();
    }

    public async Task UpdateCategoryAsync(Guid id, WriteCategoryDto productDto)
    {
        var category = await _db.Categories.FirstOrDefaultAsync(c => c.Id == id);
        if (category == null)
        {
            throw new Exception("Category not found");
        }
        
        if (productDto.ParentCategoryId != null)
        {
            category.ParentCategoryId = productDto.ParentCategoryId;
        }
        if (productDto.Name != null)
        {
            category.Name = productDto.Name;
        }
        if (productDto.ImageUrl != null)
        {
            category.ImageUrl = productDto.ImageUrl;
        }
        
        _db.Categories.Update(category);
        
        await SaveChangesAsyncWithTransaction();
    }

    public Task DeleteCategoryAsync(Guid id)
    {
        throw new NotImplementedException();
    }
}