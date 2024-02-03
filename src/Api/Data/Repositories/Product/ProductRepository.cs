using AutoMapper;
using ECommerce.Models.DTOs.Product;
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

        if (productDto.MainMaterialId != Guid.Empty)
        {
            var mainMaterial = await _db.Materials.FirstOrDefaultAsync(m => m.Id == productDto.MainMaterialId);
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