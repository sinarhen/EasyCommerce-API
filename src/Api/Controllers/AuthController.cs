using AutoMapper;
using Ecommerce.Data;
using Ecommerce.DTOs;
using Ecommerce.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly UserManager<Customer> _userManager;
    private readonly SignInManager<Customer> _signInManager;
    private readonly RoleManager<CustomerRole> _roleManager;

    public AuthController(
        IMapper _mapper,
        UserManager<Customer> _userManager, 
        SignInManager<Customer> _signInManager,
        RoleManager<CustomerRole> _roleManager
    )
    {
        this._mapper = _mapper;
        this._userManager = _userManager;
        this._signInManager = _signInManager;
        this._roleManager = _roleManager;
    }

    [HttpPost("register")]
    public async Task<ActionResult> Register([FromBody] RegisterDto dto)
    {
        if (await _userManager.Users.AnyAsync(x => x.Email == dto.Email))
        {
            return BadRequest("Email already in use");
        }

        if (dto == null)
        {
            return BadRequest("Empty data");
        }
                
        var user = _mapper.Map<Customer>(dto);
        var result = await _userManager.CreateAsync(user, dto.Password);
        if (!result.Succeeded)
        {
            return BadRequest(result.Errors);
        }
        
        return Ok();
    }
    
    // [HttpPost("register")]
    // public async Task<IActionResult> Register([FromBody] RegisterDto dto)
    // {
    //     var user = new Customer
    //     {
    //         UserName = dto.Email,
    //         Email = dto.Email,
    //         FirstName = dto.FirstName,
    //         LastName = dto.LastName,
    //         Address = dto.Address,
    //         City = dto.City,
    //         Country = dto.Country,
    //         PostalCode = dto.PostalCode
    //     };
    //
    //     var result = await _context.Users.AddAsync(user);
    //     await _context.SaveChangesAsync();
    //
    //     return Ok(result.Entity);
    // }
}