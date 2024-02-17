using AutoMapper;
using ECommerce.Config;
using ECommerce.Data.Repositories.Admin;
using ECommerce.Models.DTOs.Admin;
using ECommerce.Models.DTOs.User;
using ECommerce.Models.Entities;
using ECommerce.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Extensions;

namespace ECommerce.Controllers;

[ApiController]
[Authorize(Policy = Policies.AdminPolicy)]
[Route("api/admin")]
public class AdminController : GenericController
{
    private readonly IAdminRepository _repository;

    public AdminController(IAdminRepository repository, IMapper mapper) : base(mapper)
    {
        _repository = repository;
    }

    // GET: api/admin/users
    [HttpGet("users")]
    public async Task<ActionResult<IEnumerable<UserDto>>> GetAllUsers()
    {
        return Ok(await _repository.GetAllUsers());
    }

    // DELETE: api/admin/users/{id}
    [HttpDelete("users/{id}")]
    public ActionResult DeleteUser(string id)
    {
        _repository.DeleteUser(id);
        return Ok("Successfully deleted user");
    }

    // GET: api/admin/users/{id}
    [HttpGet("users/{id}")]
    public async Task<ActionResult<User>> GetUserById(string id)
    {
        var user = await _repository.GetUserById(id);

        return Ok(user);
    }

    // PUT: api/admin/users/{id}/ban
    [HttpPut("users/{id}/ban")]
    [ServiceFilter(typeof(ValidationService))]
    public async Task<ActionResult> BanUser(string id, [FromBody] BanUserDto data)
    {
        var bannedUser = await _repository.BanUser(id, data);
        return Ok(new
        {
            message = "User has been banned",
            bannedUser
        });
    }


    // PUT: api/admin/users/{id}/ban
    [HttpPut("users/{id}/unban")]
    public async Task<ActionResult> UnbanUser(string id)
    {
        await _repository.UnbanUser(id);
        return Ok(new
        {
            message = "User has been unbanned"
        });
    }

    // GET: api/admin/users/banned
    [HttpGet("users/banned")]
    public async Task<ActionResult<IEnumerable<BannedUser>>> GetBannedUsers()
    {
        return Ok(await _repository.GetBannedUsers());
    }


    [Authorize(Policy = Policies.AdminPolicy)]
    [HttpPut("users/{id}/role")]
    public async Task<ActionResult> UpdateUserRole(string id, [FromBody] ChangeUserRoleDto dto)
    {
        await _repository.UpdateUserRole(id, dto.Role, UserRoles.GetHighestUserRole(GetUserRoles()));
        return Ok(
            new
            {
                message = "User role has been updated"
            }
        );
    }

    [HttpGet("users/upgrade-requests")]
    public async Task<ActionResult<IEnumerable<SellerUpgradeRequestDto>>> GetUpgradeRequests()
    {
        return Ok(await _repository.GetSellerUpgradeRequests());
    }

    [HttpGet("users/upgrade-requests/{id:guid}")]
    public async Task<ActionResult<SellerUpgradeRequestDetailsDto>> GetUpgradeRequestById(Guid id)
    {
        return Ok(await _repository.GetSellerUpgradeRequestById(id));
    }

    [HttpPut("users/upgrade-requests/{id:guid}")]
    [ServiceFilter(typeof(ValidationService))]
    public async Task<ActionResult> UpdateUpgradeRequestStatus(Guid id, [FromBody] SellerUpgradeRequestDto dto)
    {
        if (dto.Status == null) return BadRequest("Status is required");

        if (SellerUpgradeRequestStatus.Approved.GetDisplayName() != dto.Status &&
            SellerUpgradeRequestStatus.Rejected.GetDisplayName() != dto.Status)
            return BadRequest("Invalid status. Valid statuses are `Approved` or `Rejected`");
        await _repository.UpgradeSellerUpgradeRequestStatus(id, dto.Message, dto.Status);
        return Ok(new
        {
            message = "Seller upgrade request status has been updated"
        });
    }
}