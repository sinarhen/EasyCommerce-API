using ECommerce.Models.DTOs.Billboard;
using ECommerce.Models.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Data.Repositories.Billboard;


public class BillboardRepository : BaseRepository, IBillboardRepository
{
    public BillboardRepository(ProductDbContext db) : base(db)
    {

    }

public async Task<Models.Entities.Billboard> CreateBillboardForCollectionAsync(Guid collectionId, string userId, CreateBillboardDto createBillboardDto)
{
    var collection = await _db.Collections
        .Include(c => c.Store)
        .SingleOrDefaultAsync(c => c.Id == collectionId) 
        ?? throw new ArgumentException("Collection not found");

    if (userId != collection.Store.OwnerId)
    {
        throw new UnauthorizedAccessException("You are not authorized to create a billboard for this collection");
    }
    var billboard = new Models.Entities.Billboard{
        Title = createBillboardDto.Title,
        Subtitle = createBillboardDto.Subtitle,
        ImageUrl = createBillboardDto.ImageUrl,
        CollectionId = collectionId,
    };

    await _db.Billboards.AddAsync(billboard);
    await SaveChangesAsyncWithTransaction();
    return billboard;
}

    public async Task DeleteBillboard(Guid billboardId, string userId)
    {
        var billboard = await _db.FindAsync<Models.Entities.Billboard>(billboardId) ?? throw new ArgumentException("Billboard not found");
        if (billboard.Collection.Store.OwnerId != userId)
        {
            throw new UnauthorizedAccessException("You are not authorized to delete this billboard");
        }
        _db.Billboards.Remove(billboard);

        await SaveChangesAsyncWithTransaction();
    }

    public async Task<Models.Entities.Billboard> GetBillboardAsync(Guid billboardId)
    {
        var billboard = await _db.Billboards
            .Include(b => b.BillboardFilter)
            .FirstOrDefaultAsync(b => b.Id == billboardId);

        if (billboard == null)
        {
            throw new ArgumentException("Billboard not found");
        }

        return billboard;
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
                            .ThenInclude(c => c.Store).Include(billboard => billboard.BillboardFilter)
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
        

        // var filterDto = updateBillboardDto.BillboardFilter;
        // if (filterDto != null)
        // {
        //     if (!string.IsNullOrEmpty(filterDto.OrderBy))
        //     {
        //         billboard.BillboardFilter.OrderBy = filterDto.OrderBy;
        //     }

        //     if (!string.IsNullOrEmpty(filterDto.Search))
        //     {
        //         billboard.BillboardFilter.Search = filterDto.Search;
        //     }
        
        //     billboard.BillboardFilter.FromPrice = filterDto.FromPrice;

        //     billboard.BillboardFilter.ToPrice = filterDto.ToPrice;

        //     if (filterDto.Gender != null && !Enum.IsDefined(typeof(Gender), filterDto.Gender))
        //     {
        //         throw new ArgumentException("Invalid Gender");
        //     }
        //     billboard.BillboardFilter.Title = filterDto.Title;
        //     billboard.BillboardFilter.Subtitle = filterDto.Subtitle;
        // }


        await SaveChangesAsyncWithTransaction();
        return billboard;

    }
}