using ECommerce.Data.Repositories.Billboard;
using ECommerce.Models.DTOs.Billboard;
using ECommerce.Models.Entities;
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

    public async Task<Models.Entities.Billboard> UpdateBillboardAsync(UpdateBillboardDto updateBillboardDto, string userId, Guid billboardId)
    {
        var billboard = await _db.Billboards
            .Include(b => b.Collection)
            .ThenInclude(c => c.Store)
            .FirstOrDefaultAsync(b => b.Id == billboardId) 
            ?? 
            throw new ArgumentException($"Billboard not found: {billboardId}");

        if (billboard.Collection.Store.OwnerId != userId)
        {
            throw new UnauthorizedAccessException("You are not authorized to update this billboard");
        }

        billboard.Title = updateBillboardDto.Title;
        billboard.Subtitle = updateBillboardDto.Subtitle;
        billboard.ImageUrl = updateBillboardDto.ImageUrl;
        billboard.CollectionId = updateBillboardDto.CollectionId ?? billboard.CollectionId;
        

        var filterDto = updateBillboardDto.BillboardFilter;
        if (filterDto != null)
        {
            if (!string.IsNullOrEmpty(filterDto.OrderBy))
            {
                billboard.BillboardFilter.OrderBy = filterDto.OrderBy;
            }

            if (!string.IsNullOrEmpty(filterDto.Search))
            {
                billboard.BillboardFilter.Search = filterDto.Search;
            }
        
            billboard.BillboardFilter.FromPrice = filterDto.FromPrice;

            billboard.BillboardFilter.ToPrice = filterDto.ToPrice;

            if (filterDto.Gender != null && !Enum.IsDefined(typeof(Gender), filterDto.Gender))
            {
                throw new ArgumentException("Invalid Gender");
            }
        }

        billboard.FilterTitle = updateBillboardDto.FilterTitle;
        billboard.FilterSubtitle = updateBillboardDto.FilterSubtitle;


        await SaveChangesAsyncWithTransaction();
        return billboard;

    }
}