using ECommerce.Config;
using ECommerce.Models.DTOs.User;
using ECommerce.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Data.Repositories.Admin;

public class AdminRepository: BaseRepository, IAdminRepository
{
    protected AdminRepository(ProductDbContext db) : base(db)
    {
    }
    
    private string GetHighestUserRole(string userId)
    {
        var userRoles = _db.UserRoles.Where(ur => ur.UserId == userId);
        var roles = _db.Roles.Where(r => userRoles.Any(ur => ur.RoleId == r.Id));
        var highestRole = roles.OrderByDescending(r => UserRoles.RoleHierarchy[r.Name]).FirstOrDefault();
        return highestRole?.Name;
    }
    public async Task<IEnumerable<UserDto>> GetAllUsers()
    {
        var users = await _db.Users
            .Select(u => new UserDto
            {
                Id = u.Id,
                Username = u.UserName,
                Email = u.Email,
                Role = GetHighestUserRole(u.Id),
                IsBanned = _db.BannedUsers.Any(b => b.UserId == u.Id),
                CreatedAt = u.CreatedAt,
                UpdatedAt = u.UpdatedAt
            })
            .AsNoTracking()
            .ToListAsync();
        return users;
    }

    public Task<UserDto> GetUserById(int id)
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