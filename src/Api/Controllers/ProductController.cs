using System.Security.Claims;
using AutoMapper;
using ECommerce.Config;
using ECommerce.Data.Repositories.Product;
using ECommerce.Models.DTOs;
using ECommerce.Models.Entities;
using ECommerce.RequestHelpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Controllers;

[ApiController]
[Route("api/products")]
public class ProductController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IProductRepository _repository;

    public ProductController(IMapper mapper, IProductRepository repository)
    {
        _mapper = mapper;
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
            var productDtos = _mapper.Map<List<ProductDto>>(products);
            return Ok(new
            {
                Products = productDtos,
                Total = productDtos.Count,
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

    private List<ColorDto> GetColorDtos(IEnumerable<ProductStock> stocks, Guid? sizeId = null)
    {
        var query = stocks.AsQueryable();

        if (sizeId.HasValue)
        {
            query = query.Where(ps => ps.SizeId == sizeId.Value);
        }

        return query.Select(ps => new ColorDto
            {
                Id = ps.Color.Id,
                Name = ps.Color.Name,
                HexCode = ps.Color.HexCode,
                ImageUrls = ps.Product.Images
                    .Where(i => i.ColorId == ps.ColorId)
                    .SelectMany(i => i.ImageUrls)
                    .ToList(),
                IsAvailable = ps.Stock > 0,
                Quantity = ps.Stock
            })
            .ToList();
    }

    private List<SizeDto> GetSizeDtos(IEnumerable<ProductStock> stocks, Guid? colorId = null)
    {
        var query = stocks.AsQueryable();

        if (colorId.HasValue)
        {
            query = query.Where(s => s.ColorId == colorId.Value);
        }

        return query
            .Select(s => new SizeDto {
                Id = s.SizeId, 
                Name = s.Size.Name, 
                Quantity = s.Stock })
            .OrderBy(s => s.Value)
            .ToList();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> GetProduct(Guid id, Guid? sizeId = null, Guid? colorId = null)
    {
        var product = await _repository.GetProductAsync(id);

        if (product == null)
        {
            return NotFound();
        }
        var productDto = _mapper.Map<ProductDetailsDto>(product);
        
        if (sizeId.HasValue && colorId.HasValue)
        {
            var stock = product.Stocks.FirstOrDefault(s => s.SizeId == sizeId.Value && s.ColorId == colorId.Value);
            if (stock != null)
            {
                productDto.Availability = new AvailabilityDto { Quantity = stock.Stock, Price = stock.Price };
            }
        }

        productDto.Colors = GetColorDtos(product.Stocks, (sizeId.HasValue && colorId.HasValue) ? sizeId : null);
        productDto.Sizes = GetSizeDtos(product.Stocks, (sizeId.HasValue && colorId.HasValue) ? colorId : null);
        return Ok(productDto);
    }    

    [Authorize(Policy = Policies.SellerPolicy)]
    [HttpPost]
    public async Task<ActionResult<ProductDto>> CreateProduct(CreateProductDto productDto)
    {
        var username = User.Claims.FirstOrDefault(c => c.Type == CustomClaimTypes.Username)?.Value;
        
        var roles = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value);

        try
        {
            var product = await _repository.CreateProductAsync(productDto, username, roles);
            
            return _mapper.Map<ProductDto>(product);
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
            var username = User.Claims.FirstOrDefault(c => c.Type == CustomClaimTypes.Username)?.Value;
            var roles = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value);
            var product = await _repository.UpdateProductAsync(id, productDto, username, roles);

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
        await _repository.DeleteProductAsync(id);
        
        
        return Ok();        
    }
}