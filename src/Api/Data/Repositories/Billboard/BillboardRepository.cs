using ECommerce.Models.DTOs.Billboard;
using ECommerce.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Data.Repositories.Billboard;


public class BillboardRepository : BaseRepository, IBillboardRepository
{
    public BillboardRepository(ProductDbContext db) : base(db)
    {

    }

    public async Task<Models.Entities.Billboard> CreateBillboardForCollectionAsync(Guid collectionId, string userId, CreateBillboardDto createBillboardDto, bool isAdmin)
    {
        Console.WriteLine("Entered the function" + userId);
        if (createBillboardDto == null)
        {
            throw new ArgumentException("Request body is empty");
        }
        var collection = await _db.Collections
            .Include(c => c.Store)
            .SingleOrDefaultAsync(c => c.Id == collectionId) 
            ?? throw new ArgumentException("Collection not found");

        var isOwner = ValidateOwner(collection.Store.OwnerId, userId, isAdmin);
        if (!isOwner)
        {
            throw new UnauthorizedAccessException("You are not authorized to create a billboard for this collection");
        }
        Console.WriteLine("Stage: 1");
        
        var billboard = new Models.Entities.Billboard
        {
            Title = createBillboardDto.Title,
            Subtitle = createBillboardDto.Subtitle,
            ImageUrl = createBillboardDto.ImageUrl,
            CollectionId = collectionId,
            BillboardFilter = new Models.Entities.BillboardFilter()
        };

        if (createBillboardDto.BillboardFilter != null)
        {
            if (createBillboardDto.BillboardFilter.CategoryId != Guid.Empty)
            {
                var category = await _db.Categories.FindAsync(createBillboardDto.BillboardFilter.CategoryId);
                if (category == null)
                {
                    throw new ArgumentException("Category for billboard filter not found");
                }
            }
            if (createBillboardDto.BillboardFilter.ColorId != Guid.Empty)
            {
                var color = await _db.Colors.FindAsync(createBillboardDto.BillboardFilter.ColorId) ?? throw new ArgumentException("Color for billboard filter not found");
            }
            if (createBillboardDto.BillboardFilter.SizeId != Guid.Empty)
            {
                var size = await _db.Sizes.FindAsync(createBillboardDto.BillboardFilter.SizeId) 
                    ?? throw new ArgumentException("Size for billboard filter not found");
            }
            if (!string.IsNullOrEmpty(createBillboardDto.BillboardFilter.OrderBy))
            {
                billboard.BillboardFilter.OrderBy = createBillboardDto.BillboardFilter.OrderBy;
            }
            if (!string.IsNullOrEmpty(createBillboardDto.BillboardFilter.Search))
            {
                billboard.BillboardFilter.Search = createBillboardDto.BillboardFilter.Search;
            }
            billboard.BillboardFilter.FromPrice = createBillboardDto.BillboardFilter.FromPrice;
            billboard.BillboardFilter.ToPrice = createBillboardDto.BillboardFilter.ToPrice;

        }
        

        await _db.Billboards.AddAsync(billboard);
        await SaveChangesAsyncWithTransaction();

        return billboard;
    }

    public async Task DeleteBillboardAsync(Guid billboardId, string userId, bool isAdmin)
    {
        var billboard = await _db.FindAsync<Models.Entities.Billboard>(billboardId) ?? throw new ArgumentException("Billboard not found");
        var isOwner = ValidateOwner(billboard.Collection.Store.OwnerId, userId, isAdmin);
        if (!isOwner)
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

    public async Task<Models.Entities.Billboard> UpdateBillboardAsync(UpdateBillboardDto updateBillboardDto, string userId, Guid billboardId, bool isAdmin)
    {
        var billboard = await _db.Billboards
                            .Include(b => b.Collection)
                            .ThenInclude(c => c.Store).Include(billboard => billboard.BillboardFilter)
                            .FirstOrDefaultAsync(b => b.Id == billboardId) 
            ?? 
            throw new ArgumentException($"Billboard not found: {billboardId}");

        var isOwner = ValidateOwner(billboard.Collection.Store.OwnerId, userId);
        if (!isOwner)
        {
            throw new UnauthorizedAccessException("You are not authorized to update this billboard");
        }
        billboard.Title = updateBillboardDto.Title;
        billboard.Subtitle = updateBillboardDto.Subtitle;
        billboard.ImageUrl = updateBillboardDto.ImageUrl;
        billboard.CollectionId = updateBillboardDto.CollectionId != Guid.Empty ? updateBillboardDto.CollectionId : billboard.CollectionId;
        

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
            billboard.BillboardFilter.Title = filterDto.Title;
            billboard.BillboardFilter.Subtitle = filterDto.Subtitle;
        }


        await SaveChangesAsyncWithTransaction();
        return billboard;

    }
}