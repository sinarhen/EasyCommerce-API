using ECommerce.Models.DTOs.Admin;
using ECommerce.Models.DTOs.User;
using ECommerce.Models.Entities;

namespace ECommerce.Data.Repositories.Admin;

public interface IAdminRepository
{
    Task<IEnumerable<UserDto>> GetAllUsers();
    Task<UserDto> GetUserById(string id);
    Task DeleteUser(string id);
    Task BanUser(BanUserDto data);
    Task UpdateUserRole(string id, string role);
}