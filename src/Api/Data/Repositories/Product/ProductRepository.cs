using AutoMapper;
using ECommerce.Entities.Enum;
using ECommerce.Models.DTOs;
using ECommerce.Models.DTOs.Color;
using ECommerce.Models.DTOs.Material;
using ECommerce.Models.DTOs.Product;
using ECommerce.Models.DTOs.Size;
using ECommerce.Models.DTOs.Stock;
using ECommerce.Models.DTOs.User;
using ECommerce.Models.Entities;
using ECommerce.Models.Enum;
using ECommerce.RequestHelpers;
using ECommerce.RequestHelpers.SearchParams;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Data.Repositories.Product;

public class ProductRepository : BaseRepository, IProductRepository
{
    private readonly IMapper _mapper;

    public ProductRepository(ProductDbContext db, IMapper mapper) : base(db)
    {
        _mapper = mapper;
    }

    public async Task<IEnumerable<ProductDto>> GetProductsAsync(ProductSearchParams searchParams)
    {
        return await FilterProductsBySearchParams(searchParams);
    }


    public async Task<ProductDetailsDto> GetProductAsync(Guid id)
    {
        return await GetProductDtoById(id);
    }
    
    public async Task<Models.Entities.Product> CreateProductAsync(CreateProductDto productDto, string userId,
        bool isAdmin)
    {
        var ownerId = await _db.Collections
            .AsNoTracking()
            .Where(c => c.Id == productDto.CollectionId)
            .Select(c => c.Store.OwnerId)
            .FirstOrDefaultAsync();
        
        var isOwner = ValidateOwner(userId, ownerId, isAdmin);
        if (!isOwner) throw new UnauthorizedAccessException("You are not the owner of this store");
        
        
        ValidateCreateProductDtoOnModelLevel(productDto);
        
        var colorIds = productDto.Stocks.Select(s => s.ColorId).ToList();
        var existingColorIds = await _db.Colors
            .AsNoTracking()
            .Where(c => colorIds.Contains(c.Id))
            .Select(c => c.Id)
            .ToListAsync();
        var nonExistingColors = colorIds.Except(existingColorIds).ToList();
        
        if (nonExistingColors.Any())
            throw new ArgumentException(
                $"Colors with the following IDs do not exist: {string.Join(", ", nonExistingColors)}");
        
        var sizeIds = productDto.Stocks.Select(s => s.SizeId).ToList();
        var existingSizes = await _db.Sizes
            .AsNoTracking()
            .Where(s => sizeIds.Contains(s.Id))
            .Select(s => s.Id)
            .ToListAsync();
        var nonExistingSizes = sizeIds.Except(existingSizes).ToList();
        if (nonExistingSizes.Any())
            throw new ArgumentException(
                $"Sizes with the following IDs do not exist: ${string.Join(", ", nonExistingSizes)}");
    
        
        var materialIds = productDto.Materials.Select(m => m.Id).ToList();
        var existingMaterialIds = await _db.Materials
            .AsNoTracking()
            .Where(m => materialIds.Contains(m.Id))
            .Select(m => m.Id)
            .ToListAsync();
        var nonExistingMaterialIds = materialIds.Except(existingMaterialIds).ToList();
        if (nonExistingMaterialIds.Any())
            throw new ArgumentException(
                $"Materials with the following IDs do not exist: {string.Join(", ", nonExistingMaterialIds)}");
        if (nonExistingColors.Any())
            throw new ArgumentException(
                $"Colors with the following IDs do not exist: {string.Join(", ", nonExistingColors)}");
        if (nonExistingSizes.Any())
            throw new ArgumentException(
                $"Sizes with the following IDs do not exist: {string.Join(", ", nonExistingSizes)}");
        
        var category = await _db.Categories.FirstOrDefaultAsync(c => c.Id == productDto.CategoryId) 
                       ?? throw new ArgumentException($"Category with ID {productDto.CategoryId} does not exist");
        
        var product = _mapper.Map<Models.Entities.Product>(productDto);
        if (product == null) throw new ArgumentException("Product mapping from dto is null");

        // Denormalization to avoid joins and improve performance
        product.SellerId = userId;
        
        await _db.Products.AddAsync(product);

        if (productDto.Stocks != null)
        {
            product.Stocks = productDto.Stocks.Select(stockDto => new ProductStock
            {
                ProductId = product.Id,
                ColorId = stockDto.ColorId,
                SizeId = stockDto.SizeId,
                Stock = stockDto.Stock,
                Price = stockDto.Price
            }).ToList();
    
        }
        
        if (productDto.Materials != null)
        {
            product.Materials = productDto.Materials.Select(material => new ProductMaterial
            {
                ProductId = product.Id,
                MaterialId = material.Id,
                Percentage = material.Percentage
            }).ToList();
        }
        
        if (productDto.Images != null)
        {
            product.Images = productDto.Images.Select(imageDto => new ProductImage
            {
                ProductId = product.Id,
                ColorId = imageDto.ColorId,
                ImageUrls = imageDto.ImageUrls
            }).ToList();
        }
            
        var initialOrder = CalculateDepth(category);
        AddToCategories(category, product, initialOrder);
        
        await SaveChangesAsyncWithTransaction();

        return product;
    }

