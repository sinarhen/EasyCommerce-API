﻿using ECommerce.Config;
using ECommerce.Models.DTOs.Admin;
using ECommerce.Models.DTOs.User;
using ECommerce.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Extensions;

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
            var isBanned = await _db.BannedUsers.AnyAsync(b => b.UserId == user.Id);

            var userDto = new UserDto
            {
                Id = user.Id,
                Username = user.UserName,
                Email = user.Email,
                Role = UserRoles.GetHighestUserRole(roles),
                IsBanned = isBanned,
                CreatedAt = user.CreatedAt,
                UpdatedAt = user.UpdatedAt
            };

            userDtos.Add(userDto);
        }

        return userDtos;
    }

    public async Task<UserDto> GetUserById(string id)
    {
        var user = await FindUser(id) ?? throw new ArgumentException("User not found");

        var roles = await GetUserRoles(user);
        var isBanned = await _db.BannedUsers.AnyAsync(b => b.UserId == user.Id);

        var userDto = new UserDto
        {
            Id = user.Id,
            Username = user.UserName,
            Email = user.Email,
            Role = UserRoles.GetHighestUserRole(roles),
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
        var user = await FindUser(id) ?? throw new ArgumentException("User not found");

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
        await SaveChangesAsyncWithTransaction();


        return bannedUser;
    }

    public async Task UnbanUser(string id)
    {
        var user = await FindUser(id) ?? throw new ArgumentException("User not found");

        var bannedUser = await _db.BannedUsers
            .FirstOrDefaultAsync(b => b.UserId == id) ?? throw new ArgumentException("User is not banned");
        
        _db.BannedUsers.Remove(bannedUser);
        await SaveChangesAsyncWithTransaction();

        return;
    }

    public async Task<IEnumerable<BannedUser>> GetBannedUsers()
    {
        var bannedUsers = await _db.BannedUsers
            .AsNoTracking()
            .ToListAsync();

        return bannedUsers;
    }
    public async Task UpdateUserRole(string id, string role, string adminRole)
    {
        var user = await FindUser(id) ?? throw new ArgumentException("User not found");

        Console.WriteLine("USER:", user);
        var userRoles = await GetUserRoles(user);
        
        Console.WriteLine("USER ROLES:");
        foreach (var r in userRoles)
        {
            Console.WriteLine(r);
            
        }
        if (UserRoles.GetHighestUserRole(userRoles) == role)
        {
            throw new ArgumentException("User already in this role");
        }

        if (UserRoles.RoleHierarchy[role] >= UserRoles.RoleHierarchy[adminRole])
        {
            throw new ArgumentException("You cannot assign a role higher than your own or equal to your role");
        }
        var allRoles = UserRoles.GetAllRoles();

        var level = UserRoles.RoleHierarchy[role];
        var highestRole = UserRoles.GetHighestUserRole(userRoles);
        if (UserRoles.RoleHierarchy[highestRole] > UserRoles.RoleHierarchy[role])
        {
            // remove all roles that are higher than the new role
            var rolesToRemove = allRoles.Where(r => r != UserRoles.Customer && UserRoles.RoleHierarchy[r] > level && userRoles.Contains(r));
            await _userManager.RemoveFromRolesAsync(user, rolesToRemove);
        }
        else if (UserRoles.RoleHierarchy[highestRole] < UserRoles.RoleHierarchy[role])
        {
            // add all roles that are lower than the new role
            var rolesToAdd = allRoles.Where(r => UserRoles.RoleHierarchy[r] <= level && !userRoles.Contains(r));
            await _userManager.AddToRolesAsync(user, rolesToAdd);
        }
        await SaveChangesAsyncWithTransaction();
        return;

    }

    public async Task<IEnumerable<SellerUpgradeRequestDto>> GetSellerUpgradeRequests()
    {
        return await _db.SellerUpgradeRequests
            .AsNoTracking()
            .Where(r => _db.BannedUsers.All(b => b.UserId != r.UserId))
            .Select(r => new SellerUpgradeRequestDto
            {
                Id = r.Id,
                Status = r.Status.GetDisplayName(),
                DecidedAt = r.DecidedAt,
                Message = r.Message,
                User = new UserDto 
                {
                    Id = r.UserId,
                    Username = r.User.UserName,
                    Email = r.User.Email,
                    CreatedAt = r.User.CreatedAt,
                    UpdatedAt = r.User.UpdatedAt,
                    ImageUrl = r.User.ImageUrl

                }
            })
            .ToListAsync();
    }

    public async Task<SellerUpgradeRequestDetailsDto> GetSellerUpgradeRequestById(Guid id)
    {
        return await _db.SellerUpgradeRequests
            .AsNoTracking()
            .Where(r => r.Id == id)
            .Select(r => new SellerUpgradeRequestDetailsDto
            {
                Id = r.Id,
                Status = r.Status.GetDisplayName(),
                DecidedAt = r.DecidedAt,
                Message = r.Message,
                SellerInfo = new SellerInfo
                {
                    CompanyName = r.SellerInfo.CompanyName,
                    CompanyDescription = r.SellerInfo.CompanyDescription,
                    CompanyEmail = r.SellerInfo.CompanyEmail,
                    CompanyPhone = r.SellerInfo.CompanyPhone,
                    
                },
            })
            .FirstOrDefaultAsync();
    }

    public Task ApproveSellerUpgradeRequest(Guid id, string message)
    {
        throw new NotImplementedException();
    }
}