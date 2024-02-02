using ECommerce.Models.DTOs.Billboard;
using ECommerce.Models.Entities;
using ECommerce.Data.Repositories.Billboard;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using ECommerce.Data.Repositories.BillboardFilter;
using System.Security.Claims;
using ECommerce.Config;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;

namespace ECommerce.Controllers
{
    [ApiController]
    [Route("api/billboards/{billboardId}/filter")]
    public class BillboardFilterController : GenericController
    {
        private readonly IBillboardFilterRepository _billboardRepository;

        public BillboardFilterController(IBillboardFilterRepository billboardRepository, IMapper mapper) : base(mapper)
        {
            _billboardRepository = billboardRepository;
        }

        [Authorize(Policy = Policies.SellerPolicy)]
        [HttpPost]
        public async Task<ActionResult<BillboardFilterDto>> CreateBillboardFilter(Guid billboardId, [FromBody]BillboardFilterDto createBillboardFilterDto)
        {

            var billboardFilter = await _billboardRepository.CreateBillboardFilterAsync(billboardId, GetUserId(), createBillboardFilterDto, IsAdmin());
            return CreatedAtAction(nameof(GetBillboardFilter), new { id = billboardFilter.Id }, _mapper.Map<BillboardFilterDto>(billboardFilter));
        }

        [Authorize(Policy = Policies.SellerPolicy)]
        [HttpGet("{id}")]
        public async Task<ActionResult<BillboardFilterDto>> GetBillboardFilter(Guid billbaordId, Guid id)
        {
            var billboardFilter = await _billboardRepository.GetBillboardFilterAsync(GetUserId(), id, IsAdmin());
            if (billboardFilter == null)
            {
                return NotFound();
            }
            return _mapper.Map<BillboardFilterDto>(billboardFilter);
        }

        [Authorize(Policy = Policies.SellerPolicy)]
        [HttpPut("{id}")]
        public async Task<ActionResult<BillboardFilterDto>> UpdateBillboardFilter(Guid billboardId, Guid id, [FromBody] BillboardFilterDto updateBillboardFilterDto)
        {
            var billboardFilter = await _billboardRepository.UpdateBillboardFilterAsync(GetUserId(), id, updateBillboardFilterDto, IsAdmin());
            if (billboardFilter == null)
            {
                return NotFound();
            }
            return _mapper.Map<BillboardFilterDto>(billboardFilter);
        }

        [Authorize(Policy = Policies.SellerPolicy)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBillboardFilter(Guid billboardId, Guid id)
        {
            await _billboardRepository.DeleteBillboardFilterAsync(GetUserId(), id, IsAdmin());
            return NoContent();
        }

    }
}