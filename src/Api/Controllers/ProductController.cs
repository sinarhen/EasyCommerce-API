using System.Security.Claims;
using AutoMapper;
using ECommerce.Config;
using Ecommerce.Data;
using Ecommerce.Models.DTOs;
using ECommerce.Models.DTOs;
using ECommerce.Models.Entities;
using Ecommerce.RequestHelpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Controllers;

[ApiController]
[Route("api/products")]
public class ProductController : ControllerBase
{
    private readonly ProductDbContext _dbContext;
    private readonly IMapper _mapper;

    public ProductController(ProductDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<List<ProductDto>>> GetProducts([FromQuery] SearchParams searchParams)
{
    var query = _dbContext.Products.AsNoTracking().AsSplitQuery();

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
            p.Categories.Any(pc => pc.Category.Sizes.Any(s => sizes.Contains(s.Size.Name.ToLower())) && categories.Contains(pc.Category.Name.ToLower()))
        );
    }
    else
    {
        if (categories != null)
        {
            query = query.Where(p => p.Categories.Any(pc => categories.Contains(pc.Category.Name.ToLower())));
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

    var productDto = _mapper.Map<List<ProductDto>>(products);
    return Ok(new
    {
        Products = productDto,
        Total = await query.CountAsync(),
        PageNumber = searchParams.PageNumber,
        PageSize = searchParams.PageSize
    });
}
        
    private List<ColorDto> GetColorDtos(IEnumerable<ProductStock> stocks, Guid? sizeId = null)
    {
        var query = stocks.AsQueryable();

        if (sizeId.HasValue)
        {
            query = query.Where(ps => ps.SizeId == sizeId.Value);
        }

        return query.Select(ps => new ColorDto
        {
            Id = ps.Color.Id,
            Name = ps.Color.Name,
            HexCode = ps.Color.HexCode,
            ImageUrls = ps.Product.Images
                .Where(i => i.ColorId == ps.ColorId)
                .SelectMany(i => i.ImageUrls)
                .ToList(),
            IsAvailable = ps.Stock > 0,
            Quantity = ps.Stock
        })
        .ToList();
    }

    private List<SizeDto> GetSizeDtos(IEnumerable<ProductStock> stocks, Guid? colorId = null)
    {
        var query = stocks.AsQueryable();

        if (colorId.HasValue)
        {
            query = query.Where(s => s.ColorId == colorId.Value);
        }

        return query
            .Select(s => new SizeDto {
                Id = s.SizeId, 
                Name = s.Size.Name, 
                Quantity = s.Stock })
            .OrderBy(s => s.Value)
            .ToList();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> GetProduct(Guid id, Guid? sizeId = null, Guid? colorId = null)
    {
        var product = await _dbContext.Products.AsNoTracking().AsSplitQuery()
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

        if (product == null)
        {
            return NotFound();
        }

        var productDto = _mapper.Map<ProductDetailsDto>(product);

        if (sizeId.HasValue && colorId.HasValue)
        {
            var stock = product.Stocks.FirstOrDefault(s => s.SizeId == sizeId.Value && s.ColorId == colorId.Value);
            if (stock != null)
            {
                productDto.Availability = new AvailabilityDto { Quantity = stock.Stock, Price = stock.Price };
            }
        }

        productDto.Colors = GetColorDtos(product.Stocks, (sizeId.HasValue && colorId.HasValue) ? sizeId : null);
        productDto.Sizes = GetSizeDtos(product.Stocks, (sizeId.HasValue && colorId.HasValue) ? colorId : null);
        return Ok(productDto);
    }    
    private void ValidateOnModelLevel(CreateProductDto productDto)
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
    
    [Authorize(Roles = UserRoles.Admin + "," + UserRoles.SuperAdmin)]
    [HttpPost]
    public async Task<IActionResult> CreateProduct(CreateProductDto productDto)
    {
        try 
        {
            ValidateOnModelLevel(productDto);
        }
        catch (ArgumentException e)
        {
            return BadRequest(e.Message);
        }
        
        var nonExistingMaterialIds = productDto.Materials
            .Where(m => !_dbContext.Materials.Any(material => material.Id == m.Id))
            .Select(m => m.Id)
            .ToList();
        
        var nonExistingColors = productDto.Stocks
            .Where(s => !_dbContext.Colors.Any(color => color.Id == s.ColorId))
            .Select(color => color.ColorId)
            .ToList();
        

        var nonExistingSizes = productDto.Stocks
            .SelectMany(s => s.Sizes)
            .Select(size => _dbContext.Sizes.FirstOrDefault(s => s.Id == size.SizeId))
            .Where(size => size == null)
            .ToList();
        
        var existingMainMaterial = _dbContext.Materials.FirstOrDefault(material => material.Id == productDto.MainMaterialId);
        var existingOccasion = _dbContext.Occasions.FirstOrDefault(occasion => occasion.Id == productDto.OccasionId);
        var category = _dbContext.Categories.FirstOrDefault(c => c.Id == productDto.CategoryId);
        var existingCollection = _dbContext.Collections.Include(collection => collection.Store)
            .ThenInclude(store => store.Owner).FirstOrDefault(collection => collection.Id == productDto.CollectionId);

        if (existingCollection == null)
        {
            return BadRequest($"Collection with ID {productDto.CollectionId} does not exist");
        }
        
        if (existingCollection.Store.Owner.UserName != User.Claims.FirstOrDefault(c => c.Type == CustomClaimTypes.Username)?.Value)
        {
            Console.WriteLine("Store owner: " + existingCollection.Store.Owner.UserName);
            Console.WriteLine("User: " + User.Claims.FirstOrDefault(c => c.Type == CustomClaimTypes.Username)?.Value);
            return Unauthorized("You are not the owner of this store");
        }

        if (nonExistingMaterialIds.Any())
        {
            return BadRequest($"Materials with the following IDs do not exist: {string.Join(", ", nonExistingMaterialIds)}");
        }
        if (nonExistingColors.Any())
        {
            return BadRequest($"Colors with the following IDs do not exist: {string.Join(", ", nonExistingColors)}");
        }
        if (nonExistingSizes.Any())
        {
            return BadRequest($"Sizes with the following IDs do not exist: {string.Join(", ", nonExistingSizes)}");
        }
        if (existingMainMaterial == null)
        {
            return BadRequest($"Main material with ID {productDto.MainMaterialId} does not exist");
        }
        
        if (existingOccasion == null)
        {
            return BadRequest($"Occasion with ID {productDto.OccasionId} does not exist");
        }

        if (productDto.CategoryId == Guid.Empty || category == null)
        {
            return BadRequest($"Category with ID {productDto.CategoryId} does not exist");
        }
        
        
        
        var product = _mapper.Map<Product>(productDto);
        
        product.Stocks = productDto.Stocks.Select(stockDto => new ProductStock
        {
            Product = product,
            Color = _dbContext.Colors.FirstOrDefault(color => color.Id == stockDto.ColorId),
            Size = _dbContext.Sizes.FirstOrDefault(s => s.Id == stockDto.Sizes.First().SizeId),
            Stock = stockDto.Sizes.First().Stock,
            Price = stockDto.Sizes.First().Price
        }).ToList();
        
        product.Materials = productDto.Materials.Select(material => new ProductMaterial
        {
            Product = product,
            Material = _dbContext.Materials.FirstOrDefault(m => m.Id == material.Id),
            Percentage = material.Percentage
        }).ToList();
        
        product.Images = productDto.Stocks.Select(stockDto => new ProductImage
        {
            Product = product,
            Color = _dbContext.Colors.FirstOrDefault(color => color.Id == stockDto.ColorId),
            ImageUrls = stockDto.ImageUrls
        }).ToList();
        
        AddToCategories(category, product);
        
        await _dbContext.Products.AddAsync(product);
        await _dbContext.SaveChangesAsync();
        
        return Ok();
    }

    private void AddToCategories(Category category, Product product)
    {
        if (category.ParentCategory != null)
        {
            AddToCategories(category.ParentCategory, product);
        }
        else
        {
            product.Categories.Add(new ProductCategory
            {
                ProductId = product.Id,
                CategoryId = category.Id
            });
        }
    }
    
    private void ClearProductCategories(Product product)
    {
        foreach (var productCategory in product.Categories)
        {
            _dbContext.ProductCategories.Remove(productCategory);
        }
    }
    
    private void ClearProductMaterials(Product product)
    {
        foreach (var productMaterial in product.Materials)
        {
            _dbContext.ProductMaterials.Remove(productMaterial);
        }
    }

    private void ClearProductStocks(Product product)
    {
        foreach (var productStock in product.Stocks)
        {
            _dbContext.ProductStocks.Remove(productStock);
        }
    }
    
    
    
    [Authorize(Roles = UserRoles.Admin + "," + UserRoles.SuperAdmin + "," + UserRoles.Seller)]
    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateProduct(Guid id, UpdateProductDto productDto)
    {
        var product = await _dbContext.Products.Include(product => product.Stocks)
            .Include(product => product.Categories).Include(product => product.Materials).FirstOrDefaultAsync(p => p.Id == id);
        if (product == null)
        {
            return NotFound();
        }
        
        if (!string.IsNullOrEmpty(productDto.Name))
        {
            product.Name = productDto.Name;
        }
           
        // checking if category id is not empty
        if (!string.IsNullOrEmpty(productDto.CategoryId))
        {
            // checking if category with given id exists
            var category = await _dbContext.Categories.FirstOrDefaultAsync(c => c.Id == Guid.Parse(productDto.CategoryId));
            if (category == null)
            {
                return BadRequest($"Category with ID {productDto.CategoryId} does not exist");
            }
            
            // clearing all categories from product
            ClearProductCategories(product);
            AddToCategories(category, product);
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
            var mainMaterial = await _dbContext.Materials.FirstOrDefaultAsync(m => m.Id == Guid.Parse(productDto.MainMaterialId));
            if (mainMaterial == null)
            {
                return BadRequest($"Main material with ID {productDto.MainMaterialId} does not exist");
            }
            product.MainMaterial = mainMaterial;
        }

        if (!string.IsNullOrEmpty(productDto.Gender))
        {
            if (!Enum.TryParse<Gender>(productDto.Gender, out var gender))
            {
                return BadRequest($"Invalid gender value. Valid gender options are: ({string.Join(", ", Enum.GetNames<Gender>())})");
            }

            product.Gender = gender;
        }

        if (!string.IsNullOrEmpty(productDto.Season))
        {
            if (!Enum.TryParse<Season>(productDto.Season, out var season))
            {
                return BadRequest($"Invalid season value. Valid seasons are: ({string.Join(", ", Enum.GetNames<Season>())})");
            }
            product.Season = season;
        }
        
        if (!string.IsNullOrEmpty(productDto.OccasionId))
        {
            var occasion = await _dbContext.Occasions.FirstOrDefaultAsync(o => o.Id == Guid.Parse(productDto.OccasionId));
            if (occasion == null)
            {
                return BadRequest($"Occasion with ID {productDto.OccasionId} does not exist");
            }
            product.Occasion = occasion;
        }
        
        if (!string.IsNullOrEmpty(productDto.CollectionId))
        {
            var collection = await _dbContext.Collections.Include(collection => collection.Store)
                .ThenInclude(store => store.Owner).FirstOrDefaultAsync(collection => collection.Id == Guid.Parse(productDto.CollectionId));
            if (collection == null)
            {
                return BadRequest($"Collection with ID {productDto.CollectionId} does not exist");
            }
            if (collection.Store.Owner.UserName != User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value)
            {
                return Unauthorized("You are not the owner of this store");
            }
            product.Collection = collection;
        }
        
        if (productDto.Materials is { Count: > 0 })
        {
            var nonExistingMaterialIds = productDto.Materials
                .Where(m => !_dbContext.Materials.Any(material => material.Id == m.Id))
                .Select(m => m.Id)
                .ToList();
            
            if (nonExistingMaterialIds.Any())
            {
                return BadRequest($"Materials with the following IDs do not exist: {string.Join(", ", nonExistingMaterialIds)}");
            }
            
            ClearProductMaterials(product);
            
            product.Materials = productDto.Materials.Select(material => new ProductMaterial
            {
                Product = product,
                Material = _dbContext.Materials.FirstOrDefault(m => m.Id == material.Id),
                Percentage = material.Percentage
            }).ToList();
        }
        
        if (productDto.Stocks is { Count: > 0 })
        {
            var nonExistingColors = productDto.Stocks
                .Where(s => !_dbContext.Colors.Any(color => color.Id == s.ColorId))
                .Select(color => color.ColorId)
                .ToList();
            
            var nonExistingSizes = productDto.Stocks
                .SelectMany(s => s.Sizes)
                .Select(size => _dbContext.Sizes.FirstOrDefault(s => s.Id == size.SizeId))
                .Where(size => size == null)
                .ToList();
            
            if (nonExistingColors.Any())
            {
                return BadRequest($"Colors with the following IDs do not exist: {string.Join(", ", nonExistingColors)}");
            }
            if (nonExistingSizes.Any())
            {
                return BadRequest($"Sizes with the following IDs do not exist: {string.Join(", ", nonExistingSizes)}");
            }
            
            ClearProductStocks(product);
            product.Stocks = productDto.Stocks.Select(stockDto => new ProductStock
            {
                Product = product,
                Color = _dbContext.Colors.FirstOrDefault(color => color.Id == stockDto.ColorId),
                Size = _dbContext.Sizes.FirstOrDefault(s => s.Id == stockDto.Sizes.First().SizeId),
                Stock = stockDto.Sizes.First().Stock,
                Price = stockDto.Sizes.First().Price
            }).ToList();
        }
        
        await _dbContext.SaveChangesAsync();
        return Ok();
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteProduct(Guid id)
    {
        // TODO: Implement
        return Ok();
        
    }
}