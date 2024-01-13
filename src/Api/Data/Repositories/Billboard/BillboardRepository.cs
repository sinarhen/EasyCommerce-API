using ECommerce.Data.Repositories.Billboard;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Data.Repositories;


public class BillboardRepository : BaseRepository, IBillboardRepository
{
    public BillboardRepository(ProductDbContext db) : base(db)
    {

    }

    public Task<Models.Entities.Billboard> CreateBillboardForCollectionAsync(CreateBillboardDto createBillboardDto)
    {
        throw new NotImplementedException();
    }

    public Task DeleteBillboard(Guid billboardId)
    {
        throw new NotImplementedException();
    }

    public Task<Models.Entities.Billboard> GetBillboardAsync(Guid billboardId)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<Models.Entities.Billboard>> GetBillboardsForCollectionAsync(Guid collectionId)
    {
        return await _db.Billboards.AsNoTracking()
            .Include(c => c.BillboardFilter)
            .Where(c => c.CollectionId == collectionId).ToListAsync();

        
    }

    public Task<Models.Entities.Billboard> UpdateBillboard(UpdateBillboardDto updateBillboardDto)
    {
        throw new NotImplementedException();
    }
}