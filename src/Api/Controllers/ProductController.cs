using AutoMapper;
using ECommerce.Config;
using ECommerce.Data.Repositories.Product;
using ECommerce.Models.DTOs.Category;
using ECommerce.Models.DTOs.Product;
using ECommerce.Models.Entities;
using ECommerce.RequestHelpers.SearchParams;
using ECommerce.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Controllers;

[ApiController]
[Route("api/products")]
public class ProductController : GenericController
{
    private readonly IProductRepository _repository;

    public ProductController(IMapper mapper, IProductRepository repository) : base(mapper)
    {
        _repository = repository;
    }

    [HttpGet]
    public async Task<ActionResult<List<ProductDto>>> GetProducts([FromQuery] ProductSearchParams searchParams)
    {
        var (products, filters) = await _repository.GetProductsAsync(searchParams);
        
        if (products == null) return NotFound();
        var productDtos = products as ProductDto[] ?? products.ToArray();
        return Ok(new
        {
            Products = productDtos,
            Filters = new
            {
                Categories = _mapper.Map<List<CategoryDto>>(filters.Categories),
                Occasions = filters.Occasions,
                Materials = filters.Materials,
                Sizes = filters.Sizes,
                Colors = filters.Colors
            },
            Total = productDtos.Length,
            searchParams.PageNumber,
            searchParams.PageSize
        });
    }


    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ProductDto>> GetProduct(Guid id, Guid? sizeId = null, Guid? colorId = null)
    {
        var product = await _repository.GetProductAsync(id);
        if (product == null) return NotFound();
        return Ok(product);
    }

    [Authorize(Policy = Policies.SellerPolicy)]
    [HttpPost]
    [ServiceFilter(typeof(ValidationService))]
    public async Task<ActionResult<ProductDto>> CreateProduct(CreateProductDto productDto)
    {
        var product = await _repository.CreateProductAsync(productDto, GetUserId(), IsAdmin());

        return StatusCode(201, new
        {
            product.Id
        });
    }

    [Authorize(Policy = Policies.SellerPolicy)]
    [HttpPatch("{id:guid}")]
    [ServiceFilter(typeof(ValidationService))]
    public async Task<ActionResult> UpdateProduct(Guid id, UpdateProductDto productDto)
    {
        var product = await _repository.UpdateProductAsync(id, productDto, GetUserId(), IsAdmin());

        if (product == null) return NotFound();

        return Ok();
    }

    [Authorize(Policy = Policies.SellerPolicy)]
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> DeleteProduct(Guid id)
    {
        await _repository.DeleteProductAsync(id, GetUserId(), IsAdmin());


        return Ok();
    }
}