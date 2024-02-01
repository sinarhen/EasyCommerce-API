using System.Security.Claims;
using AutoMapper;
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
        return User.IsInRole("Admin") || User.IsInRole("SuperAdmin");
    }

    protected List<string> GetUserRoles()
    {
        return User.FindAll(ClaimTypes.Role).Select(c => c.Value).ToList();
            
    }
}