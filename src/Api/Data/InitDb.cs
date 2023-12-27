using ECommerce.Config;
using ECommerce.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Data;

public static class InitDb
{

    private static readonly Color Red = new Color
    {
        Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
        Name = "Red",
        HexCode = "#FF0000"
    };
    private static readonly Color Green = new Color
    {
        Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
        Name = "Green",
        HexCode = "#008000"
    };
    private static readonly Color Pink = new Color
    {
        Id = Guid.Parse("77777777-7777-7777-7777-777777777777"),
        Name = "Pink",
        HexCode = "#FFC0CB"
    };
    private static readonly Color Gray = new Color
    {
        Id = Guid.Parse("88888888-8888-8888-8888-888888888888"),
        Name = "Gray",
        HexCode = "#808080"
    };
    private static readonly Color Denim = new Color
    {
        Id = Guid.Parse("99999999-9999-9999-9999-999999999999"),
        Name = "Denim",
        HexCode = "#1560BD"
    };
    
    private static readonly Color Yellow = new Color
    {
        Id = Guid.Parse("33333333-3333-3333-3333-333333333333"),
        Name = "Yellow",
        HexCode = "#FFFF00"
    };
    private static readonly Color Purple = new Color
    {
        Id = Guid.Parse("44444444-4444-4444-4444-444444444444"),
        Name = "Purple",
        HexCode = "#800080"
    };
    private static readonly Color Orange = new Color
    {
        Id = Guid.Parse("55555555-5555-5555-5555-555555555555"),
        Name = "Orange",
        HexCode = "#FFA500"
    };
    private static readonly Color Brown = new Color
    {
        Id = Guid.Parse("66666666-6666-6666-6666-666666666666"),
        Name = "Brown",
        HexCode = "#A52A2A"
    };
    public static void Initialize(WebApplication app)
    {
        var scope = app.Services.CreateScope();
        var services = scope.ServiceProvider;
        var context = services.GetRequiredService<ProductDbContext>();
        var userManager = services.GetRequiredService<UserManager<Customer>>();
        var roleManager = services.GetRequiredService<RoleManager<CustomerRole>>();
        
        SeedData(
            context: context, 
            userManager: userManager, 
            roleManager: roleManager
        );   
    }

    private static async Task EnsureRolesAreCreated(RoleManager<CustomerRole> roleManager)
    {

        if (!await roleManager.RoleExistsAsync(UserRoles.Admin))
        {
            var role = new CustomerRole();
            role.Name = UserRoles.Admin;
            var roleResult = await roleManager.CreateAsync(role);
            if (!roleResult.Succeeded)
            {
                throw new Exception("Failed to create admin role");
            }
        }

        if (!await roleManager.RoleExistsAsync(UserRoles.Customer))
        {
            var role = new CustomerRole();
            role.Name = UserRoles.Customer;
            var roleResult = await roleManager.CreateAsync(role);
            if (!roleResult.Succeeded)
            {
                throw new Exception("Failed to create customer role");
            }
        }
        
        if (!await roleManager.RoleExistsAsync(UserRoles.SuperAdmin))
        {
            var role = new CustomerRole();
            role.Name = UserRoles.SuperAdmin;
            var roleResult = await roleManager.CreateAsync(role);
            if (!roleResult.Succeeded)
            {
                throw new Exception("Failed to create SuperAdmin role");
            }
        }

    }

    private static async Task EnsureAdminIsCreated(
        UserManager<Customer> userManager, 
        string adminName, 
        string adminEmail, 
        string adminPassword
    )
    {
        var admin = await userManager.FindByEmailAsync(adminEmail);
        if (admin == null)
        {
            
            admin = new Customer();
            admin.Email = adminEmail;
            admin.UserName = adminName;

            var adminResult = await userManager.CreateAsync(admin, adminPassword);
            
            if (!adminResult.Succeeded)
            {
                throw new Exception("Failed to create admin user");
            }
            
        }
        if (await userManager.IsInRoleAsync(admin, UserRoles.SuperAdmin))
        {
            return;
        }
        var roleResult = await userManager.AddToRoleAsync(admin, UserRoles.SuperAdmin);
        if (!roleResult.Succeeded)
        {
            throw new Exception("Failed to add admin user to roles");
        }
    }

