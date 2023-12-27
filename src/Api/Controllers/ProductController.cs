using ECommerce.Models.DTOs;
using ECommerce.Models.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Controllers;

[ApiController]
[Route("api/products")]
public class ProductController : ControllerBase
{

    [HttpGet]
    public async Task<ActionResult<List<ProductDto>>> GetProducts()
    {
        return Ok();
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