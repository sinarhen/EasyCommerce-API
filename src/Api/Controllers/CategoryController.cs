using System.Security.Claims;
using AutoMapper;
using ECommerce.Config;
using ECommerce.Data.Repositories.Category;
using ECommerce.Models.DTOs.Category;
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
        try
        {
            var categories = await _repository.GetCategoriesAsync();
            return Ok(
                new
                {
                    Categories = _mapper.Map<List<CategoryDto>>(categories)
                }
            );
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> GetCategory(Guid id)
    {
        try
        {
            if (id == Guid.Empty) return BadRequest();
            var category = await _repository.GetCategoryAsync(id);
            if (category == null) return NotFound();
            return Ok(_mapper.Map<CategoryDto>(category));
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [Authorize(Policy = Policies.AdminPolicy)]
    [HttpPost]
    public async Task<ActionResult> CreateCategory(WriteCategoryDto categoryDto)
    {
        try
        {
            await _repository.CreateCategoryAsync(categoryDto);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }

        return Ok();
    }

    [Authorize(Policy = Policies.AdminPolicy)]
    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateCategory(Guid id, WriteCategoryDto categoryDto)
    {
        Console.WriteLine("UpdateCategory User: " + User.Identity?.Name + "\n\n roles: " +
                          User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value));
        try
        {
            await _repository.UpdateCategoryAsync(id, categoryDto);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }

        return Ok();
    }

    [Authorize(Policy = Policies.AdminPolicy)]
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteCategory(Guid id)
    {
        try
        {
            await _repository.DeleteCategoryAsync(id);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }

        return Ok();
    }
}