    public async Task<Models.Entities.Product> UpdateProductAsync(Guid id, UpdateProductDto productDto, string userId,
        bool isAdmin)
    {
        
        // TODO: Optimize this method
        var product = await _db.Products
            .Include(product => product.Stocks)
            .Include(product => product.Categories).ThenInclude(c => c.Category).ThenInclude(c => c.ParentCategory)
            .Include(product => product.Materials)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (product == null) throw new ArgumentException($"Product with ID {id} does not exist");


        var ownerId = product.SellerId;

        if (!ValidateOwner(userId, ownerId, isAdmin))
            throw new UnauthorizedAccessException("You have no permission to update this product");


        if (!string.IsNullOrEmpty(productDto.Name)) product.Name = productDto.Name;

        // checking if category id is not empty
        if (productDto.CategoryId != Guid.Empty && productDto.CategoryId != product.Categories.First().CategoryId)
        {
            // checking if category with given id exists
            var category = await _db.Categories.FindAsync(productDto.CategoryId);
            if (category == null)
                throw new ArgumentException($"Category with ID {productDto.CategoryId} does not exist");

            // clearing all categories from product
            ClearProductCategories(product);

            var initialOrder = CalculateDepth(category);
            AddToCategories(category, product, initialOrder);
        }


        if (!string.IsNullOrEmpty(productDto.Description)) product.Description = productDto.Description;
        if (!string.IsNullOrEmpty(productDto.SizeChartImageUrl))
            product.SizeChartImageUrl = productDto.SizeChartImageUrl;

        if (!string.IsNullOrEmpty(productDto.Gender))
        {
            if (!Enum.TryParse<Gender>(productDto.Gender, out var gender))
                throw new ArgumentException(
                    $"Invalid gender value. Valid gender options are: ({string.Join(", ", Enum.GetNames<Gender>())})");

            product.Gender = gender;
        }

        if (!string.IsNullOrEmpty(productDto.Season))
        {
            if (!Enum.TryParse<Season>(productDto.Season, out var season))
                throw new ArgumentException(
                    $"Invalid season value. Valid seasons are: ({string.Join(", ", Enum.GetNames<Season>())})");
            product.Season = season;
        }

        if (productDto.OccasionId != Guid.Empty)
        {
            var occasion = await _db.Occasions.FirstOrDefaultAsync(o => o.Id == productDto.OccasionId);
            if (occasion == null)
                throw new ArgumentException($"Occasion with ID {productDto.OccasionId} does not exist");
            product.Occasion = occasion;
        }

        if (productDto.CollectionId != Guid.Empty)
        {
            var collection = await _db.Collections.FirstOrDefaultAsync(c => c.Id == productDto.CollectionId);
            if (collection == null)
                throw new ArgumentException($"CollectionId with ID {productDto.CollectionId} does not exist");
            product.Collection = collection;
        }

        if (productDto.Materials is { Count: > 0 })
        {
            var materials = await _db.Materials.AsNoTracking().ToListAsync();
            var nonExistingMaterialIds = productDto.Materials
                .Where(m => materials.All(material => material.Id != m.Id))
                .Select(m => m.Id)
                .ToList();

            if (nonExistingMaterialIds.Any())
                throw new ArgumentException(
                    $"Materials with the following IDs do not exist: {string.Join(", ", nonExistingMaterialIds)}");

            ClearProductMaterials(product);

            product.Materials = productDto.Materials.Select(material => new ProductMaterial
            {
                Product = product,
                Material = _db.Materials.FirstOrDefault(m => m.Id == material.Id),
                Percentage = material.Percentage
            }).ToList();
        }

        if (productDto.Stocks is { Count: > 0 })
        {
            var colors = await _db.Colors.AsNoTracking().ToListAsync();
            var sizes = await _db.Sizes.AsNoTracking().ToListAsync();

            var nonExistingColors = productDto.Stocks
                .Where(s => colors.All(color => color.Id != s.ColorId))
                .Select(color => color.ColorId)
                .ToList();

            var nonExistingSizes = productDto.Stocks
                .Where(size => sizes.All(s => s.Id != size.SizeId))
                .Select(s => s.SizeId)
                .ToList();

            if (nonExistingColors.Any())
                throw new ArgumentException(
                    $"Colors with the following IDs do not exist: {string.Join(", ", nonExistingColors)}");
            if (nonExistingSizes.Any())
                throw new ArgumentException(
                    $"Sizes with the following IDs do not exist: {string.Join(", ", nonExistingSizes)}");

            ClearProductStocks(product);
            product.Stocks = productDto.Stocks.Select(stockDto => new ProductStock
            {
                Product = product,
                ColorId = stockDto.ColorId,
                SizeId = stockDto.SizeId,
                Stock = stockDto.Stock,
                Price = stockDto.Price
            }).ToList();
        }


        await SaveChangesAsyncWithTransaction();

        return product;
    }

