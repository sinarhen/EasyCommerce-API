using AutoMapper;
using ECommerce.Config;
using Ecommerce.Data;
using Ecommerce.Data.Repositories.Category;
using Ecommerce.Models.DTOs;
using ECommerce.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Controllers;

[ApiController]
[Route("api/categories")]
public class CategoryController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly ICategoryRepository _repository;

    public CategoryController(IMapper mapper, ICategoryRepository repository)
    {
        _mapper = mapper;
        _repository = repository;
    }

    [HttpGet]
    public async Task<ActionResult<List<CategoryDto>>> GetCategories()
    {
        var categories = await _repository.GetCategoriesAsync();
        var categoriesDtos = _mapper.Map<List<CategoryDto>>(categories);
        
        return Ok(categoriesDtos);
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