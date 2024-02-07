using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Controllers;

[ApiController]
[Route("api/customer/{customerId}")]
public class CustomerController : GenericController
{
    public CustomerController(IMapper mapper) : base(mapper)
    {
    }

    [HttpGet("reviews")]
    public async Task<IActionResult> GetReviewsForUser()
    {
        try {
            
            
            string res = null;
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