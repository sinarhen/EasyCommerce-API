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
    var query = _dbContext.Products.AsNoTracking().AsSplitQuery();

    // TODO: FILTER BY
    query = searchParams.FilterBy switch
    {
        "in_stock" => query.Where(p => p.Stocks.Any(s => s.Stock > 0)),
        "discount" => query.Where(p => p.Discount > 0),
        "out_of_stock" => query.Where(p => p.Stocks.All(s => s.Stock == 0)),
        "new" => query.Where(p => p.CreatedAt > DateTime.Now.AddDays(-7)),
        _ => query,
    };

    if (!string.IsNullOrEmpty(searchParams.Category))
    {
        var categories = searchParams.Category.Split(",").Select(c => c.Trim().ToLower()).ToList();
        query = query.Where(p => p.Categories.Any(pc => categories.Contains(pc.Category.Name.ToLower())));
    }
    // TODO: ORDER BY 
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

    // TODO: PAGINATION
    if (searchParams.PageSize.HasValue && searchParams.PageNumber.HasValue)
    {
        query = query
            .Skip(searchParams.PageSize.Value * (searchParams.PageNumber.Value - 1))
            .Take(searchParams.PageSize.Value);
    }

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
        .ToListAsync();

    var productDto = _mapper.Map<List<ProductDto>>(products);
    return Ok(productDto);
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