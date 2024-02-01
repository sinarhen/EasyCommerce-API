using ECommerce.Models.Entities;
using ECommerce.RequestHelpers;
using Microsoft.EntityFrameworkCore;

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
    
    protected static bool ValidateOwner(string userId, string ownerId, bool? isAdmin)
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
    protected void ClearProductCategories(ECommerce.Models.Entities.Product product)
    {
        _db.ProductCategories.RemoveRange(product.Categories);
    }
    
    protected void ClearProductMaterials(ECommerce.Models.Entities.Product product)
    {
        _db.ProductMaterials.RemoveRange(product.Materials);
    }

    protected void ClearProductStocks(ECommerce.Models.Entities.Product product)
    {
        _db.ProductStocks.RemoveRange(product.Stocks);
    }
    private static IQueryable<ECommerce.Models.Entities.Product> ApplyFilterBy(IQueryable<ECommerce.Models.Entities.Product> query, ProductSearchParams searchParams)
    {
        return searchParams.FilterBy switch
        {
            "in_stock" => query.Where(p => p.Stocks.Any(s => s.Stock > 0)),
            "discount" => query.Where(p => p.Discount > 0),
            "out_of_stock" => query.Where(p => p.Stocks.All(s => s.Stock == 0)),
            "new" => query.Where(p => p.CreatedAt > DateTime.Now.AddDays(-7)),
            _ => query,
        };
    }

    private static IQueryable<ECommerce.Models.Entities.Product> ApplyCategoryFilter(IQueryable<ECommerce.Models.Entities.Product> query, ProductSearchParams searchParams)
    {
        if (!string.IsNullOrEmpty(searchParams.Category))
        {
            var categories = SplitAndLowercase(searchParams.Category);
            query = query.Where(p => p.Categories.Any(pc => categories.Contains(pc.Category.Name.ToLower()) || categories.Contains(pc.CategoryId.ToString())));
        }
        return query;
    }


    private static List<string> SplitAndLowercase(string input)
    {
        return input.Split(",").Select(c => c.Trim().ToLower()).ToList();
    }
    private static IQueryable<ECommerce.Models.Entities.Product> ApplySizeFilter(IQueryable<ECommerce.Models.Entities.Product> query, ProductSearchParams searchParams)
    {
        if (!string.IsNullOrEmpty(searchParams.Size))
        {
            var sizes = SplitAndLowercase(searchParams.Size);
            query = query.Where(p => p.Stocks.Any(s => sizes.Contains(s.Size.Name.ToLower())));
        }
        return query;
    }
    
    private static IQueryable<ECommerce.Models.Entities.Product> ApplyCollectionFilter(IQueryable<ECommerce.Models.Entities.Product> query, ProductSearchParams searchParams)
    {
        if (searchParams.CollectionId != Guid.Empty)
        {
            query = query.Where(p => p.CollectionId == searchParams.CollectionId);
        }
        return query;
    }

    private static IQueryable<ECommerce.Models.Entities.Product> ApplyColorFilter(IQueryable<ECommerce.Models.Entities.Product> query, ProductSearchParams searchParams)
    {
        if (!string.IsNullOrEmpty(searchParams.Color))
        {
            var colors = SplitAndLowercase(searchParams.Color);
            query = query.Where(p => p.Stocks.Any(s => colors.Contains(s.Color.Name.ToLower())));
        }
        return query;
    }

    private static IQueryable<ECommerce.Models.Entities.Product> ApplyOccasionFilter(IQueryable<ECommerce.Models.Entities.Product> query, ProductSearchParams searchParams)
    {
        if (!string.IsNullOrEmpty(searchParams.Occasion))
        {
            var occasions = SplitAndLowercase(searchParams.Occasion);
            query = query.Where(p => occasions.Contains(p.Occasion.Name.ToLower()));
        }
        return query;
    }

    private static IQueryable<ECommerce.Models.Entities.Product> ApplyMaterialFilter(IQueryable<ECommerce.Models.Entities.Product> query, ProductSearchParams searchParams)
    {
        if (!string.IsNullOrEmpty(searchParams.Material))
        {
            var materials = SplitAndLowercase(searchParams.Material);
            query = query.Where(p => p.Materials.Any(pm => materials.Contains(pm.Material.Name.ToLower())));
        }
        return query;
    }

    private static IQueryable<ECommerce.Models.Entities.Product> ApplySearchTermFilter(IQueryable<ECommerce.Models.Entities.Product> query, ProductSearchParams searchParams)
    {
        if (!string.IsNullOrEmpty(searchParams.SearchTerm))
        {
            var searchTerm = searchParams.SearchTerm.Trim().ToLower();
            query = query.Where(p => p.Name.ToLower().Contains(searchTerm));
        }
        return query;
    }

    private static IQueryable<ECommerce.Models.Entities.Product> ApplyOrderBy(IQueryable<ECommerce.Models.Entities.Product> query, ProductSearchParams searchParams)
    {
        return searchParams.OrderBy switch
        {
            "price" => query.OrderBy(p => p.Stocks.Any() ? p.Stocks.Min(s => s.Price) : decimal.MaxValue),
            "price_desc" => query.OrderByDescending(p => p.Stocks.Any() ? p.Stocks.Min(s => s.Price): decimal.MaxValue),
            "name" => query.OrderBy(p => p.Name),
            "name_desc" => query.OrderByDescending(p => p.Name),
            "newest" => query.OrderByDescending(p => p.CreatedAt),
            "oldest" => query.OrderBy(p => p.CreatedAt),
            "bestseller" => query.OrderBy(p => p.Orders.Count),
            _ => query.OrderBy(p => p.Name)
        };
    }

    private static IQueryable<ECommerce.Models.Entities.Product> ApplyPaging(IQueryable<ECommerce.Models.Entities.Product> query, ProductSearchParams searchParams)
    {
        return query
            .Skip(searchParams.PageSize * (searchParams.PageNumber - 1))
            .Take(searchParams.PageSize);
    }
    protected async Task<List<Models.Entities.Product>> FilterProductsBySearchParams(ProductSearchParams searchParams)
    {
        var query = _db.Products.AsNoTracking().AsSplitQuery();

        query = ApplyFilterBy(query, searchParams);
        query = ApplyCategoryFilter(query, searchParams);
        query = ApplySizeFilter(query, searchParams);
        query = ApplyCollectionFilter(query, searchParams);
        query = ApplyColorFilter(query, searchParams);
        query = ApplyOccasionFilter(query, searchParams);
        query = ApplyMaterialFilter(query, searchParams);
        query = ApplySearchTermFilter(query, searchParams);
        query = ApplyOrderBy(query, searchParams);
        query = ApplyPaging(query, searchParams);

        var products = await query
            .Include(p => p.Categories).ThenInclude(productCategory => productCategory.Category)
            .Include(p => p.Occasion)
            .Include(p => p.MainMaterial)
            .Include(p => p.Stocks).ThenInclude(s => s.Color)
            .Include(p => p.Stocks).ThenInclude(s => s.Size)
            .Include(p => p.Images)
            .Include(p => p.Materials).ThenInclude(m => m.Material)
            .Include(product => product.Reviews)
            .Include(product => product.Orders)
            .Include(product => product.Collection)
            .ToListAsync();
        return products;
    }

}
