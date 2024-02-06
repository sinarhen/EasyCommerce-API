using ECommerce.Config;
using ECommerce.Models.DTOs.Admin;
using ECommerce.Models.DTOs.User;
using ECommerce.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Data.Repositories.Admin;

public class AdminRepository: BaseRepository, IAdminRepository
{
    private readonly UserManager<User> _userManager;
    public AdminRepository(ProductDbContext db, UserManager<User> userManager) : base(db)
    {
        _userManager = userManager;
    }
    private async Task<IEnumerable<string>> GetUserRoles(User user)
    {
        var roles = await _userManager.GetRolesAsync(user);
        return roles;
        
    }

    private async Task<User> FindUser(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        return user;
    }

    public async Task<IEnumerable<UserDto>> GetAllUsers()
    {
        var users = await _userManager.Users
            .AsNoTracking()
            .ToListAsync();

        var userDtos = new List<UserDto>();

        foreach (var user in users)
        {
            var roles = await GetUserRoles(user);
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
    private static string GetHighestUserRole(IEnumerable<string> roles)
    {
        var highestRole = roles.MaxBy(r => UserRoles.RoleHierarchy[r ?? UserRoles.Customer]);
        return highestRole ?? UserRoles.Customer;
    }

    public async Task<UserDto> GetUserById(string id)
    {
        var user = await FindUser(id) ?? throw new ArgumentException("User not found");

        var roles = await GetUserRoles(user);
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

    public async Task<BannedUser> BanUser(string id, BanUserDto data)
    {
        var user = FindUser(id) ?? throw new ArgumentException("User not found");

        if (user == null)
        {
            throw new ArgumentException("User not found");
        }

        var isBanned = await _db.BannedUsers.AnyAsync(b => b.UserId == id);
        if (isBanned)
        {
            throw new ArgumentException("User is already banned");
        }

        var bannedUser = new BannedUser
        {
            UserId = id,
            Reason = data.Reason,
            BanEndTime = data.BanEndTime ?? DateTime.MaxValue,
            BanStartTime = DateTime.UtcNow
        };
        await _db.BannedUsers.AddAsync(bannedUser);
        await _db.SaveChangesAsync();

        return bannedUser;
    }

    public async Task UnbanUser(string id)
    {
        var user = FindUser(id) ?? throw new ArgumentException("User not found");

        var bannedUser = await _db.BannedUsers
            .FirstOrDefaultAsync(b => b.UserId == id) ?? throw new ArgumentException("User is not banned");
        
        _db.BannedUsers.Remove(bannedUser);
        await _db.SaveChangesAsync();

        return;
    }

    public async Task<IEnumerable<BannedUser>> GetBannedUsers()
    {
        var bannedUsers = await _db.BannedUsers
            .AsNoTracking()
            .ToListAsync();

        return bannedUsers;
    }
    public async Task UpdateUserRole(string id, string role)
    {
        var user = FindUser(id) ?? throw new ArgumentException("User not found");

        var userRoles = await _db.UserRoles
            .Where(r => r.UserId == id)
            .ToListAsync();

        // TODO: Complete the implementation of updating user's role    
    }
}