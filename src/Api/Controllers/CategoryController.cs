using AutoMapper;
using ECommerce.Config;
using Ecommerce.Data;
using Ecommerce.Models.DTOs;
using ECommerce.Models.DTOs;
using Ecommerce.RequestHelpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Controllers;

[ApiController]
[Route("api/products")]
public class CategoryController : ControllerBase
{
    private readonly ProductDbContext _dbContext;
    private readonly IMapper _mapper;

    public CategoryController(ProductDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult> GetCategories([FromQuery] SearchParams searchParams)
    {
        return Ok();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> GetCategory(Guid id, Guid? sizeId = null, Guid? colorId = null)
    {
        return Ok();
    }    

    [Authorize(Roles = UserRoles.Admin + "," + UserRoles.SuperAdmin)]
    [HttpPost]
    public async Task<ActionResult> CreateCategory(CreateProductDto productDto)
    {
        return Ok();
    }
    
    [Authorize(Roles = UserRoles.Admin + "," + UserRoles.SuperAdmin)]
    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateCategory(Guid id, UpdateProductDto productDto)
    {
        return Ok();
    }

    [Authorize(Roles = UserRoles.Admin + "," + UserRoles.SuperAdmin)]
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteCategory(Guid id)
    {
        return Ok();        
    }
}