using AutoMapper;
using ECommerce.Config;
using ECommerce.Entities.Enum;
using ECommerce.Models.DTOs;
using ECommerce.Models.DTOs.Color;
using ECommerce.Models.DTOs.Material;
using ECommerce.Models.DTOs.Product;
using ECommerce.Models.DTOs.Size;
using ECommerce.Models.DTOs.User;
using ECommerce.Models.Entities;
using ECommerce.RequestHelpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Data.Repositories.Product;

public class ProductRepository: BaseRepository, IProductRepository
{
    
    private readonly IMapper _mapper;
    private readonly UserManager<User> _userManager;
    public ProductRepository(ProductDbContext db, UserManager<User> userManager, IMapper mapper) : base(db)
    {
        _mapper = mapper;
        _userManager = userManager;
    }
    
    

    private void ClearProductCategories(ECommerce.Models.Entities.Product product)
    {
        _db.ProductCategories.RemoveRange(product.Categories);
    }
    
    private void ClearProductMaterials(ECommerce.Models.Entities.Product product)
    {
        _db.ProductMaterials.RemoveRange(product.Materials);
    }

    private void ClearProductStocks(ECommerce.Models.Entities.Product product)
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
            "new" => query.Where(p => p.CreatedAt > DateTimeOffset.UtcNow.AddDays(-7)),
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

    // TODO: FIX REDUNDANT CODE

