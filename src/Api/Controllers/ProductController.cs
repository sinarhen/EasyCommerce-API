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
            .Include(p => p.Category)
            .Include(p => p.Occasion)
            .Include(p => p.MainMaterial)
            .Include(p => p.Stocks)
            .ThenInclude(s => s.Color)
            .Include(p => p.Stocks)
            .ThenInclude(s => s.Size)
            .Include(p => p.Images)
            .Include(p => p.Materials)
            .ThenInclude(m => m.Material)
            .ToListAsync();
        
        var productDto = _mapper.Map<List<ProductDto>>(products);
        
        return Ok(productDto);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> GetProduct(Guid id)
    {
        return Ok();
    }

    [HttpPost]
    public async Task<IActionResult> CreateProduct(Product product)
    {
        // TODO: Implement
        return Ok();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProduct(Guid id, Product product)
    {
        // TODO: Implement
        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduct(Guid id)
    {
        // TODO: Implement
        return Ok();
        
    }
}