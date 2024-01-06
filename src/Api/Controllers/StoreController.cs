using AutoMapper;
using ECommerce.Config;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Controllers;

[ApiController]
[Route("api/stores")]
public class StoreController : ControllerBase
{
    private readonly IMapper _mapper;

    public StoreController(IMapper mapper)
    {
        _mapper = mapper;
    }
    
    [Authorize(Roles = UserRoles.Admin + "," + UserRoles.SuperAdmin)]
    [HttpGet("{id}")]
    public async Task<ActionResult> GetStore()
    {
        return Ok();
    }

    [Authorize(Roles = UserRoles.Admin + "," + UserRoles.SuperAdmin)]
    [HttpGet]
    public async Task<ActionResult> GetStores()
    {
        return Ok();
    }
    
    [Authorize(Roles = UserRoles.Admin + "," + UserRoles.SuperAdmin + "," + UserRoles.Seller)]
    [HttpPost]
    public async Task<ActionResult> CreateStore()
    {
        return Ok();
    }
    
    
    [Authorize(Roles = UserRoles.Admin + "," + UserRoles.SuperAdmin)]
    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateStore()
    {
        return Ok();
    }
    
    
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteStore()
    {
        return Ok();
    }
    
    
    
    
}