using AutoMapper;
using ECommerce.Config;
using ECommerce.Models.DTOs;
using ECommerce.Models.Entities;
using ECommerce.RequestHelpers;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Data.Repositories.Product;

public class ProductRepository: BaseRepository, IProductRepository
{
    
    private readonly IMapper _mapper;
    public ProductRepository(ProductDbContext db, IMapper mapper) : base(db)
    {
        _mapper = mapper;
    }
    
    
    
    private static void ValidateOnModelLevel(CreateProductDto productDto)
    {
        if (productDto == null)
        {
            throw new ArgumentException("Request body is empty");
        }
        
        if (productDto.Stocks == null || productDto.Stocks.Count == 0)
        {
            throw new ArgumentException("Product must have at least one stock");
        }
        if (productDto.Stocks.Any(s => s.Sizes == null || s.Sizes.Count == 0))
        {
            throw new ArgumentException("Product stock must have at least one size");
        }
        if (productDto.Stocks.Any(s => s.Sizes.Any(size => size.Stock <= 0)))
        {
            throw new ArgumentException("Product stock must have at least one size with stock greater than 0");
        }
        if (productDto.Stocks.Any(s => s.Sizes.Any(size => size.Price <= 0)))
        {
            throw new ArgumentException("Product stock must have at least one size with price greater than 0");
        }
        if (productDto.Stocks.Any(s => s.Sizes.All(size => size.SizeId == Guid.Empty)))
        {
            throw new ArgumentException("Product stock has invalid size id");
        }
        if (productDto.Stocks.Any(s => s.ColorId == Guid.Empty))
        {
            throw new ArgumentException("Product stock must have valid color id");
        }
        if (productDto.Materials == null || productDto.Materials.Count == 0)
        {
            throw new ArgumentException("Product must have at least one material");
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
            
    public async Task<IEnumerable<Models.Entities.Product>> GetProductsAsync(SearchParams searchParams)
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

        await SaveChangesAsyncWithTransaction();
        
        return products;

    }
    

    public async Task<Models.Entities.Product> GetProductAsync(Guid id)
    {
        var product = await _db.Products.AsNoTracking().AsSplitQuery()
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
            .FirstOrDefaultAsync(p => p.Id == id);

        return product;
    }

    public async Task<Models.Entities.Product> CreateProductAsync(CreateProductDto productDto, string username, IEnumerable<string> roles)
    {
        ValidateOnModelLevel(productDto);
        
        var nonExistingMaterialIds = productDto.Materials
            .Where(m => !_db.Materials.Any(material => material.Id == m.Id))
            .Select(m => m.Id)
            .ToList();
        
        var nonExistingColors = productDto.Stocks
            .Where(s => !_db.Colors.Any(color => color.Id == s.ColorId))
            .Select(color => color.ColorId)
            .ToList();
        

        var nonExistingSizes = productDto.Stocks
            .SelectMany(s => s.Sizes)
            .Select(size => _db.Sizes.FirstOrDefault(s => s.Id == size.SizeId))
            .Where(size => size == null)
            .ToList();
        
        var existingMainMaterial = await _db.Materials.FirstOrDefaultAsync(material => material.Id == productDto.MainMaterialId);
        var existingOccasion = await _db.Occasions.FirstOrDefaultAsync(occasion => occasion.Id == productDto.OccasionId);
        var category = await _db.Categories.FirstOrDefaultAsync(c => c.Id == productDto.CategoryId);
        var existingCollection = await _db.Collections.Include(collection => collection.Store)
            .ThenInclude(store => store.Owner).FirstOrDefaultAsync(collection => collection.Id == productDto.CollectionId);

        if (existingCollection == null)
        {
            throw new ArgumentException($"Collection with ID {productDto.CollectionId} does not exist");
        }
        
        var enumerable = roles as string[] ?? roles.ToArray();
        
        // check if role is seller
        if (!enumerable.Contains(UserRoles.Seller) && !enumerable.Contains(UserRoles.Admin) && !enumerable.Contains(UserRoles.SuperAdmin))
        {
            throw new UnauthorizedAccessException("You are not a seller");
        }
        
        if (existingCollection.Store.Owner.UserName != username 
            && !enumerable.Contains(UserRoles.Admin) 
            && !enumerable.Contains(UserRoles.SuperAdmin))
        {
            throw new UnauthorizedAccessException("You are not the owner of this store");
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
        if (existingMainMaterial == null)
        {
            throw new ArgumentException($"Main material with ID {productDto.MainMaterialId} does not exist");
        }
        
        if (existingOccasion == null)
        {
            throw new ArgumentException($"Occasion with ID {productDto.OccasionId} does not exist");
        }

        if (productDto.CategoryId == Guid.Empty || category == null)
        {
            throw new ArgumentException($"Category with ID {productDto.CategoryId} does not exist");
        }
        
        var product = _mapper.Map<Models.Entities.Product>(productDto);
        
        
        product.Stocks = productDto.Stocks.Select(stockDto => new ProductStock
        {
            Product = product,
            Color = _db.Colors.FirstOrDefault(color => color.Id == stockDto.ColorId),
            Size = _db.Sizes.FirstOrDefault(s => s.Id == stockDto.Sizes.First().SizeId),
            Stock = stockDto.Sizes.First().Stock,
            Price = stockDto.Sizes.First().Price
        }).ToList();
        
        product.Materials = productDto.Materials.Select(material => new ProductMaterial
        {
            Product = product,
            Material = _db.Materials.FirstOrDefault(m => m.Id == material.Id),
            Percentage = material.Percentage
        }).ToList();
        
        product.Images = productDto.Stocks.Select(stockDto => new ProductImage
        {
            Product = product,
            Color = _db.Colors.FirstOrDefault(color => color.Id == stockDto.ColorId),
            ImageUrls = stockDto.ImageUrls
        }).ToList();
        
        int initialOrder = CalculateDepth(category);
        AddToCategories(category, product, initialOrder);

        await _db.Products.AddAsync(product);
        await SaveChangesAsyncWithTransaction();

        return product;
    }

    public async Task<Models.Entities.Product> UpdateProductAsync(Guid id, UpdateProductDto productDto, string username, IEnumerable<string> roles)
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


        var storeOwner = product.Collection.Store.Owner.UserName;
        
        if (storeOwner != username 
            && !roles.Contains(UserRoles.Admin) 
            && !roles.Contains(UserRoles.SuperAdmin))
        {
            throw new UnauthorizedAccessException("You are not the owner of this store");
        }

        var categories = await _db.Categories.ToListAsync();
        var materials = await _db.Materials.ToListAsync();
        var colors = await _db.Colors.ToListAsync();
        var sizes = await _db.Sizes.ToListAsync();
        var occasions = await _db.Occasions.ToListAsync();
        var collections = await _db.Collections.Include(collection => collection.Store)
                .ThenInclude(store => store.Owner).ToListAsync();

        if (!string.IsNullOrEmpty(productDto.Name))
        {
            product.Name = productDto.Name;
        }
           
        // checking if category id is not empty
        if (!string.IsNullOrEmpty(productDto.CategoryId))
        {
            // checking if category with given id exists
            var category = categories.FirstOrDefault(c => c.Id == Guid.Parse(productDto.CategoryId));
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
        if (productDto.Discount.HasValue)
        {
            product.Discount = productDto.Discount.Value;
        }
        if (!string.IsNullOrEmpty(productDto.SizeChartImageUrl))
        {
            product.SizeChartImageUrl = productDto.SizeChartImageUrl;
        }

        if (!string.IsNullOrEmpty(productDto.MainMaterialId))
        {
            var mainMaterial = materials.FirstOrDefault(m => m.Id == Guid.Parse(productDto.MainMaterialId));
            if (mainMaterial == null)
            {
                throw new ArgumentException($"Main material with ID {productDto.MainMaterialId} does not exist");
            }
            product.MainMaterial = mainMaterial;
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
        
        if (!string.IsNullOrEmpty(productDto.OccasionId))
        {
            var occasion = occasions.FirstOrDefault(o => o.Id == Guid.Parse(productDto.OccasionId));
            if (occasion == null)
            {
                throw new ArgumentException($"Occasion with ID {productDto.OccasionId} does not exist");
            }
            product.Occasion = occasion;
        }

        if (!string.IsNullOrEmpty(productDto.CollectionId))
        {
            var collection = collections.FirstOrDefault(c => c.Id == Guid.Parse(productDto.CollectionId));
            if (collection == null)
            {
                throw new ArgumentException($"Collection with ID {productDto.CollectionId} does not exist");
            }
            product.Collection = collection;
        }
        
        if (productDto.Materials is { Count: > 0 })
        {
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
            var nonExistingColors = productDto.Stocks
                .Where(s => colors.All(color => color.Id != s.ColorId))
                .Select(color => color.ColorId)
                .ToList();
            
            var nonExistingSizes = productDto.Stocks
                .SelectMany(s => s.Sizes)
                .Select(size => sizes.FirstOrDefault(s => s.Id == size.SizeId))
                .Where(size => size == null)
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
                Color = _db.Colors.FirstOrDefault(color => color.Id == stockDto.ColorId),
                Size = _db.Sizes.FirstOrDefault(s => s.Id == stockDto.Sizes.First().SizeId),
                Stock = stockDto.Sizes.First().Stock,
                Price = stockDto.Sizes.First().Price
            }).ToList();
        }


        await SaveChangesAsyncWithTransaction();
        
        return product;
    }
    
    public async Task DeleteProductAsync(Guid id)
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
        
        ClearProductCategories(product);
        ClearProductMaterials(product);
        ClearProductStocks(product);
        
        _db.Products.Remove(product);
        await SaveChangesAsyncWithTransaction();
    }
    




}