using ECommerce.Models.Entities;

namespace ECommerce.Data.Repositories.Admin;

public interface IAdminRepository
{
    Task<IEnumerable<User>> GetAllUsers();
    Task<User> GetUserById(int id);
    Task DeleteUser(int id);
    Task BanUser(int id);
    Task UpdateUserRole(int id, string role);
}