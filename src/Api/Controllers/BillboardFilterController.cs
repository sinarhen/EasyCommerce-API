using ECommerce.Models.DTOs.Billboard;
using ECommerce.Models.Entities;
using ECommerce.Data.Repositories.Billboard;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using ECommerce.Data.Repositories.BillboardFilter;

namespace ECommerce.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BillboardFilterController : ControllerBase
    {
        private readonly IBillboardFilterRepository _billboardRepository;

        public BillboardFilterController(IBillboardFilterRepository billboardRepository)
        {
            _billboardRepository = billboardRepository;
        }

        [HttpPost]
        public async Task<ActionResult<BillboardFilter>> CreateBillboardFilter(BillboardFilterDto createBillboardFilterDto)
        {
            var billboardFilter = await _billboardRepository.CreateBillboardFilterAsync(createBillboardFilterDto);
            return CreatedAtAction(nameof(GetBillboardFilter), new { id = billboardFilter.Id }, billboardFilter);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BillboardFilter>> GetBillboardFilter(Guid id)
        {
            var billboardFilter = await _billboardRepository.GetBillboardFilterAsync(id);
            if (billboardFilter == null)
            {
                return NotFound();
            }
            return billboardFilter;
        }

    }
}