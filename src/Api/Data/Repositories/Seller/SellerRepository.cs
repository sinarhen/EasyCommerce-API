using ECommerce.Models.Entities;

namespace ECommerce.Data.Repositories.Seller;

public class SellerRepository: BaseRepository, ISellerRepository
{
    protected SellerRepository(ProductDbContext db) : base(db)
    {
    }

    public Task<bool> UpdateSellerInfo(string id, SellerInfo sellerInfo)
    {
        throw new NotImplementedException();
    }
}