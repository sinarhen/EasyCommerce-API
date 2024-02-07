using ECommerce.Models.Entities;

namespace ECommerce.Data.Repositories;

public class BaseRepository
{
    protected readonly ProductDbContext _db;
    

    protected BaseRepository(ProductDbContext db)
    {
        _db = db;
    }
    protected async Task SaveChangesAsyncWithTransaction()
    {
        await using var transaction = await _db.Database.BeginTransactionAsync();
        try
        {
            await _db.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
    
    protected int CalculateDepth(ECommerce.Models.Entities.Category category)
    {
        int depth = 1;
        while (category.ParentCategory != null)
        {
            depth++;
            category = category.ParentCategory;
        }
        return depth;
    }
    
    protected static bool ValidateOwner(string userId, string ownerId, bool? isAdmin = false)
    {
        bool isAdminValue = isAdmin.HasValue && isAdmin.Value;
        return (ownerId == userId || isAdminValue);
    }
    protected void AddToCategories(ECommerce.Models.Entities.Category category, ECommerce.Models.Entities.Product product, int order)
    {
        var currentCategory = category;
        while (currentCategory != null)
        {
            product.Categories.Add(new ProductCategory
            {
                ProductId = product.Id,
                CategoryId = currentCategory.Id,
                Order = order
            });

            order--;
            currentCategory = currentCategory.ParentCategory;
        }
    }
    
    
}
