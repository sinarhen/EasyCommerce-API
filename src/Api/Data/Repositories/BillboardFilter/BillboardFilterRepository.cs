using ECommerce.Models.DTOs.Billboard;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Data.Repositories.BillboardFilter;

public class BillboardFilterRepository : BaseRepository, IBillboardFilterRepository
{
    public BillboardFilterRepository(ProductDbContext context) : base(context)
    {
    }


    public async Task<Models.Entities.BillboardFilter> CreateBillboardFilterAsync(Guid billboardId, string userId,
        BillboardFilterDto writeBillboardFilterDto, bool isAdmin = false)
    {
        var billboard = await _db.Billboards.Include(b => b.Collection).ThenInclude(c => c.Store)
            .FirstOrDefaultAsync(b => b.Id == billboardId);
        if (billboard == null) throw new ArgumentException("Billboard not found");
        var ownerId = billboard.Collection.Store.OwnerId;
        ValidateOwner(userId, ownerId, isAdmin);
        var billboardFilter = new Models.Entities.BillboardFilter
        {
            Title = writeBillboardFilterDto.Title,
            Subtitle = writeBillboardFilterDto.Subtitle,
            Gender = writeBillboardFilterDto.Gender,
            Season = writeBillboardFilterDto.Season,
            OrderBy = writeBillboardFilterDto.OrderBy,
            FromPrice = writeBillboardFilterDto.FromPrice,
            ToPrice = writeBillboardFilterDto.ToPrice,
            Search = writeBillboardFilterDto.Search,
            CategoryId = writeBillboardFilterDto.CategoryId,
            ColorId = writeBillboardFilterDto.ColorId,
            SizeId = writeBillboardFilterDto.SizeId
        };

        await _db.BillboardFilters.AddAsync(billboardFilter);
        await _db.SaveChangesAsync();

        return billboardFilter;
    }

    public async Task DeleteBillboardFilterAsync(string userId, Guid id, bool? isAdmin)
    {
        var billboardFilter = await _db.BillboardFilters
            .Include(bf => bf.Billboard)
            .ThenInclude(b => b.Collection)
            .ThenInclude(c => c.Store)
            .FirstOrDefaultAsync(b => b.Id == id);
        if (billboardFilter == null) throw new ArgumentException("BillboardFilter not found");

        var ownerId = billboardFilter.Billboard.Collection.Store.OwnerId;
        ValidateOwner(userId, ownerId, isAdmin.Value);

        _db.BillboardFilters.Remove(billboardFilter);
        await _db.SaveChangesAsync();
    }

    public async Task<Models.Entities.BillboardFilter> GetBillboardFilterAsync(string userId, Guid id,
        bool isAdmin = false)
    {
        var billboardFilter = await _db.BillboardFilters.FirstOrDefaultAsync(b => b.Id == id);
        if (billboardFilter == null) throw new ArgumentException("BillboardFilter not found");

        var ownerId = billboardFilter.Billboard.Collection.Store.OwnerId;
        ValidateOwner(userId, ownerId, isAdmin);

        return billboardFilter;
    }


    public async Task<Models.Entities.BillboardFilter> UpdateBillboardFilterAsync(string userId, Guid id,
        BillboardFilterDto writeBillboardFilterDto, bool isAdmin = false)
    {
        var billboardFilter = await _db.BillboardFilters.Include(bf => bf.Billboard).ThenInclude(b => b.Collection)
            .ThenInclude(c => c.Store).FirstOrDefaultAsync(b => b.Id == id);
        if (billboardFilter == null) throw new ArgumentException("BillboardFilter not found");
        var ownerId = billboardFilter.Billboard.Collection.Store.OwnerId;
        ValidateOwner(userId, ownerId, isAdmin);

        billboardFilter.Title = writeBillboardFilterDto.Title;
        billboardFilter.Subtitle = writeBillboardFilterDto.Subtitle;
        billboardFilter.Gender = writeBillboardFilterDto.Gender;
        billboardFilter.Season = writeBillboardFilterDto.Season;
        billboardFilter.OrderBy = writeBillboardFilterDto.OrderBy;
        billboardFilter.FromPrice = writeBillboardFilterDto.FromPrice;
        billboardFilter.ToPrice = writeBillboardFilterDto.ToPrice;
        billboardFilter.Search = writeBillboardFilterDto.Search;
        billboardFilter.CategoryId = writeBillboardFilterDto.CategoryId;
        billboardFilter.ColorId = writeBillboardFilterDto.ColorId;
        billboardFilter.SizeId = writeBillboardFilterDto.SizeId;

        await _db.SaveChangesAsync();

        return billboardFilter;
    }
}