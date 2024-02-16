using AutoMapper;
using ECommerce.Data.Repositories.Customer;
using ECommerce.Models.DTOs.Cart;
using ECommerce.Models.DTOs.User;
using ECommerce.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Controllers;

[ApiController]
[Route("api/customer")]
[Authorize]
public class CustomerController : GenericController
{
    private readonly ICustomerRepository _repository;

    public CustomerController(IMapper mapper, ICustomerRepository repository) : base(mapper)
    {
        _repository = repository;
    }

    [HttpGet("reviews")]
    public async Task<IActionResult> GetReviewsForUser()
    {
        try
        {
            var res = await _repository.GetReviewsForUser(GetUserId(), GetUserRoles());

            return Ok(res);
        }
        catch (UnauthorizedAccessException e)
        {
            return Unauthorized(e.Message);
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

    [HttpGet("cart")]
    public async Task<IActionResult> GetCart()
    {
        try
        {
            var res = await _repository.GetCartForUser(GetUserId());

            return Ok(res);
        }
        catch (UnauthorizedAccessException e)
        {
            return Unauthorized(e.Message);
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
    
    [HttpPost("cart")]
    [ServiceFilter(typeof(ValidationService))]
    public async Task<IActionResult> AddToCart([FromBody] CreateCartItemDto cartItem)
    {
        try
        {
            await _repository.AddProductToCart(GetUserId(), cartItem);

            return Ok("Successfully added to cart");
        }
        catch (UnauthorizedAccessException e)
        {
            return Unauthorized(e.Message);
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

    [HttpPost("upgrade")]
    [ServiceFilter(typeof(ValidationService))]
    public async Task<IActionResult> UpgradeToSeller([FromBody] SellerInfoCreateDto sellerInfo)
    {
        try
        {
            await _repository.RequestUpgradingToSeller(GetUserId(), sellerInfo);

            return Ok("Successfully requested to upgrade to seller");
        }
        catch (UnauthorizedAccessException e)
        {
            return Unauthorized(e.Message);
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
}