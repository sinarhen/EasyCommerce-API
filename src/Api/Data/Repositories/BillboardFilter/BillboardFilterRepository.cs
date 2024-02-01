using ECommerce.Models.DTOs.Billboard;
using ECommerce.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Data.Repositories.BillboardFilter;
public class BillboardFilterRepository : BaseRepository, IBillboardFilterRepository
{
    public BillboardFilterRepository(ProductDbContext context) : base(context)
    {
        
    }

    
    public async Task<Models.Entities.BillboardFilter> CreateBillboardFilterAsync(string billboardId, string userId, BillboardFilterDto writeBillboardFilterDto, bool? isAdmin)
    {
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
            SizeId = writeBillboardFilterDto.SizeId,
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
        if (billboardFilter == null)
        {
            throw new ArgumentException("BillboardFilter not found");
        }

        if (billboardFilter.Billboard.Collection.Store.OwnerId != userId)
        {
            throw new UnauthorizedAccessException("You do not have permission to delete this BillboardFilter");
        }


        _db.BillboardFilters.Remove(billboardFilter);
        await _db.SaveChangesAsync();
    }
    private static void ValidateUser(string userId, Models.Entities.BillboardFilter billboardFilter, bool? isAdmin)
    {
        if (billboardFilter.Billboard.Collection.Store.OwnerId != userId && !isAdmin.Value)
        {
            throw new UnauthorizedAccessException("You do not have permission to delete this BillboardFilter");
        }
    }

    public async Task<Models.Entities.BillboardFilter> GetBillboardFilterAsync(string userId, Guid id, bool? isAdmin)
    {
        var billboardFilter = await _db.BillboardFilters.FirstOrDefaultAsync(b => b.Id == id);
        if (billboardFilter == null)
        {
            throw new ArgumentException("BillboardFilter not found");
        }
        ValidateUser(userId, billboardFilter, isAdmin.Value);

        return billboardFilter;
    }


    public async Task<Models.Entities.BillboardFilter> UpdateBillboardFilterAsync(string userId, Guid id, BillboardFilterDto writeBillboardFilterDto, bool? isAdmin)
    {
        var billboardFilter = await _db.BillboardFilters.FirstOrDefaultAsync(b => b.Id == id);
        if (billboardFilter == null)
        {
            throw new ArgumentException("BillboardFilter not found");
        }

        if (billboardFilter.Billboard.Collection.Store.OwnerId != userId && !isAdmin.Value)
        {
            throw new UnauthorizedAccessException("You do not have permission to update this BillboardFilter");
        }
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