    public async Task DeleteProductAsync(Guid id, string userId, bool isAdmin)
    {
        var product = await _db.Products
            .FirstOrDefaultAsync(p => p.Id == id);

        if (product == null) throw new ArgumentException($"Product with ID {id} does not exist");
        var ownerId = product.SellerId;
        ValidateOwner(userId, ownerId, isAdmin);

        ClearProductCategories(product);
        ClearProductMaterials(product);
        ClearProductStocks(product);

        _db.Products.Remove(product);
        await SaveChangesAsyncWithTransaction();
    }


    private void ClearProductCategories(Models.Entities.Product product)
    {
        _db.ProductCategories.RemoveRange(product.Categories);
    }

    private void ClearProductMaterials(Models.Entities.Product product)
    {
        _db.ProductMaterials.RemoveRange(product.Materials);
    }

    private void ClearProductStocks(Models.Entities.Product product)
    {
        _db.ProductStocks.RemoveRange(product.Stocks);
    }

    private static IQueryable<Models.Entities.Product> ApplyFilterBy(IQueryable<Models.Entities.Product> query,
        ProductSearchParams searchParams)
    {
        return searchParams.FilterBy switch
        {
            "in_stock" => query.Where(p => p.Stocks.Any(s => s.Stock > 0)),
            "discount" => query.Where(p => p.Stocks.Any(s => s.Discount > 0)),
            "out_of_stock" => query.Where(p => p.Stocks.All(s => s.Stock == 0)),
            "new" => query.Where(p => p.CreatedAt > DateTimeOffset.UtcNow.AddDays(-7)),
            _ => query
        };
    }

    private static IQueryable<Models.Entities.Product> ApplyCategoryFilter(IQueryable<Models.Entities.Product> query,
        ProductSearchParams searchParams)
    {
        if (Guid.Empty != searchParams.CategoryId)
            query = query.Where(p => p.Categories.Any(pc => pc.CategoryId == searchParams.CategoryId));
        return query;
    }


    private static List<string> SplitAndLowercase(string input)
    {
        return input.Split(",").Select(c => c.Trim().ToLower()).ToList();
    }

    private static IQueryable<Models.Entities.Product> ApplySizeFilter(IQueryable<Models.Entities.Product> query,
        ProductSearchParams searchParams)
    {
        if (searchParams.SizeId != Guid.Empty)
            query = query.Where(p => p.Stocks.Any(s => s.SizeId == searchParams.SizeId));
        return query;
    }

    private static IQueryable<Models.Entities.Product> ApplyCollectionFilter(IQueryable<Models.Entities.Product> query,
        ProductSearchParams searchParams)
    {
        if (searchParams.CollectionId != Guid.Empty)
            query = query.Where(p => p.CollectionId == searchParams.CollectionId);
        return query;
    }

    private static IQueryable<Models.Entities.Product> ApplyColorFilter(IQueryable<Models.Entities.Product> query,
        ProductSearchParams searchParams)
    {
        if (searchParams.ColorId != Guid.Empty)
            query = query.Where(p => p.Stocks.Any(s => s.ColorId == searchParams.ColorId));
        return query;
    }

    private static IQueryable<Models.Entities.Product> ApplyOccasionFilter(IQueryable<Models.Entities.Product> query,
        ProductSearchParams searchParams)
    {
        if (searchParams.OccasionId != Guid.Empty) query = query.Where(p => p.OccasionId == searchParams.OccasionId);
        return query;
    }

