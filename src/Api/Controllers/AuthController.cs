using System.IdentityModel.Tokens.Jwt;
using AutoMapper;
using ECommerce.Config;
using ECommerce.Data;
using ECommerce.Models.DTOs.Auth;
using ECommerce.Models.DTOs.User;
using ECommerce.Models.Entities;
using ECommerce.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : GenericController
{
    private readonly ProductDbContext _db;
    private readonly JwtService _jwtService;
    private readonly UserManager<User> _userManager;

    public AuthController(
        IMapper mapper,
        UserManager<User> userManager,
        ProductDbContext db,
        JwtService jwtService
    ) : base(mapper)
    {
        _userManager = userManager;
        _jwtService = jwtService;
        _db = db;
    }

    [HttpPost("register")]
    [ServiceFilter(typeof(ValidationService))]
    public async Task<ActionResult<string>> Register([FromBody] RegisterDto dto)
    {
        // Check if a user with the same email exists
        var userExists = await _userManager.Users.AnyAsync(x => x.Email == dto.Email);

        // Map the DTO to a User object
        var user = _mapper.Map<User>(dto);

        if (userExists) return BadRequest("User with such email already exists");

        var createdUser = await _userManager.CreateAsync(user, dto.Password);
        
        if (!createdUser.Succeeded)
        {
            Console.WriteLine(createdUser.Errors);
            return BadRequest("User was not created");
        }
        
        // Wait for both tasks to complete
        var roleResult = await _userManager.AddToRoleAsync(user, UserRoles.Customer);

        if (!createdUser.Succeeded || !roleResult.Succeeded)
        {
            Console.WriteLine(createdUser.Errors);
            return BadRequest("User was not created");
        }
        // Generate the JWT token
        var token = _jwtService.GenerateToken(user.Id, user.UserName, new List<string> { UserRoles.Customer });
        var tokenAsString = _jwtService.WriteToken(token);

        return CreatedAtAction(
            nameof(Register),
            tokenAsString
        );
    }

    [HttpPost("login")]
    [ServiceFilter(typeof(ValidationService))]
    public async Task<ActionResult<string>> Login([FromBody] LoginDto dto)
    {
        if (dto == null) return UnprocessableEntity(new
        {
            Message = "Empty data", 
            StatusCode = StatusCodes.Status422UnprocessableEntity
        });

        var user = await _userManager.FindByEmailAsync(dto.Email);

        if (user == null) return Unauthorized(
            "User with such email does not exist"
         );

        var passwordIsCorrect = await _userManager.CheckPasswordAsync(user, dto.Password);
        if (!passwordIsCorrect) return Unauthorized("Email or password is incorrect");

        var roles = await _userManager.GetRolesAsync(user);

        var token = _jwtService.GenerateToken(user.Id, user.UserName, roles);
        var tokenAsString = _jwtService.WriteToken(token);
        return Ok(tokenAsString);
    }

    [HttpGet("validate-token")]
    public ActionResult<SimplePrincipal> ValidateToken()
    {
        var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
        var principal = _jwtService.ValidateToken(token);
        if (principal == null) return BadRequest("Invalid token");

        return Ok(principal);
    }

    [HttpGet("refresh-token")]
    public async Task<ActionResult<JwtSecurityToken>> RefreshToken()
    {
        var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
        if (token == null) return BadRequest(new
        {
            Message = "Token is required",
            StatusCode = StatusCodes.Status400BadRequest
            
        });

        var principal = _jwtService.ValidateToken(token);
        if (principal == null) return BadRequest(new
        {
            Message = "Invalid token",
        });

        var id = GetUserId();
        var user = await _userManager.FindByIdAsync(id);
        if (user == null) return BadRequest(new
        {
            Message = "User does not exist",
        });

        var roles = await _userManager.GetRolesAsync(user);

        // rest of your code
        var newToken = _jwtService.GenerateToken(user.Id, user.UserName, roles);
        var tokenAsString = _jwtService.WriteToken(newToken);
        return Ok(new
        {
            token = tokenAsString,
            expiration = newToken.ValidTo
        });
    }


    [HttpGet("me")]
    public async Task<ActionResult<UserDto>> GetMe()
    {
        var id = GetUserId();
        var user = await _userManager.FindByIdAsync(id);
        if (user == null) return BadRequest(new
        {
            Message = "User does not exist",
            StatusCode = StatusCodes.Status400BadRequest
        });
        var roles = await _userManager.GetRolesAsync(user);

        return Ok(new UserDto
        {
            Id = user.Id,
            Username = user.UserName,
            Email = user.Email,
            ImageUrl = user.ImageUrl,
            Role = roles.FirstOrDefault(),
            Roles = roles.ToList(),
            IsBanned = await _db.BannedUsers.AnyAsync(x => x.UserId == user.Id),
            CreatedAt = user.CreatedAt,
            UpdatedAt = user.UpdatedAt
        });
    }

    [HttpPost("change-password")]
    [ServiceFilter(typeof(ValidationService))]
    public async Task<ActionResult> ChangePassword([FromBody] ChangePasswordDto dto)
    {
        if (dto == null) return BadRequest(new
        {
            Message = "Empty data",
        });

        var user = await _userManager.FindByEmailAsync(dto.Email);
        if (user == null) return BadRequest(new
        {
            Message = "User with such email does not exist",
            Field = "email",
        });

        var passwordIsCorrect = await _userManager.CheckPasswordAsync(user, dto.OldPassword);
        if (!passwordIsCorrect) return Unauthorized(new
        {
            Field = "password",
            Message = "Password is incorrect",
        });

        var result = await _userManager.ChangePasswordAsync(user, dto.OldPassword, dto.NewPassword);
        if (!result.Succeeded) return BadRequest(result.Errors);


        var token = _jwtService.GenerateToken(user.Id, user.UserName, await _userManager.GetRolesAsync(user));

        return CreatedAtAction(nameof(ChangePassword),
            new
            {
                token = _jwtService.WriteToken(token),
                expiration = token.ValidTo
            });
    }

    [HttpPost("change-email")]
    [ServiceFilter(typeof(ValidationService))]
    public async Task<ActionResult> ChangeEmail([FromBody] ChangeEmailDto dto)
    {
        if (dto == null) return BadRequest(new
        {
            Message = "Empty data",
        });

        var user = await _userManager.FindByEmailAsync(dto.OldEmail);
        if (user == null) return BadRequest(new
        {
            Message = "User with such email does not exist",
            Field = "email",
        });

        var passwordIsCorrect = await _userManager.CheckPasswordAsync(user, dto.Password);
        if (!passwordIsCorrect) return Unauthorized(new
        {
            Field = "password",
            Message = "Password is incorrect",
        });

        var result = await _userManager.SetEmailAsync(user, dto.NewEmail);
        if (!result.Succeeded) return BadRequest(result.Errors);


        var token = _jwtService.GenerateToken(user.Id, user.UserName, await _userManager.GetRolesAsync(user));

        return CreatedAtAction(nameof(ChangePassword),
            new
            {
                token = _jwtService.WriteToken(token),
                expiration = token.ValidTo
            });
    }
}