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
    var category = await _db.Categories.AsNoTracking()
        .Include(c => c.Products)
        .ThenInclude(productCategory => productCategory.Product)
        .FirstOrDefaultAsync(c => c.Id == id);
    if (category == null)
    {
        throw new ArgumentException($"Category not found: {id}");
    }
    
    var parentCategory = await _db.Categories.AsNoTracking()
        .FirstOrDefaultAsync(c => c.Id == categoryDto.ParentCategoryId);
    
    if (parentCategory != null)
    {
        await UpdateProductCategories(category, parentCategory);

        category.ParentCategoryId = categoryDto.ParentCategoryId;
    }
    
    // Rest of the update logic...

    _db.Categories.Update(category);
    await _db.SaveChangesAsync();
}

private async Task UpdateProductCategories(Models.Entities.Category category, Models.Entities.Category newParentCategory)
{
    var products = category.Products.ToList();
    var productCategoriesToRemove = new List<ProductCategory>();
    foreach (var product in products)
    {
        var productCategories = await _db.ProductCategories.AsNoTracking()
            .Where(pc => pc.ProductId == product.ProductId && pc.CategoryId != category.Id)
            .ToListAsync();
        productCategoriesToRemove.AddRange(productCategories);
    
        AddToCategories(newParentCategory, product.Product, CalculateDepth(category));
    }

    _db.ProductCategories.RemoveRange(productCategoriesToRemove);
}
    public async Task DeleteCategoryAsync(Guid id)
    {
        var category = await _db.Categories.Include(category => category.Products)
            .ThenInclude(productCategory => productCategory.Product).FirstOrDefaultAsync(c => c.Id == id);
        if (category == null)
        {
            throw new Exception("Category not found");
        }

        category.Products.Clear();
        _db.Categories.Remove(category);
        
        await SaveChangesAsyncWithTransaction();
    }

}