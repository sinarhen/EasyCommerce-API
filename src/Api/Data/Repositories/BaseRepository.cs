using ECommerce.Models.DTOs;
using ECommerce.Models.DTOs.Color;
using ECommerce.Models.DTOs.Product;
using ECommerce.Models.DTOs.Size;
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
            "discount" => query.Where(p => p.Stocks.Any(s => s.Discount > 0)),
            "out_of_stock" => query.Where(p => p.Stocks.All(s => s.Stock == 0)),
            "new" => query.Where(p => p.CreatedAt > DateTime.Now.AddDays(-7)),
            _ => query,
        };
    }

    private static IQueryable<ECommerce.Models.Entities.Product> ApplyCategoryFilter(IQueryable<ECommerce.Models.Entities.Product> query, ProductSearchParams searchParams)
    {
        if (Guid.Empty != searchParams.CategoryId)
        {
            query = query.Where(p => p.Categories.Any(pc => pc.CategoryId == searchParams.CategoryId));
        }
        return query;
    }


    private static List<string> SplitAndLowercase(string input)
    {
        return input.Split(",").Select(c => c.Trim().ToLower()).ToList();
    }
    private static IQueryable<ECommerce.Models.Entities.Product> ApplySizeFilter(IQueryable<ECommerce.Models.Entities.Product> query, ProductSearchParams searchParams)
    {
        if (searchParams.SizeId != Guid.Empty)
        {
            query = query.Where(p => p.Stocks.Any(s => s.SizeId == searchParams.SizeId));
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
        if (searchParams.ColorId != Guid.Empty)
        {
            query = query.Where(p => p.Stocks.Any(s => s.ColorId == searchParams.ColorId));
        }
        return query;
    }

    private static IQueryable<ECommerce.Models.Entities.Product> ApplyOccasionFilter(IQueryable<ECommerce.Models.Entities.Product> query, ProductSearchParams searchParams)
    {
        if (searchParams.OccasionId != Guid.Empty)
        {
            query = query.Where(p => p.OccasionId == searchParams.OccasionId);
        }
        return query;
    }

    private static IQueryable<ECommerce.Models.Entities.Product> ApplyMaterialFilter(IQueryable<ECommerce.Models.Entities.Product> query, ProductSearchParams searchParams)
    {
        if (searchParams.MaterialId != Guid.Empty)
        {
            query = query.Where(p => p.Materials.Any(pm => pm.MaterialId == searchParams.MaterialId));
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
    protected async Task<List<ProductDto>> FilterProductsBySearchParams(ProductSearchParams searchParams)
    {
        var query = _db.Products
            .Include(p => p.Stocks)
                .ThenInclude(ps => ps.Color)
            .Include(p => p.Images)
            .AsNoTracking().AsSplitQuery();

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
            .Select(p => new ProductDto
            {
                Id = p.Id,
                Collection = new IdNameDto
                {
                    Id = p.Collection.Id,
                    Name = p.Collection.Name
                },

                Categories = p.Categories
                    .Select(pc => new ProductCategoryDto
                    {
                        Id = pc.Category.Id,
                        Name = pc.Category.Name,
                        Order = pc.Order
                    })
                    .OrderBy(pc => pc.Order)
                    .ToArray(),
                Occasion = new IdNameDto
                {
                    Id = p.Occasion.Id,
                    Name = p.Occasion.Name
                },
                MainMaterialName = p.MainMaterial.Name,
                Name = p.Name,
                Description = p.Description,

                OrdersCount = p.Orders.Count,
                // OrdersCountLastMonth = p.Orders.Count(o => o.CreatedAt > DateTime.Now - TimeSpan.FromDays(30)),
                ReviewsCount = p.Reviews.Count,
                AvgRating = p.Reviews.Count == 0 ? 0 : p.Reviews.Average(r => r.Rating),
                // IsNew = p.CreatedAt > DateTime.Now - TimeSpan.FromDays(30),
                IsBestseller = p.Orders.Count > 10,
                CreatedAt = p.CreatedAt,
                UpdatedAt = p.UpdatedAt,
                Sizes = p.Stocks
                    .Select(ps => new SizeDto
                    {
                        Id = ps.Size.Id,
                        Name = ps.Size.Name,
                        Value = ps.Size.Value
                    })
                    .OrderBy(ps => ps.Value)
                    .ToList(),
                Colors = p.Stocks
                    .Select(ps => new ColorDto
                    {
                        Id = ps.Color.Id,
                        Name = ps.Color.Name,
                        HexCode = ps.Color.HexCode,
                        // ImageUrls = ps.Product.Images
                        //     .Where(i => i.ColorId == ps.ColorId)
                        //     .SelectMany(i => i.ImageUrls)
                        //     .ToList(),
                        IsAvailable = p.Stocks.Any(stock => stock.Stock > 0),
                        Quantity = ps.Stock
                    })
                    .ToList(),
                IsAvailable = p.Stocks.Any(ps => ps.Stock > 0),
                MinPrice = p.Stocks.Any() ? p.Stocks.Min(s => s.Price) : 0,
                DiscountPrice = p.Stocks.Any() ? p.Stocks.Min(s => s.Price) * (decimal)(1 - p.Stocks.Average(s => s.Discount) / 100) : 0
            })
            .ToListAsync();

        // Load the ImageUrls separately
        var imageUrls = await query
            .SelectMany(p => p.Images)
            .Select(i => new { i.ColorId, i.ImageUrls })
            .ToListAsync();

        foreach (var product in products)
        {
            foreach (var colorDto in product.Colors)
            {
                colorDto.ImageUrls = imageUrls
                    .Where(i => i.ColorId == colorDto.Id)
                    .SelectMany(i => i.ImageUrls)
                    .ToList();
            }
        }
        return products;
    }

}
