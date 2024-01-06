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
    

    protected void AddToCategories(ECommerce.Models.Entities.Category category, ECommerce.Models.Entities.Product product, int order)
    {
        product.Categories.Add(new ProductCategory
        {
            ProductId = product.Id,
            CategoryId = category.Id,
            Order = order
        });

        if (category.ParentCategory != null)
        {
            AddToCategories(category.ParentCategory, product, order - 1);
        }
    }

}
