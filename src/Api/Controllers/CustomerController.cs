using AutoMapper;
using ECommerce.Data.Repositories.Customer;
using ECommerce.Models.DTOs.User;
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
            var res = await _repository.GetReviewsForUser(GetUserId());

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

    [HttpPost("upgrade")]
    public async Task<IActionResult> UpgradeToSeller([FromBody] SellerInfoCreateDto sellerInfo)
    {
        try
        {
            var res = await _repository.RequestUpgradingToSeller(GetUserId(), sellerInfo);

            return Ok(res ? "Successfully requested to upgrade to seller" : "Failed to request to upgrade to seller");
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