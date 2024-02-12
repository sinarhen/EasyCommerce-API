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
        catch (Exception e)
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    protected int CalculateDepth(Models.Entities.Category category)
    {
        var depth = 1;
        while (category.ParentCategory != null)
        {
            depth++;
            category = category.ParentCategory;
        }

        return depth;
    }

    protected static bool ValidateOwner(string userId, string ownerId, bool? isAdmin = false)
    {
        var isAdminValue = isAdmin.HasValue && isAdmin.Value;
        return ownerId == userId || isAdminValue;
    }

    protected void AddToCategories(Models.Entities.Category category, Models.Entities.Product product, int order)
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