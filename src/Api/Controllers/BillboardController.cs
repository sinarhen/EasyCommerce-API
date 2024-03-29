using AutoMapper;
using ECommerce.Config;
using ECommerce.Data.Repositories.Billboard;
using ECommerce.Models.DTOs.Billboard;
using ECommerce.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Controllers;

[ApiController]
[Route("api/collections/{collectionId}/billboards")]
public class BillboardController : GenericController
{
    private readonly IBillboardRepository _repository;

    public BillboardController(IMapper mapper, IBillboardRepository repository) : base(mapper)
    {
        _repository = repository;
    }


    [HttpGet]
    public async Task<IActionResult> GetBillboardsForCollection(Guid collectionId)
    {
        var billboards = await _repository.GetBillboardsForCollectionAsync(collectionId);
        return Ok(_mapper.Map<List<BillboardDto>>(billboards));
    }

    [HttpPost]
    [Authorize(Policy = Policies.SellerPolicy)]
    [ServiceFilter(typeof(ValidationService))]
    public async Task<IActionResult> CreateBillboardForCollection(Guid collectionId,
        [FromBody] CreateBillboardDto createBillboardDto)
    {
        var billboard =
            await _repository.CreateBillboardForCollectionAsync(collectionId, GetUserId(), createBillboardDto,
                IsAdmin());
        return Ok(_mapper.Map<BillboardDto>(billboard));
    }

    [HttpPatch("{billboardId:guid}")]
    [Authorize(Policy = Policies.SellerPolicy)]
    [ServiceFilter(typeof(ValidationService))]
    public async Task<IActionResult> UpdateBillboard(Guid billboardId, [FromBody] UpdateBillboardDto updateBillboardDto)
    {
        var billboard =
            await _repository.UpdateBillboardAsync(billboardId, updateBillboardDto, GetUserId(), IsAdmin());
        return Ok(_mapper.Map<BillboardDto>(billboard));
    }

    [HttpDelete("{billboardId}")]
    [Authorize(Policy = Policies.SellerPolicy)]
    public async Task<IActionResult> DeleteBillboard(Guid collectionId, Guid billboardId)
    {
        await _repository.DeleteBillboardAsync(collectionId, billboardId, GetUserId(), IsAdmin());
        return Ok();
    }
}