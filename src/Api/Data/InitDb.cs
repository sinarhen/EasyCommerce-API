using Ecommerce.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Ecommerce.Data;

public class InitDb
{
    private readonly RoleManager<CustomerRole> _roleManager;
    private readonly UserManager<Customer> _userManager;
    private readonly SignInManager<Customer> _signInManager;
    private readonly IConfiguration _configuration;

    public InitDb(
        RoleManager<CustomerRole> roleManager,
        UserManager<Customer> userManager,
        SignInManager<Customer> signInManager,
        IConfiguration configuration
    )
    {
        _roleManager = roleManager;
        _userManager = userManager;
        _signInManager = signInManager;
        _configuration = configuration;
    }

    public void EnsureRolesAreCreated()
    {

        if (!_roleManager.RoleExistsAsync("Admin").Result)
        {
            var role = new CustomerRole();
            role.Name = "Admin";
            var roleResult = _roleManager.CreateAsync(role).Result;
            if (!roleResult.Succeeded)
            {
                throw new Exception("Failed to create admin role");
            }
        }

        if (!_roleManager.RoleExistsAsync("Customer").Result)
        {
            var role = new CustomerRole();
            role.Name = "Customer";
            var roleResult = _roleManager.CreateAsync(role).Result;
            if (!roleResult.Succeeded)
            {
                throw new Exception("Failed to create customer role");
            }
        }

    }

    public void EnsureAdminIsCreated()
    {
        var admin = _userManager.FindByEmailAsync("admin@admin.com").Result;
        if (admin == null)
        {
            admin = new Customer();
            admin.Email = _configuration.GetConnectionString("");
        }
    }
    
    public void SeedData()
    {
        EnsureRolesAreCreated();
        EnsureAdminIsCreated();
    }
    
}