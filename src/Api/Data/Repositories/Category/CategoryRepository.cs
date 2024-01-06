using Ecommerce.RequestHelpers;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Data.Repositories.Category;

public class CategoryRepository : ICategoryRepository
{
    public Task<ActionResult> GetCategoriesAsync(SearchParams searchParams)
    {
        throw new NotImplementedException();
    }

    public Task<ActionResult> GetCategoryAsync(Guid id, Guid? sizeId = null, Guid? colorId = null)
    {
        throw new NotImplementedException();
    }

    public Task<ActionResult> CreateCategoryAsync(WriteCategoryDto categoryDto)
    {
        throw new NotImplementedException();
    }

    public Task<ActionResult> UpdateCategoryAsync(Guid id, WriteCategoryDto productDto)
    {
        throw new NotImplementedException();
    }

    public Task<ActionResult> DeleteCategoryAsync(Guid id)
    {
        throw new NotImplementedException();
    }
}