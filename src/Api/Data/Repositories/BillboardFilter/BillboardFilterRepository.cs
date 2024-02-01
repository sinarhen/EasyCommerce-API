using ECommerce.Models.DTOs.Billboard;
using ECommerce.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Data.Repositories.BillboardFilter;
public class BillboardFilterRepository : BaseRepository, IBillboardFilterRepository
{
    public BillboardFilterRepository(ProductDbContext context) : base(context)
    {
    }
    public Task<Models.Entities.BillboardFilter> CreateBillboardFilterAsync(BillboardFilterDto writeBillboardFilterDto)
    {
        throw new NotImplementedException();
    }

    public Task DeleteBillboardFilterAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<Models.Entities.BillboardFilter> GetBillboardFilterAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Models.Entities.BillboardFilter>> GetBillboardFiltersAsync(BillboardFilterDto billboardFilterDto)
    {
        throw new NotImplementedException();
    }

    public Task<Models.Entities.BillboardFilter> UpdateBillboardFilterAsync(Guid id, BillboardFilterDto writeBillboardFilterDto)
    {
        throw new NotImplementedException();
    }
}