    private static IQueryable<Models.Entities.Product> ApplyMaterialFilter(IQueryable<Models.Entities.Product> query,
        ProductSearchParams searchParams)
    {
        if (searchParams.MaterialId != Guid.Empty)
            query = query.Where(p => p.Materials.Any(pm => pm.MaterialId == searchParams.MaterialId));
        return query;
    }

    private static IQueryable<Models.Entities.Product> ApplySearchTermFilter(IQueryable<Models.Entities.Product> query,
        ProductSearchParams searchParams)
    {
        if (!string.IsNullOrEmpty(searchParams.SearchTerm))
        {
            var searchTerm = searchParams.SearchTerm.Trim().ToLower();
            query = query.Where(p => p.Name.ToLower().Contains(searchTerm));
        }

        return query;
    }

    private static IQueryable<Models.Entities.Product> ApplyOrderBy(IQueryable<Models.Entities.Product> query,
        ProductSearchParams searchParams)
    {
        return searchParams.OrderBy switch
        {
            ProductsOrderBy.Price => query.OrderBy(p => p.Stocks.Any() ? p.Stocks.Min(s => s.Price) : decimal.MaxValue),
            ProductsOrderBy.PriceDesc => query.OrderByDescending(p =>
                p.Stocks.Any() ? p.Stocks.Min(s => s.Price) : decimal.MaxValue),
            ProductsOrderBy.Name => query.OrderBy(p => p.Name),
            ProductsOrderBy.NameDesc => query.OrderByDescending(p => p.Name),
            ProductsOrderBy.Newest => query.OrderByDescending(p => p.CreatedAt),
            ProductsOrderBy.Oldest => query.OrderBy(p => p.CreatedAt),
            ProductsOrderBy.Bestseller => query.OrderBy(p => p.Orders.Count),
            _ => query.OrderBy(p => p.Name)
        };
    }

    private static IQueryable<Models.Entities.Product> ApplyPaging(IQueryable<Models.Entities.Product> query,
        ProductSearchParams searchParams)
    {
        return query
            .Skip(searchParams.PageSize * (searchParams.PageNumber - 1))
            .Take(searchParams.PageSize);
    }
    private async Task<ProductDetailsDto> GetProductDtoById(Guid id)
{
    var product = await _db.Products
        .AsNoTracking()
        .Include(p => p.Stocks)
        .ThenInclude(s => s.Color)
        .Include(p => p.Stocks)
        .ThenInclude(s => s.Size)
        .Include(p => p.Materials).ThenInclude(productMaterial => productMaterial.Material)
        .Include(p => p.Images)
        .Include(p => p.Reviews)
        .ThenInclude(r => r.User)
        .Include(p => p.Categories).ThenInclude(productCategory => productCategory.Category)
        .Include(product => product.Orders).Include(product => product.Occasion).Include(product => product.Collection)
        .FirstOrDefaultAsync(p => p.Id == id);

    if (product == null)
    {
        throw new ArgumentException($"Product with ID {id} does not exist");
    }


    var productDto = new ProductDetailsDto
    {
        Id = product.Id,
        Collection = new IdNameDto
        {
            Id = product.Collection.Id,
            Name = product.Collection.Name
        },
        Categories = product.Categories
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
            Id = product.Occasion.Id,
            Name = product.Occasion.Name
        },
        Images = product.Images
            .Select(i => new ProductImageDto
            {
                ColorId = i.ColorId,
                ImageUrls = i.ImageUrls
            })
            .ToList(),
        Materials = product.Materials
            .Select(pm => new MaterialDto
            {
                Id = pm.Material.Id,
                Name = pm.Material.Name,
                Percentage = pm.Percentage
            })
            .ToList(),
        Sizes = product.Stocks
            .Select(ps => new SizeDto
            {
                Id = ps.Size.Id,
                Name = ps.Size.Name,
                Value = ps.Size.Value,
            })
            .ToList(),
        Reviews = product.Reviews.Select(r => new ReviewDto
        {
            User = new UserDto
            {
                Id = r.User.Id,
                Username = r.User.UserName,
                ImageUrl = r.User.ImageUrl
            },
            Title = r.Title,
            Content = r.Content,
            Rating = r.Rating,
            CreatedAt = r.CreatedAt
        }).ToList(),
    
        SizeChartImageUrl = product.SizeChartImageUrl,
    
        Name = product.Name,
        Description = product.Description,
        Gender = product.Gender.ToString(),
        Season = product.Season.ToString(),
        OrdersCount = product.Orders.Count,
        ReviewsCount = product.Reviews.Count,
        AvgRating = product.Reviews.Count == 0 ? 0 : product.Reviews.Average(r => (int)r.Rating),
        IsNew = product.CreatedAt > DateTimeOffset.UtcNow - TimeSpan.FromDays(30),
        IsBestseller = product.Orders.Count > 10,
        CreatedAt = product.CreatedAt,
        UpdatedAt = product.UpdatedAt,
        Colors = product.Stocks
            .Select(ps => new ColorDto
            {
                Id = ps.Color.Id,
                Name = ps.Color.Name,
                HexCode = ps.Color.HexCode
            })
            .ToList(),
        Stocks = product.Stocks.Select(ps => new ProductStockDto
        {
            ColorId = ps.ColorId,
            SizeId = ps.SizeId,
            Stock = ps.Stock,
            Price = ps.Price,
            Discount = ps.Discount
        }).ToList(),
        IsAvailable = product.Stocks.Any(ps => ps.Stock > 0),
        MinPrice = product.Stocks.Any() ? product.Stocks.Min(s => s.Price) : 0,
    
    };
    
