using ECommerce.Config;
using ECommerce.Models.DTOs.Admin;
using ECommerce.Models.DTOs.User;
using ECommerce.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Data.Repositories.Admin;

public class AdminRepository: BaseRepository, IAdminRepository
{
    public AdminRepository(ProductDbContext db) : base(db)
    {
    }
    private async Task<IEnumerable<string>> GetUserRoles(string userId)
    {
        var roles = await _db.UserRoles
            .Where(r => r.UserId == userId)
            .Select(r => r.RoleId)
            .ToListAsync();

        var roleNames = await _db.Roles
            .Where(r => roles.Contains(r.Id))
            .Select(r => r.Name)
            .ToListAsync();

        return roleNames;
    }
    public async Task<IEnumerable<UserDto>> GetAllUsers()
    {
        var users = await _db.Users
            .AsNoTracking()
            .ToListAsync();

        var userDtos = new List<UserDto>();

        foreach (var user in users)
        {
            var roles = await GetUserRoles(user.Id);
            var highestRole = GetHighestUserRole(roles);
            var isBanned = await _db.BannedUsers.AnyAsync(b => b.UserId == user.Id);

            var userDto = new UserDto
            {
                Id = user.Id,
                Username = user.UserName,
                Email = user.Email,
                Role = highestRole,
                IsBanned = isBanned,
                CreatedAt = user.CreatedAt,
                UpdatedAt = user.UpdatedAt
            };

            userDtos.Add(userDto);
        }

        return userDtos;
    }
    private string GetHighestUserRole(IEnumerable<string> roles)
    {
        var highestRole = roles.MaxBy(r => UserRoles.RoleHierarchy[r ?? UserRoles.Customer]);
        return highestRole ?? UserRoles.Customer;
    }

    public async Task<UserDto> GetUserById(string id)
    {
        var user = await _db.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == id) ?? throw new ArgumentException("User not found");

        var roles = await GetUserRoles(user.Id);
        var highestRole = GetHighestUserRole(roles);
        var isBanned = await _db.BannedUsers.AnyAsync(b => b.UserId == user.Id);

        var userDto = new UserDto
        {
            Id = user.Id,
            Username = user.UserName,
            Email = user.Email,
            Role = highestRole,
            IsBanned = isBanned,
            CreatedAt = user.CreatedAt,
            UpdatedAt = user.UpdatedAt
        };

        return userDto;
    }

    public Task DeleteUser(string id)
    {
        throw new NotImplementedException();
    }

    public async Task BanUser(BanUserDto data)
    {
        var user = await _db.Users
            .FirstOrDefaultAsync(u => u.Id == data.UserId) ?? throw new ArgumentException("User not found");


        var isBanned = await _db.BannedUsers.AnyAsync(b => b.UserId == user.Id);
        if (isBanned)
        {
            throw new ArgumentException("User is already banned");
        }

        var bannedUser = new BannedUser
        {
            UserId = user.Id,
            Reason = data.Reason,
            BanEndTime = data.BanEndTime ?? DateTime.MaxValue,
            BanStartTime = DateTime.UtcNow
        }; 

        await _db.BannedUsers.AddAsync(bannedUser);
        await _db.SaveChangesAsync();

        
    }

    public Task UpdateUserRole(string id, string role)
    {
        throw new NotImplementedException();
    }
}