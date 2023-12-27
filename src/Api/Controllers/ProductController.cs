using AutoMapper;
using AutoMapper.QueryableExtensions;
using ECommerce.Config;
using Ecommerce.Data;
using ECommerce.Models.DTOs;
using ECommerce.Models.Entities;
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
    public async Task<ActionResult<List<ProductDto>>> GetProducts()
    {
        var products = await _dbContext.Products
            .ProjectTo<ProductDto>(_mapper.ConfigurationProvider)
            .ToListAsync();
        return Ok(products);
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