    return productDto;
}

    private async Task<List<ProductDto>> FilterProductsBySearchParams(ProductSearchParams searchParams)
    {
        var query = _db.Products
            .AsNoTracking()
            .AsSplitQuery();

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

        return await query.Select(p => new ProductDto
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
            Images = p.Images
                .Select(i => new ProductImageDto
                {
                    ColorId = i.ColorId,
                    ImageUrls = i.ImageUrls
                })
                .ToList(),

            Occasion = new IdNameDto
            {
                Id = p.Occasion.Id,
                Name = p.Occasion.Name
            },
            MainMaterial = new IdNameDto
            {
                Id = p.Materials.First().Material.Id,
                Name = p.Materials.First().Material.Name
            },
            Name = p.Name,
            Description = p.Description,
            Gender = p.Gender.ToString(),
            Season = p.Season.ToString(),
            OrdersCount = p.Orders.Count,
            ReviewsCount = p.Reviews.Count,
            AvgRating = p.Reviews.Count == 0 ? 0 : p.Reviews.Average(r => (int)r.Rating),
            IsNew = p.CreatedAt > DateTimeOffset.UtcNow - TimeSpan.FromDays(30),
            IsOnSale = p.Stocks.Any(s => s.Discount > 0),
            IsBestseller = p.Orders.Count > 10,
            CreatedAt = p.CreatedAt,
            UpdatedAt = p.UpdatedAt,
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
                })
                .ToList(),
            IsAvailable = p.Stocks.Any(ps => ps.Stock > 0),
            MinPrice = p.Stocks.Any() ? p.Stocks.Min(s => s.Price) : 0,
        }).ToListAsync();
        ;
    }


    private static void ValidateCreateProductDtoOnModelLevel(CreateProductDto productDto)
    {
        if (productDto == null) throw new ArgumentException("Request body is empty");
        
        if (productDto.CategoryId == Guid.Empty) throw new ArgumentException("Product must have valid category id");
        if (productDto.Stocks.Any(
                s => s.SizeId == Guid.Empty || s.ColorId == Guid.Empty || s.Stock <= 0 || s.Price <= 0))
            throw new ArgumentException("Product stock must have valid size, color, stock and price");
        if (productDto.Materials.Any(m => m.Id == Guid.Empty))
            throw new ArgumentException("Product must have valid material id");
        if (productDto.MainMaterialId == Guid.Empty)
            throw new ArgumentException("Product must have valid main material id");
        if (productDto.OccasionId == Guid.Empty) throw new ArgumentException("Product must have valid occasion id");
        if (string.IsNullOrEmpty(productDto.Name)) throw new ArgumentException("Product must have name");
        if (productDto.Discount is < 0 or > 1)
            throw new ArgumentException("Product discount must be between 0 and 1(represents percentage)");

        // Check if gender is a valid value from Gender Enum
        if (!Enum.TryParse<Gender>(productDto.Gender, out var gender))
            throw new ArgumentException(
                $"Invalid gender value. Valid genders are: ({string.Join(", ", Enum.GetNames<Gender>())})");

        // Check if season is a valid value from Season Enum
        if (!Enum.TryParse<Season>(productDto.Season, out var season))
            throw new ArgumentException(
                $"Invalid season value. Valid seasons are: ({string.Join(", ", Enum.GetNames<Season>())})");
    }
}