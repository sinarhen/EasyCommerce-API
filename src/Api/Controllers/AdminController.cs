using AutoMapper;
using ECommerce.Config;
using ECommerce.Data.Repositories.Admin;
using ECommerce.Models.DTOs.Admin;
using ECommerce.Models.DTOs.User;
using ECommerce.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Controllers
{
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
            try
            {
                return Ok(await _repository.GetAllUsers());
                
            } catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
            
        }

        // DELETE: api/admin/users/{id}
        [HttpDelete("users/{id}")]
        public ActionResult DeleteUser(string id)
        {
            try {
                _repository.DeleteUser(id);
                return Ok("Successfully deleted user");
            } catch (ArgumentException e)
            {
                return BadRequest(e.Message);
            } catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        // GET: api/admin/users/{id}
        [HttpGet("users/{id}")]
        public async Task<ActionResult<User>> GetUserById(string id)
        {
            try {
                var user = await _repository.GetUserById(id);

                return Ok(user);
            } catch (ArgumentException e)
            {
                return BadRequest(e.Message);
            } catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        // PUT: api/admin/users/{id}/ban
        [HttpPut("users/{id}/ban")]
        public async Task<ActionResult> BanUser(string id, [FromBody] BanUserDto data)
        {
            try {
                var bannedUser = await _repository.BanUser(id, data);
                return Ok(new {
                    message = "User has been banned",
                    bannedUser
                });
            } catch (ArgumentException e)
            {
                return BadRequest(e.Message);
            } catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

                // PUT: api/admin/users/{id}/ban
        [HttpPut("users/{id}/unban")]
        public async Task<ActionResult> UnbanUser(string id)
        {
            try {
                await _repository.UnbanUser(id);
                return Ok(new {
                    message = "User has been unbanned",
                });
            } catch (ArgumentException e)
            {
                return BadRequest(e.Message);
            } catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        // GET: api/admin/banned-users
        [HttpGet("banned-users")]
        public async Task<ActionResult<IEnumerable<BannedUser>>> GetBannedUsers()
        {
            try {
                return Ok(await _repository.GetBannedUsers());
            } catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
        

        [Authorize(Policy = Policies.AdminPolicy)]
        [HttpPut("users/{id}/role")]
        public async Task<ActionResult> UpdateUserRole(string id, [FromBody] ChangeUserRoleDto dto)
        {
            try {
                await _repository.UpdateUserRole(id, dto.Role, UserRoles.GetHighestUserRole(GetUserRoles()));
                return Ok(
                    new {
                        message = "User role has been updated"
                    }
                );
            } catch (ArgumentException e)
            {
                return BadRequest(e.Message);
            } catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500, e.Message);
            }
        }

        [HttpGet("upgrade-requests")]
        public async Task<ActionResult<IEnumerable<SellerUpgradeRequestDto>>> GetUpgradeRequests()
        {
            try {
                return Ok(await _repository.GetSellerUpgradeRequests());
            } catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
        
    }


}