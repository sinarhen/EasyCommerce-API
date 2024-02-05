using ECommerce.Models.Entities;

namespace ECommerce.Data.Repositories.Admin;

public class AdminRepository: BaseRepository, IAdminRepository
{
    protected AdminRepository(ProductDbContext db) : base(db)
    {
    }

    public Task<IEnumerable<User>> GetAllUsers()
    {
        throw new NotImplementedException();
    }

    public Task<User> GetUserById(int id)
    {
        throw new NotImplementedException();
    }

    public Task DeleteUser(int id)
    {
        throw new NotImplementedException();
    }

    public Task BanUser(int id)
    {
        throw new NotImplementedException();
    }

    public Task UpdateUserRole(int id, string role)
    {
        throw new NotImplementedException();
    }
}