using System.Security.Claims;
using AutoMapper;
using ECommerce.Config;
using ECommerce.Data.Repositories.Product;
using ECommerce.Models.DTOs;
using ECommerce.Models.DTOs.Color;
using ECommerce.Models.DTOs.Product;
using ECommerce.Models.DTOs.Size;
using ECommerce.Models.Entities;
using ECommerce.RequestHelpers;
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
        try
        {
            var products = await _repository.GetProductsAsync(searchParams);
        
            if (products == null)
            {
                return NotFound();
            }
            return Ok(new
            {
                Products = products,
                Total = products.Count(),
                PageNumber = searchParams.PageNumber,
                PageSize = searchParams.PageSize
            });

        }
        catch (ArgumentException e)
        {
            return BadRequest(e.Message);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
        
    }


    [HttpGet("{id}")]
    public async Task<ActionResult<ProductDto>> GetProduct(Guid id, Guid? sizeId = null, Guid? colorId = null)
    {
        var product = await _repository.GetProductAsync(id);

        return Ok(product);
    }    

    [Authorize(Policy = Policies.SellerPolicy)]
    [HttpPost]
    public async Task<ActionResult<ProductDto>> CreateProduct(CreateProductDto productDto)
    {

        try
        {
            var product = await _repository.CreateProductAsync(productDto, GetUserId(), IsAdmin());
            
            return StatusCode(201);
        }
        catch (ArgumentException e)
        {
            return BadRequest(e.Message);
        }
        catch (UnauthorizedAccessException  e)
        {
            return Unauthorized(e.Message);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
        
        
    }
    
    [Authorize(Policy = Policies.SellerPolicy)]
    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateProduct(Guid id, UpdateProductDto productDto)
    {
        try
        {
            var product = await _repository.UpdateProductAsync(id, productDto, GetUserId(), IsAdmin());

            if (product == null)
            {
                return NotFound();
            }
            
            return Ok();
        } catch (ArgumentException e)
        {
            return BadRequest(e.Message);
        }
        catch (UnauthorizedAccessException  e)
        {
            return Unauthorized(e.Message);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    [Authorize(Policy = Policies.SellerPolicy)]
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteProduct(Guid id)
    {
        await _repository.DeleteProductAsync(id, GetUserId(), IsAdmin());
        
        
        return Ok();        
    }
}