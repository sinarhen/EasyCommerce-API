using ECommerce.Models.DTOs.User;
using ECommerce.Models.Entities;

namespace ECommerce.Data.Repositories.Admin;

public interface IAdminRepository
{
    Task<IEnumerable<UserDto>> GetAllUsers();
    Task<UserDto> GetUserById(int id);
    Task DeleteUser(int id);
    Task BanUser(int id);
    Task UpdateUserRole(int id, string role);
}