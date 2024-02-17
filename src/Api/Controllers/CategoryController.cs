using AutoMapper;
using ECommerce.Config;
using ECommerce.Data.Repositories.Category;
using ECommerce.Models.DTOs.Category;
using ECommerce.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Controllers;

[ApiController]
[Route("api/categories")]
public class CategoryController : GenericController
{
    private readonly ICategoryRepository _repository;

    public CategoryController(IMapper mapper, ICategoryRepository repository) : base(mapper)
    {
        _repository = repository;
    }

    [HttpGet]
    public async Task<ActionResult<List<CategoryDto>>> GetCategories()
    {
        var categories = await _repository.GetCategoriesAsync();
        return Ok(
            new
            {
                Categories = _mapper.Map<List<CategoryDto>>(categories)
            }
        );
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult> GetCategory(Guid id)
    {
        var category = await _repository.GetCategoryAsync(id);
        if (category == null) return NotFound();
        return Ok(_mapper.Map<CategoryDto>(category));
    }

    [Authorize(Policy = Policies.AdminPolicy)]
    [HttpPost]
    [ServiceFilter(typeof(ValidationService))]
    public async Task<ActionResult> CreateCategory(WriteCategoryDto categoryDto)
    {
        await _repository.CreateCategoryAsync(categoryDto);

        return Ok();
    }

    [Authorize(Policy = Policies.AdminPolicy)]
    [HttpPut("{id:guid}")]
    public async Task<ActionResult> UpdateCategory(Guid id, WriteCategoryDto categoryDto)
    {
        await _repository.UpdateCategoryAsync(id, categoryDto);
        return Ok();
    }

    [Authorize(Policy = Policies.AdminPolicy)]
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteCategory(Guid id)
    {
        await _repository.DeleteCategoryAsync(id);
        return Ok();
    }
}