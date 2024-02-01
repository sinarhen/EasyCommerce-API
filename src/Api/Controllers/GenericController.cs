using System.Security.Claims;
using AutoMapper;
using ECommerce.Config;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Controllers;

public class GenericController : ControllerBase
{
    protected readonly IMapper _mapper;
    public GenericController(IMapper mapper)
    {
        _mapper = mapper;
    }


    protected string GetUserId()
    {
        return User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    }

    protected bool IsAdmin()
    {
        return User.IsInRole(UserRoles.Admin) || User.IsInRole(UserRoles.SuperAdmin);
    }

    protected bool IsSuperAdmin()
    {
        return User.IsInRole(UserRoles.SuperAdmin);
    }

    protected List<string> GetUserRoles()
    {
        return User.FindAll(ClaimTypes.Role).Select(c => c.Value).ToList();
            
    }
}