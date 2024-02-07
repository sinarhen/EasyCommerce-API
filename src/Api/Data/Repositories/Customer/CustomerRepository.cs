using ECommerce.Data;
using ECommerce.Data.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories.Customer;

public class CustomerRepository : BaseRepository, ICustomerRepository
{
    public CustomerRepository(ProductDbContext context) : base(context)
    {
    }


    public async Task<List<ECommerce.Models.Entities.Review>> GetReviewsForUser(string userId)
    {
        // TODO: Return product name for each review
        return await _db.Reviews.Where(r => r.CustomerId == userId).ToListAsync();
    }
}
