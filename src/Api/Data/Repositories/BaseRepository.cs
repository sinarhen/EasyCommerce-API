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

    protected async Task<List<Models.Entities.Product>> FilterProductsBySearchParams(ProductSearchParams searchParams)
    {
        var query = _db.Products.AsNoTracking().AsSplitQuery();

        query = searchParams.FilterBy switch
        {
            "in_stock" => query.Where(p => p.Stocks.Any(s => s.Stock > 0)),
            "discount" => query.Where(p => p.Discount > 0),
            "out_of_stock" => query.Where(p => p.Stocks.All(s => s.Stock == 0)),
            "new" => query.Where(p => p.CreatedAt > DateTime.Now.AddDays(-7)),
            _ => query,
        };
        
        List<string> categories = null;
        List<string> sizes = null;

        if (!string.IsNullOrEmpty(searchParams.Category))
        {
            categories = searchParams.Category.Split(",").Select(c => c.Trim().ToLower()).ToList();
        }

        if (!string.IsNullOrEmpty(searchParams.Size))
        {
            sizes = searchParams.Size.Split(",").Select(c => c.Trim().ToLower()).ToList();
        }

        if (categories != null && sizes != null)
        {
            query = query.Where(p =>
                p.Categories.Any(pc => pc.Category.Sizes.Any(s => sizes.Contains(s.Size.Name.ToLower())) && categories.Contains(pc.Category.Name.ToLower()) || categories.Contains(pc.CategoryId.ToString())) 
            );
        }
        else
        {
            if (categories != null)
            {
                query = query.Where(p => p.Categories.Any(pc => categories.Contains(pc.Category.Name.ToLower()) || categories.Contains(pc.CategoryId.ToString())));
            }
            if (sizes != null)
            {
                query = query.Where(p => p.Stocks.Any(s => sizes.Contains(s.Size.Name.ToLower())));
            }
        }

        if (searchParams.CollectionId != Guid.Empty)
        {
            query = query.Where(p => p.CollectionId == searchParams.CollectionId);
        }
        
        if (!string.IsNullOrEmpty(searchParams.Color))
        {
            var colors = searchParams.Color.Split(',').Select(c => c.Trim().ToLower()).ToList();
            query = query.Where(p => p.Stocks.Any(s => colors.Contains(s.Color.Name.ToLower())));
        }
        
        if (!string.IsNullOrEmpty(searchParams.Occasion))
        {
            var occasions = searchParams.Occasion.Split(",").Select(c => c.Trim().ToLower()).ToList();
            query = query.Where(p => occasions.Contains(p.Occasion.Name.ToLower()));
        } 
        
        if (!string.IsNullOrEmpty(searchParams.Material))
        {
            var materials = searchParams.Material.Split(",").Select(c => c.Trim().ToLower()).ToList();
            query = query.Where(p => p.Materials.Any(pm => materials.Contains(pm.Material.Name.ToLower())));
        }
        
        if (!string.IsNullOrEmpty(searchParams.SearchTerm))
        {
            var searchTerm = searchParams.SearchTerm.Trim().ToLower();
            query = query.Where(p => p.Name.ToLower().Contains(searchTerm));
        }

        query = searchParams.OrderBy switch
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

        query = query
            .Skip(searchParams.PageSize * (searchParams.PageNumber - 1))
            .Take(searchParams.PageSize);

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
