using ECommerce.Config;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Controllers;

[ApiController]
[Route("api/billboards")]
public class BillboardController : ControllerBase
{

    public BillboardController()
    {
        
    }


    [HttpGet("{storeId}")]
    public async Task<IActionResult> GetBillboardsForStore(Guid storeId)
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

    [HttpPost("{storeId}")]
    [Authorize(Policy = Policies.SellerPolicy)]
    public async Task<IActionResult> CreateBillboardForStore(Guid storeId)
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