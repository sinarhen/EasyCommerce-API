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
        var res = await _repository.GetReviewsForUser(GetUserId(), GetUserRoles());
        return Ok(res);
    }

    [HttpGet("cart")]
    public async Task<IActionResult> GetCart()
    {
        var res = await _repository.GetCartForUser(GetUserId());
        return Ok(res);
    }

    [HttpPost("cart")]
    [ServiceFilter(typeof(ValidationService))]
    public async Task<IActionResult> AddToCart([FromBody] CreateCartItemDto cartItem)
    {
        await _repository.AddProductToCart(GetUserId(), cartItem);
        return Ok("Successfully added to cart");
    }

    [HttpDelete("cart/{cartProductId:guid}")]
    public async Task<IActionResult> RemoveFromCart(Guid cartProductId)
    {
        await _repository.RemoveProductFromCart(GetUserId(), cartProductId);
        return Ok("Successfully removed from cart");
    }

    [HttpPost("upgrade")]
    [ServiceFilter(typeof(ValidationService))]
    public async Task<IActionResult> UpgradeToSeller([FromBody] SellerInfoCreateDto sellerInfo)
    {
        await _repository.RequestUpgradingToSeller(GetUserId(), sellerInfo);
        return Ok("Successfully requested to upgrade to seller");
    }
}