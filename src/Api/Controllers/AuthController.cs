using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using AutoMapper;
using ECommerce.Config;
using Ecommerce.Data;
using Ecommerce.DTOs;
using Ecommerce.Entities;
using ECommerce.Services;
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
    private readonly JwtService _jwtService;

    public AuthController(
        IMapper mapper,
        UserManager<Customer> userManager, 
        SignInManager<Customer> signInManager,
        JwtService jwtService
    )
    {
        _mapper = mapper;
        _userManager = userManager;
        _signInManager = signInManager;
        _jwtService = jwtService;
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
        var roleResult = await _userManager.AddToRoleAsync(user, UserRoles.Customer);
        if (!roleResult.Succeeded)
        {
            return BadRequest(result.Errors);
        } 
        return Ok();
    }

    [HttpPost("login")]
    public async Task<ActionResult<JwtSecurityToken>> Login([FromBody] LoginDto dto)
    {
        if (dto == null)
        {
            return BadRequest("Empty data");
        }
        var user = await _userManager.FindByEmailAsync(dto.Email);
        
        if (user == null)
        {
            return Unauthorized();
        }

        var passwordIsCorrect = await _userManager.CheckPasswordAsync(user, dto.Password);
        if (!passwordIsCorrect)
        {
            return Unauthorized("Password is incorrect");
        }
        
        var roles = await _userManager.GetRolesAsync(user);
        var authClaims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        };
        foreach (var role in roles)
        {
            authClaims.Add(new Claim(ClaimTypes.Role, role));
        }

        string token = _jwtService.GenerateToken(authClaims);        
        return Ok(token);
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