    private static List<ProductColorQuantity> GetProductColorQuantityForSeeding(Product product)
    {

        return new List<ProductColorQuantity>
        {
            new ProductColorQuantity
            {
                Product = product,
                Color = Black,
                Quantity = 100
            },
            new ProductColorQuantity
            {
                Product = product,
                Color = Red,
                Quantity = 100
            },
            new ProductColorQuantity
            {
                Product = product,
                Color = Green,
                Quantity = 100
            },
            new ProductColorQuantity
            {
                Product = product,
                Color = Yellow,
                Quantity = 100
            },
            new ProductColorQuantity
            {
                Product = product,
                Color = Purple,
                Quantity = 100
            },
            new ProductColorQuantity
            {
                Product = product,
                Color = Orange,
                Quantity = 100
            },
            new ProductColorQuantity
            {
                Product = product,
                Color = Brown,
                Quantity = 100
            },
            new ProductColorQuantity
            {
                Product = product,
                Color = Pink,
                Quantity = 100
            },
            new ProductColorQuantity
            {
                Product = product,
                Color = Gray,
                Quantity = 100
            },
            new ProductColorQuantity
            {
                Product = product,
                Color = Denim,
                Quantity = 100
            },
        };    
    }

private static async Task EnsureInitialProductsAreCreated(ProductDbContext context)
{
    // Ensure the "Shirts" category exists or create it
    var shirtsCategory = await context.Categories.FirstOrDefaultAsync(c => c.Name == "Shirts");
    if (shirtsCategory == null)
    {
        shirtsCategory = new Category { Name = "Shirts", Id = Guid.NewGuid() };
        await context.Categories.AddAsync(shirtsCategory);
    }
    
    if (!context.Products.Any())
    {
        // Add 10 products with new Color objects
        
        var products = new List<Product>
        {
            new Product
            {
                Id = Guid.Parse("88febf1c-f5f3-4df8-98bb-09e5fee18195"),
                CategoryId = shirtsCategory.Id,
                Name = "Men's Casual Shirt",
                Description = "A comfortable and stylish shirt for everyday wear.",
                Price = (decimal)29.99,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                ImageUrl = "https://example.com/mens-shirt.jpg",
            },
            new Product
            {
                Id = Guid.Parse("a0e3b6a0-0b0a-4b0a-9b0a-0b0a0b0a0b0a"),
                CategoryId = shirtsCategory.Id,
                Name = "Women's Formal Blouse",
                Description = "Elegant blouse for formal occasions.",
                Price = (decimal)39.99,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                ImageUrl = "https://example.com/womens-blouse.jpg",
            },
            new Product
            {
                Id = Guid.Parse("b0b0b0b0-b0b0-b0b0-b0b0-b0b0b0b0b0b0"),
                CategoryId = shirtsCategory.Id,
                Name = "Men's Striped Polo Shirt",
                Description = "Casual polo shirt with stripes for a sporty look.",
                Price = (decimal)24.99,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                ImageUrl = "https://example.com/mens-polo.jpg",
            },
            new Product
            {
                Id = Guid.Parse("c0c0c0c0-c0c0-c0c0-c0c0-c0c0c0c0c0c0"),
                CategoryId = shirtsCategory.Id,
                Name = "Women's Denim Shirt",
                Description = "Denim shirt for a trendy and casual appearance.",
                Price = (decimal)44.99,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                ImageUrl = "https://example.com/womens-denim.jpg",
            },
            new Product
            {
                Id = Guid.Parse("d0d0d0d0-d0d0-d0d0-d0d0-d0d0d0d0d0d0"),
                CategoryId = shirtsCategory.Id,
                Name = "Men's Oxford Dress Shirt",
                Description = "Classic dress shirt for a polished and sophisticated look.",
                Price = (decimal)54.99,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                ImageUrl = "https://example.com/mens-dress-shirt.jpg",
            },
            new Product
            {
                Id = Guid.Parse("e0e0e0e0-e0e0-e0e0-e0e0-e0e0e0e0e0e0"),
                CategoryId = shirtsCategory.Id,
                Name = "Women's Floral Print Blouse",
                Description = "Blouse with a floral print for a stylish and feminine touch.",
                Price = (decimal)49.99,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                ImageUrl = "https://example.com/womens-floral-blouse.jpg",
            },
            new Product
            {
                Id = Guid.Parse("f0f0f0f0-f0f0-f0f0-f0f0-f0f0f0f0f0f0"),
                CategoryId = shirtsCategory.Id,
                Name = "Men's Plaid Flannel Shirt",
                Description = "Warm and cozy flannel shirt with a plaid pattern.",
                Price = (decimal)39.99,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                ImageUrl = "https://example.com/mens-flannel-shirt.jpg",
            },
            new Product
            {
                Id = Guid.Parse("a1a1a1a1-a1a1-a1a1-a1a1-a1a1a1a1a1a1"),
                CategoryId = shirtsCategory.Id,
                Name = "Women's Striped Boat Neck Top",
                Description = "Casual top with a striped pattern and boat neck design.",
                Price = (decimal)34.99,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                ImageUrl = "https://example.com/womens-striped-top.jpg",
            },
            new Product
            {
                Id = Guid.Parse("b1b1b1b1-b1b1-b1b1-b1b1-b1b1b1b1b1b1"),
                CategoryId = shirtsCategory.Id,
                Name = "Men's V-Neck T-Shirt",
                Description = "Basic V-neck T-shirt for a relaxed and casual style.",
                Price = (decimal)19.99,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                ImageUrl = "https://example.com/mens-vneck-tshirt.jpg",
            },
            new Product
            {
                Id = Guid.Parse("c1c1c1c1-c1c1-c1c1-c1c1-c1c1c1c1c1c1"),
                CategoryId = shirtsCategory.Id,
                Name = "Women's Linen Button-Up Shirt",
                Description = "Light and breathable linen shirt for a comfortable feel.",
                Price = (decimal)49.99,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                ImageUrl = "https://example.com/womens-linen-shirt.jpg",
            },
        };
        foreach (var product in products)
        {
            product.ColorQuantities = GetProductColorQuantityForSeeding(product);
        }
        await context.Products.AddRangeAsync(products);
    }

    // Save changes to persist the new products and their associated colors
    await context.SaveChangesAsync();
}

    private static async void SeedData(ProductDbContext context, UserManager<Customer> userManager, RoleManager<CustomerRole> roleManager)
    {
        await context.Database.MigrateAsync();
        await EnsureRolesAreCreated(roleManager);
        
        string adminName = Secrets.AdminUsername;
        string adminEmail = Secrets.AdminEmail;
        string adminPassword = Secrets.AdminPassword;
        await EnsureAdminIsCreated(userManager, adminName, adminEmail, adminPassword);

        await EnsureInitialProductsAreCreated(context: context);
        
        await context.SaveChangesAsync();
    }
    
}