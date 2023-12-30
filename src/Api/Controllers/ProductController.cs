using AutoMapper;
using Ecommerce.Data;
using ECommerce.Models.DTOs;
using ECommerce.Models.Entities;
using Ecommerce.RequestHelpers;
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
        var products = await _dbContext.Products
            .AsSplitQuery()
            .Include(p => p.Categories).ThenInclude(productCategory => productCategory.Category)
            .Include(p => p.Occasion)
            .Include(p => p.MainMaterial)
            .Include(p => p.Stocks).ThenInclude(s => s.Color)
            .Include(p => p.Stocks).ThenInclude(s => s.Size)
            .Include(p => p.Images)
            .Include(p => p.Materials).ThenInclude(m => m.Material)
            .Include(product => product.Reviews)
            .Include(product => product.Orders)
            .ToListAsync();
        
        
        // TODO: ORDER BY 
        var query = searchParams.OrderBy switch
        {
            "price" => products.OrderBy(p => p.Stocks.Min(s => s.Price)),
            "price_desc" => products.OrderByDescending(p => p.Stocks.Min(s => s.Price)),
            "name" => products.OrderBy(p => p.Name),
            "name_desc" => products.OrderByDescending(p => p.Name),
            "newest" => products.OrderByDescending(p => p.CreatedAt),
            "oldest" => products.OrderBy(p => p.CreatedAt),
            "bestseller" => products.OrderBy(p => p.Orders.Count),
            _ => products.OrderBy(p => p.Name)
        };

        // TODO: FILTER BY
        var filteredQuery = searchParams.FilterBy switch
        {
            "in_stock" => query.Where(p => p.Stocks.Any(s => s.Stock > 0)),
            "discount" => query.Where(p => p.Discount > 0),
            "out_of_stock" => query.Where(p => p.Stocks.All(s => s.Stock == 0)),
            "new" => query.Where(p => p.CreatedAt > DateTime.Now.AddDays(-7)),
            _ => query,
        };

        
        if (!string.IsNullOrEmpty(searchParams.Category))
        {
            filteredQuery.Where(p => p.Categories.Any(pc => string.Equals(pc.Category.Name, searchParams.Category, StringComparison.CurrentCultureIgnoreCase)));
        }
        
        // TODO: PAGINATION
        if (searchParams.PageSize.HasValue && searchParams.PageNumber.HasValue)
        {
            filteredQuery = filteredQuery
                .Skip(searchParams.PageSize.Value * (searchParams.PageNumber.Value - 1))
                .Take(searchParams.PageSize.Value);
        }
        var productDto = _mapper.Map<List<ProductDto>>(filteredQuery);
        return Ok(productDto);
    }
    
    private IEnumerable<string> GetAllChildCategories(string parentCategory)
    {
        var childCategories = _dbContext.Categories
            .Where(c => c.ParentCategory.Name.ToLower() == parentCategory)
        .Select(c => c.Name.ToLower())
            .ToList();

        foreach (var childCategory in childCategories.ToList())
        {
            childCategories.AddRange(GetAllChildCategories(childCategory));
        }

        return childCategories;
    }

    [HttpGet("{id}")]
    public ActionResult GetProduct(Guid id)
    {
        return Ok();
    }

    [HttpPost]
    public IActionResult CreateProduct(Product product)
    {
        // TODO: Implement
        return Ok();
    }

    [HttpPut("{id}")]
    public IActionResult UpdateProduct(Guid id, Product product)
    {
        // TODO: Implement
        return Ok();
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteProduct(Guid id)
    {
        // TODO: Implement
        return Ok();
        
    }
}