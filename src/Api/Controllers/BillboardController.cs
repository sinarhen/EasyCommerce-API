using AutoMapper;
using ECommerce.Config;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Controllers;

[ApiController]
[Route("api/billboards")]
public class BillboardController : GenericController
{

    public BillboardController(IMapper mapper) : base(mapper)
    {
        
    }


    [HttpGet("{collectionId}")]
    public async Task<IActionResult> GetBillboardsForCollection(Guid collectionId)
    {
        await Task.Delay(0);
        return Ok();
    }

    [HttpGet("{billboardId}")]
    public async Task<IActionResult> GetBillboard(Guid billboardId)
    {
        await Task.Delay(0);
        return Ok();
    }

    [HttpPost("{collectionId}")]
    [Authorize(Policy = Policies.SellerPolicy)]
    public async Task<IActionResult> CreateBillboardForCollection(Guid collectionId)
    {
        await Task.Delay(0);
        return Ok();
    }

    [HttpPut("{billboardId}")]
    [Authorize(Policy = Policies.SellerPolicy)]
    public async Task<IActionResult> UpdateBillboard(Guid billboardId)
    {
        await Task.Delay(0);
        return Ok();
    }

    [HttpDelete("{billboardId}")]
    [Authorize(Policy = Policies.SellerPolicy)]
    public async Task<IActionResult> DeleteBillboard(Guid billboardId)
    {

        await Task.Delay(0);
        return Ok();
    }

}