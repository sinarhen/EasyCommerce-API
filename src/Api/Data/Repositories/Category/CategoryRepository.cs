using ECommerce.Models.DTOs;
using ECommerce.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Data.Repositories.Category;

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

    public async Task UpdateCategoryAsync(Guid id, WriteCategoryDto categoryDto)
    {
        var category = await _db.Categories.Include(c => c.Products).ThenInclude(productCategory => productCategory.Product)
            .FirstOrDefaultAsync(c => c.Id == id);
        if (category == null)
        {
            throw new Exception("Category not found");
        }
        
        // Get the old parent categories

        if (categoryDto.ParentCategoryId != null)
        {
            category.ParentCategoryId = categoryDto.ParentCategoryId;

            if (category.ParentCategoryId != categoryDto.ParentCategoryId)
            {
                // Update the ProductCategory records
                var newParents = await GetParentCategories(category);
                await UpdateProductCategories(category.Products, newParents);
            }
        }
        
        // Rest of the update logic...

        _db.Categories.Update(category);
        await SaveChangesAsyncWithTransaction();
    }
    private async Task<List<Models.Entities.Category>> GetParentCategories(Models.Entities.Category category)
    {
        var parents = new List<Models.Entities.Category>();
        var current = category;
        while (current.ParentCategoryId != null)
        {
            current = await _db.Categories.FindAsync(current.ParentCategoryId);
            parents.Add(current);
        }
        return parents;
    }



    private async Task UpdateProductCategories(ICollection<ProductCategory> productCategories)
    {
        foreach (var productCategory in productCategories)
        {
            // Delete the old ProductCategory records
            var oldProductCategories = _db.ProductCategories.Where(pc => pc.ProductId == productCategory.ProductId);
            _db.ProductCategories.RemoveRange(oldProductCategories);
        }

        await _db.SaveChangesAsync();
    }
    public async Task DeleteCategoryAsync(Guid id)
    {
        var category = await _db.Categories.FirstOrDefaultAsync(c => c.Id == id);
        if (category == null)
        {
            throw new Exception("Category not found");
        }
    }

}