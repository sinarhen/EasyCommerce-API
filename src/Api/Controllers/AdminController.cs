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
    public class AdminController : ControllerBase
    {
        private readonly IAdminRepository _repository;

        public AdminController(IAdminRepository repository)
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
        public ActionResult BanUser(string id, [FromBody] BanUserDto data)
        {
            try {
                _repository.BanUser(data);
                return Ok("Successfully banned user");
            } catch (ArgumentException e)
            {
                return BadRequest(e.Message);
            } catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
        

        [Authorize(Policy = Policies.SuperAdminPolicy)]
        [HttpPut("users/{id}/role")]
        public ActionResult UpdateUserRole(int id, [FromBody] string role)
        {
            // TODO: Implement logic to update user's role by id
            throw new NotImplementedException();
        }
        
    }
}