    private static IQueryable<ProductDto> SelectAsProductDto(IQueryable<Models.Entities.Product> query)
    {
        return query.Select(p => new ProductDto
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
                }
                ,
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
                            // .Where(i => i.ColorId == ps.ColorId)
                            // .SelectMany(i => i.ImageUrls)
                            // .ToList(),
                        IsAvailable = p.Stocks.Any(stock => stock.Stock > 0),
                        Quantity = ps.Stock
                    })
                    .ToList(),
                IsAvailable = p.Stocks.Any(ps => ps.Stock > 0),
                MinPrice = p.Stocks.Any() ? p.Stocks.Min(s => s.Price) : 0,
                DiscountPrice = p.Stocks.Any() ? p.Stocks.Min(s => s.Price) * (decimal)(1 - p.Stocks.Average(s => s.Discount) / 100) : 0
            });
    }
    private async Task<ProductDetailsDto> GetProductDtoById(Guid id)
    {
        return await _db.Products
            .AsNoTracking()
            .Select(p => new ProductDetailsDto
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
                Images = p.Images
                    .Select(i => new ProductImageDto
                    {
                        ColorId = i.ColorId,
                        ImageUrls = i.ImageUrls
                    })
                    .ToList(),
                MainMaterial = new IdNameDto
                {
                    Id = p.Materials.First().Material.Id,
                    Name = p.Materials.First().Material.Name
                },
                Materials = p.Materials
                    .Select(pm => new MaterialDto
                    {
                        Id = pm.Material.Id,
                        Name = pm.Material.Name,
                        Percentage = pm.Percentage
                    })
                    .ToList(),
                Sizes = p.Stocks
                    .Select(ps => new SizeDto
                    {
                        Id = ps.Size.Id,
                        Name = ps.Size.Name,
                        Quantity = ps.Stock,
                        Value = ps.Size.Value,
                        IsAvailable = ps.Stock > 0
                    })
                    .ToList(),
                Reviews = p.Reviews.Select(r => new ReviewDto {
                    Customer = new UserDto
                    {
                        Id = r.User.Id,
                        Username = r.User.UserName,
                        Email = r.User.Email,
                        ImageUrl = r.User.ImageUrl,
                        CreatedAt = r.User.CreatedAt,
                        UpdatedAt = r.User.UpdatedAt,
                        Role = UserRoles.GetHighestUserRole(_userManager.GetRolesAsync(r.User).Result),    

                    },
                    Title = r.Title,
                    Content = r.Content,
                    Rating = r.Rating,
                    CreatedAt = r.CreatedAt,

                }).ToList(),
                
                SizeChartImageUrl = p.SizeChartImageUrl,

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
                            // .Where(i => i.ColorId == ps.ColorId)
                            // .SelectMany(i => i.ImageUrls)
                            // .ToList(),
                        IsAvailable = p.Stocks.Any(stock => stock.Stock > 0),
                        Quantity = ps.Stock
                    })
                    .ToList(),
                IsAvailable = p.Stocks.Any(ps => ps.Stock > 0),
                MinPrice = p.Stocks.Any() ? p.Stocks.Min(s => s.Price) : 0,
                DiscountPrice = p.Stocks.Any() ? p.Stocks.Min(s => s.Price) * (decimal)(1 - p.Stocks.Average(s => s.Discount) / 100) : 0
            })
            .FirstOrDefaultAsync(p => p.Id == id);
        
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

        return await SelectAsProductDto(query).ToListAsync();;
    }
    

    
    private static void ValidateCreateProductDtoOnModelLevel(CreateProductDto productDto)
    {
        if (productDto == null)
        {
            throw new ArgumentException("Request body is empty");
        }
        
        if (productDto.Stocks.Any(s => s.SizeId == Guid.Empty || s.ColorId == Guid.Empty || s.Stock <= 0 || s.Price <= 0))
        {
            throw new ArgumentException("Product stock must have valid size, color, stock and price");
        }
        if (productDto.Materials.Any(m => m.Id == Guid.Empty))
        {
            throw new ArgumentException("Product must have valid material id");
        }
        if (productDto.MainMaterialId == Guid.Empty)
        {
            throw new ArgumentException("Product must have valid main material id");
        }
        if (productDto.OccasionId == Guid.Empty)
        {
            throw new ArgumentException("Product must have valid occasion id");
        }
        if (string.IsNullOrEmpty(productDto.Name))
        {
            throw new ArgumentException("Product must have name");
        }
        if (productDto.Discount is < 0 or > 1)
        {
            throw new ArgumentException("Product discount must be between 0 and 1(represents percentage)");
        }
        
        // Check if gender is a valid value from Gender Enum
        if (!Enum.TryParse<Gender>(productDto.Gender, out var gender))
        {
            throw new ArgumentException($"Invalid gender value. Valid genders are: ({string.Join(", ", Enum.GetNames<Gender>())})");
        }
        
        // Check if season is a valid value from Season Enum
        if (!Enum.TryParse<Season>(productDto.Season, out var season))
        {
            throw new ArgumentException($"Invalid season value. Valid seasons are: ({string.Join(", ", Enum.GetNames<Season>())})");
        }
        
    }
            
    public async Task<IEnumerable<ProductDto>> GetProductsAsync(ProductSearchParams searchParams)
    {
        return await FilterProductsBySearchParams(searchParams);
        

    }
    

    public async Task<ProductDto> GetProductAsync(Guid id)
    {
        return await GetProductDtoById(id);
    }

    public async Task<Models.Entities.Product> CreateProductAsync(CreateProductDto productDto, string userId, bool isAdmin)
    {

        var existingCollection = await _db.Collections.Include(collection => collection.Store).FirstOrDefaultAsync(collection => collection.Id == productDto.CollectionId) ?? throw new ArgumentException($"CollectionId with ID {productDto.CollectionId} does not exist");
        var isOwner = ValidateOwner(userId, existingCollection.Store.OwnerId, isAdmin);
        if (!isOwner)
        {
            throw new UnauthorizedAccessException("You are not the owner of this store");
        }
        ValidateCreateProductDtoOnModelLevel(productDto);
        
        var nonExistingMaterialIds = productDto.Materials
            .Where(m => !_db.Materials.Any(material => material.Id == m.Id))
            .Select(m => m.Id)
            .ToList();
        
        var nonExistingColors = productDto.Stocks
            .Where(s => !_db.Colors.Any(color => color.Id == s.ColorId))
            .Select(color => color.ColorId)
            .ToList();
        
        if (nonExistingColors.Any())
        {
            throw new ArgumentException($"Colors with the following IDs do not exist: {string.Join(", ", nonExistingColors)}");
        }

        var nonExistingSizes = productDto.Stocks
            .Select(size => _db.Sizes.FirstOrDefault(s => s.Id == size.SizeId))
            .Where(size => size == null)
            .ToList();
        
        if (nonExistingSizes.Any())
        {
            throw new ArgumentException($"Sizes with the following IDs do not exist: {string.Join(", ", nonExistingSizes)}");
        }


        if (nonExistingMaterialIds.Any())
        {
            throw new ArgumentException($"Materials with the following IDs do not exist: {string.Join(", ", nonExistingMaterialIds)}");
        }
        if (nonExistingColors.Any())
        {
            throw new ArgumentException($"Colors with the following IDs do not exist: {string.Join(", ", nonExistingColors)}");
        }
        if (nonExistingSizes.Any())
        {
            throw new ArgumentException($"Sizes with the following IDs do not exist: {string.Join(", ", nonExistingSizes)}");
        }
        var existingMainMaterial = await _db.Materials.FirstOrDefaultAsync(material => material.Id == productDto.MainMaterialId) ?? throw new ArgumentException($"Main material with ID {productDto.MainMaterialId} does not exist");
        var existingOccasion = await _db.Occasions.FirstOrDefaultAsync(occasion => occasion.Id == productDto.OccasionId) ?? throw new ArgumentException($"Occasion with ID {productDto.OccasionId} does not exist");
        var category = await _db.Categories.FirstOrDefaultAsync(c => c.Id == productDto.CategoryId);
        if (productDto.CategoryId == Guid.Empty || category == null)
        {
            throw new ArgumentException($"Category with ID {productDto.CategoryId} does not exist");
        }
        
        var product = _mapper.Map<Models.Entities.Product>(productDto);
        
        
        product.Stocks = productDto.Stocks.Select(stockDto => new ProductStock
            {
                Product = product,
                ColorId = stockDto.ColorId,
                SizeId = stockDto.SizeId,
                Stock = stockDto.Stock,
                Price = stockDto.Price
            }).ToList();
        
        product.Materials = productDto.Materials.Select(material => new ProductMaterial
        {
            Product = product,
            Material = _db.Materials.FirstOrDefault(m => m.Id == material.Id),
            Percentage = material.Percentage
        }).ToList();
        
        product.Images = productDto.Images.Select(imageDto => new ProductImage
        {
            Product = product,
            Color = _db.Colors.FirstOrDefault(c => c.Id == imageDto.ColorId),
            ImageUrls = imageDto.ImageUrls
        }).ToList();
        
        int initialOrder = CalculateDepth(category);
        AddToCategories(category, product, initialOrder);

        await _db.Products.AddAsync(product);
        await SaveChangesAsyncWithTransaction();

        return product;
    }

    public async Task<Models.Entities.Product> UpdateProductAsync(Guid id, UpdateProductDto productDto, string userId, bool isAdmin)
    {
        var product = await _db.Products
            .Include(product => product.Stocks)
            .Include(product => product.Categories).ThenInclude(c => c.Category).ThenInclude(c => c.ParentCategory)
            .Include(product => product.Materials)
            .Include(product => product.Collection).ThenInclude(collection => collection.Store)
            .ThenInclude(store => store.Owner)
            
            .FirstOrDefaultAsync(p => p.Id == id);

        if (product == null)
        {
            throw new ArgumentException($"Product with ID {id} does not exist");
        }


        var ownerId = product.Collection.Store.OwnerId;
        
        if (!ValidateOwner(userId, ownerId, isAdmin))
        {
            throw new UnauthorizedAccessException("You have no permission to update this product");
        }


        if (!string.IsNullOrEmpty(productDto.Name))
        {
            product.Name = productDto.Name;
        }
           
        // checking if category id is not empty
        if (productDto.CategoryId != Guid.Empty && productDto.CategoryId != product.Categories.First().CategoryId)
        {
            // checking if category with given id exists
            var category = await _db.Categories.FirstOrDefaultAsync(c => c.Id == productDto.CategoryId);
            if (category == null)
            {
                throw new ArgumentException($"Category with ID {productDto.CategoryId} does not exist");
            }
            
            // clearing all categories from product
            ClearProductCategories(product);

            var initialOrder = CalculateDepth(category);
            AddToCategories(category, product, initialOrder);
        }
        
        
        if (!string.IsNullOrEmpty(productDto.Description))
        {
            product.Description = productDto.Description;
        }
        if (!string.IsNullOrEmpty(productDto.SizeChartImageUrl))
        {
            product.SizeChartImageUrl = productDto.SizeChartImageUrl;
        }

        if (!string.IsNullOrEmpty(productDto.Gender))
        {
            if (!Enum.TryParse<Gender>(productDto.Gender, out var gender))
            {
                throw new ArgumentException($"Invalid gender value. Valid gender options are: ({string.Join(", ", Enum.GetNames<Gender>())})");
            }

            product.Gender = gender;
        }

        if (!string.IsNullOrEmpty(productDto.Season))
        {
            if (!Enum.TryParse<Season>(productDto.Season, out var season))
            {
                throw new ArgumentException($"Invalid season value. Valid seasons are: ({string.Join(", ", Enum.GetNames<Season>())})");
            }
            product.Season = season;
        }
        
        if (productDto.OccasionId != Guid.Empty)
        {
            var occasion = await _db.Occasions.FirstOrDefaultAsync(o => o.Id == productDto.OccasionId);
            if (occasion == null)
            {
                throw new ArgumentException($"Occasion with ID {productDto.OccasionId} does not exist");
            }
            product.Occasion = occasion;
        }

        if (productDto.CollectionId != Guid.Empty)
        {
            var collection = await _db.Collections.FirstOrDefaultAsync(c => c.Id == productDto.CollectionId);
            if (collection == null)
            {
                throw new ArgumentException($"CollectionId with ID {productDto.CollectionId} does not exist");
            }
            product.Collection = collection;
        }
        
        if (productDto.Materials is { Count: > 0 })
        {
            var materials = await _db.Materials.AsNoTracking().ToListAsync();
            var nonExistingMaterialIds = productDto.Materials
                .Where(m => !materials.Any(material => material.Id == m.Id))
                .Select(m => m.Id)
                .ToList();
            
            if (nonExistingMaterialIds.Any())
            {
                throw new ArgumentException($"Materials with the following IDs do not exist: {string.Join(", ", nonExistingMaterialIds)}");
            }
            
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
            {
                throw new ArgumentException($"Colors with the following IDs do not exist: {string.Join(", ", nonExistingColors)}");
            }
            if (nonExistingSizes.Any())
            {
                throw new ArgumentException($"Sizes with the following IDs do not exist: {string.Join(", ", nonExistingSizes)}");
            }
            
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
            .Include(product => product.Stocks)
            .Include(product => product.Categories)
            .Include(product => product.Materials)
            .Include(product => product.Collection)
            .FirstOrDefaultAsync(p => p.Id == id);
        
        if (product == null)
        {
            throw new ArgumentException($"Product with ID {id} does not exist");
        }
        var ownerId = product.Collection.Store.OwnerId;
        ValidateOwner(userId, ownerId, isAdmin);

        ClearProductCategories(product);
        ClearProductMaterials(product);
        ClearProductStocks(product);
        
        _db.Products.Remove(product);
        await SaveChangesAsyncWithTransaction();
    }
    




}