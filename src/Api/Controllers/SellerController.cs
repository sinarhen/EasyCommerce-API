using AutoMapper;
using ECommerce.Config;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Controllers;

[ApiController]
[Route("api/seller")]
[Authorize(Policy = Policies.SellerPolicy)]
public class SellerController: GenericController
{
    public SellerController(IMapper mapper) : base(mapper)
    {
        
    }

    [HttpGet]
    public async Task<IActionResult> GetSellerInfo()
    {
        try
        {
            var id = GetUserId();
            
            // TODO: Implement the repository method to get seller info
            return Ok();

        } 
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

}