using AutoMapper;
using Data.Repositories.Customer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Controllers;

[ApiController]
[Route("api/customer/{customerId}")]
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
        try {
            var res = await _repository.GetReviewsForUser(GetUserId());

            return Ok(res);
        } 
        catch (UnauthorizedAccessException e) {
            return Unauthorized(e.Message);
        }
        catch (ArgumentException e)
        {
            return BadRequest(e.Message);
        }
        catch (Exception e) {
            return StatusCode(500, e.Message);

        }
    }

    [HttpGet("upgrade")]
    public async Task<IActionResult> UpgradeToSeller()
    {
        try {
            var res = await _repository.UpgradeToSeller(GetUserId());

            return Ok(res);
        } 
        catch (UnauthorizedAccessException e) {
            return Unauthorized(e.Message);
        }
        catch (ArgumentException e)
        {
            return BadRequest(e.Message);
        }
        catch (Exception e) {
            return StatusCode(500, e.Message);

        }
    }
}
