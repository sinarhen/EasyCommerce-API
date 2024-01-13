using System.IdentityModel.Tokens.Jwt;
using AutoMapper;
using ECommerce.Config;
using ECommerce.Models.DTOs.Auth;
using ECommerce.Models.Entities;
using ECommerce.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly UserManager<User> _userManager;
    private readonly JwtService _jwtService;

    public AuthController(
        IMapper mapper,
        UserManager<User> userManager,
        JwtService jwtService
    )
    {
        _mapper = mapper;
        _userManager = userManager;
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

        var user = _mapper.Map<User>(dto);
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

        var token = _jwtService.GenerateToken(user.Id, user.UserName, new List<string> { UserRoles.Customer });
        var tokenAsString = _jwtService.WriteToken(token);

        return CreatedAtAction(
            nameof(Register),
            new
            {
                email = user.Email,
                token = tokenAsString,
                expiresTo = token.ValidTo,
            }
        );

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

        var token = _jwtService.GenerateToken(user.Id, user.UserName, roles);
        var tokenAsString = _jwtService.WriteToken(token);
        return Ok(new
        {
            token = tokenAsString,
            expiration = token.ValidTo,
        });
    }

    [HttpGet("validate/{token}")]
    public ActionResult<SimplePrincipal> ValidateToken(string token)
    {
        var principal = _jwtService.ValidateToken(token);
        if (principal == null)
        {
            return BadRequest();
        }

        return Ok(principal);
    }
    
    [HttpPost("change-password")]
    public async Task<ActionResult> ChangePassword([FromBody] ChangePasswordDto dto)
    {
        if (dto == null)
        {
            return BadRequest("Empty data");
        }

        var user = await _userManager.FindByEmailAsync(dto.Email);
        if (user == null)
        {
            return BadRequest("User with such email does not exist");
        }

        var passwordIsCorrect = await _userManager.CheckPasswordAsync(user, dto.OldPassword);
        if (!passwordIsCorrect)
        {
            return Unauthorized("Password is incorrect");
        }

        var result = await _userManager.ChangePasswordAsync(user, dto.OldPassword, dto.NewPassword);
        if (!result.Succeeded)
        {
            return BadRequest(result.Errors);
        }

        
        var token = _jwtService.GenerateToken(userId: user.Id, username: user.UserName, roles: await _userManager.GetRolesAsync(user));
        
        Console.WriteLine("token: " + _jwtService.WriteToken(token));
        return CreatedAtAction(nameof(ChangePassword),
        new
        {
            token = _jwtService.WriteToken(token),
            expiration = token.ValidTo,
        });
        
    } 
    
    [HttpPost("change-email")]
    public async Task<ActionResult> ChangeEmail([FromBody] ChangeEmailDto dto)
    {
        if (dto == null)
        {
            return BadRequest("Empty data");
        }

        var user = await _userManager.FindByEmailAsync(dto.OldEmail);
        if (user == null)
        {
            return BadRequest("User with such email does not exist");
        }

        var passwordIsCorrect = await _userManager.CheckPasswordAsync(user, dto.Password);
        if (!passwordIsCorrect)
        {
            return Unauthorized("Password is incorrect");
        }

        var result = await _userManager.SetEmailAsync(user, dto.NewEmail);
        if (!result.Succeeded)
        {
            return BadRequest(result.Errors);
        }

        
        var token = _jwtService.GenerateToken(userId: user.Id, username: user.UserName, roles: await _userManager.GetRolesAsync(user));
        
        return CreatedAtAction(nameof(ChangePassword),
            new
            {
                token = _jwtService.WriteToken(token),
                expiration = token.ValidTo,
            });

    }
    
}