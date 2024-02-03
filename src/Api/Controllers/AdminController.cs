using System;
using System.Collections.Generic;
using ECommerce.Models.Entities;
using Microsoft.AspNetCore.Mvc;

namespace YourNamespace.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : ControllerBase
    {
        // GET: api/admin/users
        [HttpGet("users")]
        public ActionResult<IEnumerable<User>> GetAllUsers()
        {
            // TODO: Implement logic to get all users
            throw new NotImplementedException();
        }

        // DELETE: api/admin/users/{id}
        [HttpDelete("users/{id}")]
        public ActionResult DeleteUser(int id)
        {
            // TODO: Implement logic to delete specific user by id
            throw new NotImplementedException();
        }

        // GET: api/admin/users/{id}
        [HttpGet("users/{id}")]
        public ActionResult<User> GetUserById(int id)
        {
            // TODO: Implement logic to get user by id
            throw new NotImplementedException();
        }

        // PUT: api/admin/users/{id}/ban
        [HttpPut("users/{id}/ban")]
        public ActionResult BanUser(int id)
        {
            // TODO: Implement logic to ban user by id
            throw new NotImplementedException();
        }

        // PUT: api/admin/users/{id}/role
        [HttpPut("users/{id}/role")]
        public ActionResult UpdateUserRole(int id, [FromBody] string role)
        {
            // TODO: Implement logic to update user's role by id
            throw new NotImplementedException();
        }
        
    }
}