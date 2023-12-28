using ECommerce.Config;
using ECommerce.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Data;

public static class InitDb
{ 
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
            var role = new CustomerRole
            {
                Name = UserRoles.Admin
            };
            var roleResult = await roleManager.CreateAsync(role);
            if (!roleResult.Succeeded)
            {
                throw new Exception("Failed to create admin role");
            }
        }

        if (!await roleManager.RoleExistsAsync(UserRoles.Customer))
        {
            var role = new CustomerRole
            {
                Name = UserRoles.Customer
            };
            var roleResult = await roleManager.CreateAsync(role);
            if (!roleResult.Succeeded)
            {
                throw new Exception("Failed to create customer role");
            }
        }
        
        if (!await roleManager.RoleExistsAsync(UserRoles.SuperAdmin))
        {
            var role = new CustomerRole
            {
                Name = UserRoles.SuperAdmin
            };
            var roleResult = await roleManager.CreateAsync(role);
            if (!roleResult.Succeeded)
            {
                throw new Exception("Failed to create SuperAdmin role");
            }
        }

    }

    private static async Task EnsureAdminIsCreated(
        UserManager<Customer> userManager
    )
    {
        var adminEmail = Secrets.AdminEmail;
        var adminName = Secrets.AdminUsername;
        var adminPassword = Secrets.AdminPassword;
        
        var admin = await userManager.FindByEmailAsync(adminEmail);
        if (admin == null)
        {
            
            admin = new Customer
            {
                Email = adminEmail,
                UserName = adminName
            };
            
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

    private static List<ProductStock> GetProductStocksForSeeding(Product product, IReadOnlyCollection<Color> colors, int price, IReadOnlyCollection<string> customSizes = null)
    {
        // Creating 10 product stocks for each size and color
        var list = new List<ProductStock>();
        // If customSizes is not null, create product stocks with custom sizes
        if (customSizes != null)
        {
            list.AddRange(from color in colors
            from customSize in customSizes
            select new ProductStock
            {
                Product = product,
                Color = color,
                CustomSize = customSize,
                Price = price,
                Stock = 10,
            });
        }
        else
        {
            list.AddRange(from color in colors
            from Size size in Enum.GetValues(typeof(Size))
            select new ProductStock
            {
                Product = product,
                Color = color,
                Size = size,
                Price = price,
                Stock = 10,
            });
        }
        return list;
    }
    
    private static ProductImage CreateProductImageEntity(Product product, Color color, List<string> imagesUrl)
    {
        return new ProductImage
        {
            Product = product,
            Color = color,
            ImagesUrl = imagesUrl,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
        };
    }
    private static Occasion CreateOccasionEntity(string name, string description)
    {
        return new Occasion
        {
            Name = name,
            Description = description,
            Id = Guid.NewGuid()
        };
    }
    private static Color CreateColorEntity(string name, string hexCode)
    {
        return new Color
        {
            Name = name,
            HexCode = hexCode,
            Id = Guid.NewGuid()
        };
    }
    private static Category CreateCategoryEntity(string name)
    {
        return new Category
        {
            Name = name,
            Id = Guid.NewGuid()
        };
    }
    
    private static Material CreateMaterialEntity(string name)
    {
        return new Material
        {
            Name = name,
            Id = Guid.NewGuid()
        };
    }
    private static ProductMaterial CreateProductMaterialEntity(Product product, Material material, double percentage)
    {
        return new ProductMaterial
        {
            Product = product,
            Material = material,
            Percentage = percentage,
        };
    }
    private static async Task SeedInitialProducts(ProductDbContext context)
    {
        // Colors
        var white = CreateColorEntity("White", "#FFFFFF");
        var black = CreateColorEntity("Black", "#000000");
        var red = CreateColorEntity("Red", "#FF0000");
        var green = CreateColorEntity("Green", "#008000");
        var blue = CreateColorEntity("Blue", "#0000FF");
        var yellow = CreateColorEntity("Yellow", "#FFFF00");
        var cyan = CreateColorEntity("Cyan", "#00FFFF");
        var silver = CreateColorEntity("Silver", "#C0C0C0");
        var purple = CreateColorEntity("Purple", "#800080");
        var orange = CreateColorEntity("Orange", "#FFA500");
        var violet = CreateColorEntity("Violet", "#EE82EE");
        var brown = CreateColorEntity("Brown", "#A52A2A");
        var khaki = CreateColorEntity("Khaki", "#F0E68C");
        
        
        context.Colors.AddRange(new []{white, black, red, green, blue, yellow, cyan, silver, purple, orange, violet, brown, khaki});
        
        // Occasions
        var casualOccasion = CreateOccasionEntity("Casual", "Outfits for casual occasions");
        
        var formalOccasion = CreateOccasionEntity("Formal", "Outfits for formal occasions");
        
        var sportyOccasion = CreateOccasionEntity("Sporty", "Outfits for sporty occasions");
        
        var businessOccasion = CreateOccasionEntity("Business", "Outfits for business occasions");
        
        var streetOccasion = CreateOccasionEntity("Street", "Outfits for street occasions");
        
        // Categories
        var shirtsCategory = CreateCategoryEntity("Shirts");
        var shoesCategory = CreateCategoryEntity("Shoes");
        var pantsCategory = CreateCategoryEntity("Pants");
        var accessoriesCategory = CreateCategoryEntity("Accessories");
        
        // Materials
        var cottonMaterial = CreateMaterialEntity("Cotton");
        var leatherMaterial = CreateMaterialEntity("Leather");
        var polyesterMaterial = CreateMaterialEntity("Polyester");
        var silkMaterial = CreateMaterialEntity("Silk");
        var woolMaterial = CreateMaterialEntity("Wool");
        var rubberMaterial = CreateMaterialEntity("Rubber");
        var suedeMaterial = CreateMaterialEntity("Suede");
        
        
        // Products with category "Shirt"
        var shirtsProducts = new List<Product>();
        
        // Shirts product one
        var shirtsProductOne = new Product
        {
            Id = Guid.NewGuid(),
            Category = shirtsCategory,
            Name = "Men's Casual Shirt",
            Description = "A comfortable and stylish shirt for everyday wear.",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow, 
            Season = Season.Summer,
            MainMaterial = cottonMaterial,
            CollectionYear = 2021,
            Gender = Gender.Unisex,
            Occasion = casualOccasion
        };
        shirtsProductOne.Images = new List<ProductImage>
        {
            CreateProductImageEntity(shirtsProductOne, white, new List<string>
            {
                "https://i.pinimg.com/564x/1b/7c/b6/1b7cb6fe341e990867f7f29d8fc44773.jpg",

            })
        };
        shirtsProductOne.Materials = new List<ProductMaterial>
        {
            CreateProductMaterialEntity(shirtsProductOne, cottonMaterial, 0.9),
            CreateProductMaterialEntity(shirtsProductOne, polyesterMaterial, 0.1),
        };
        shirtsProductOne.Stocks = GetProductStocksForSeeding(shirtsProductOne, colors: new []{ white, black}, 30);
        
        shirtsProducts.Add(shirtsProductOne);

        // Shirts product two
        var shirtsProductTwo = new Product
        {
            Id = Guid.NewGuid(),
            Category = shirtsCategory,
            Name = "Women's Blouse",
            Description = "Casual blouse for casual occasions.",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow, 
            Season = Season.Summer,
            MainMaterial = silkMaterial,
            CollectionYear = 2021,
            Gender = Gender.Female,
            Occasion = formalOccasion
        };
        shirtsProductTwo.Images = new List<ProductImage>
        {
            CreateProductImageEntity(shirtsProductTwo, white, new List<string>
            {
                "https://i.pinimg.com/564x/f5/58/9d/f5589d631ab3686d469ec93ac23ebc9f.jpg",

            }),
            CreateProductImageEntity(shirtsProductTwo, black, new List<string>
            {
                "https://i.pinimg.com/564x/82/2a/ac/822aac770bc03449bfb85a7d63e276d4.jpg",
            })
        };
        shirtsProductTwo.Materials = new List<ProductMaterial>
        {
            CreateProductMaterialEntity(shirtsProductTwo, silkMaterial, 0.9),
            CreateProductMaterialEntity(shirtsProductTwo, woolMaterial, 0.1),
        }; 
            
        shirtsProductTwo.Stocks = GetProductStocksForSeeding(shirtsProductTwo, colors: new []{black, white}, 40);
        
        // Shirts product three
        var shirtsProductThree = new Product
        {
            Id = Guid.NewGuid(),
            Category = shirtsCategory,
            Name = "Men's Striped Polo Shirt",
            Description = "Casual polo shirt with stripes for a sporty look.",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            Season = Season.Summer,
            MainMaterial = cottonMaterial,
            CollectionYear = 2021,
            Gender = Gender.Male,
            Occasion = casualOccasion,
        };
        shirtsProductThree.Images = new List<ProductImage>
        {
            CreateProductImageEntity(shirtsProductThree, black, new List<string>
            {
                "https://i.pinimg.com/564x/70/da/dd/70dadd5f402821dcae83e7be32e29ce7.jpg",
            })
        };
        shirtsProductThree.Materials = new List<ProductMaterial>
        {
            CreateProductMaterialEntity(shirtsProductThree, polyesterMaterial, 0.5),
            CreateProductMaterialEntity(shirtsProductThree, cottonMaterial, 0.5),
        };
        shirtsProductThree.Stocks = GetProductStocksForSeeding(shirtsProductThree, colors: new []{black}, 25);
        
        shirtsProducts.Add(shirtsProductTwo);
        
        
        // Products with category "Shoes"
        var shoesProducts = new List<Product>();
        
        // Shoes product one
        var shoesProductOne = new Product
        {
            Id = Guid.NewGuid(),
            Category = shoesCategory,
            Name = "Men's Running Shoes",
            Description = "Comfortable running shoes for active individuals.",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            Season = Season.DemiSeason,
            MainMaterial = polyesterMaterial,
            CollectionYear = 2022,
            Gender = Gender.Male,
            Occasion = sportyOccasion,
        };
    
        shoesProductOne.Images = new List<ProductImage>
        {
            CreateProductImageEntity(shoesProductOne, black, new List<string>
            {
                "https://m.media-amazon.com/images/W/MEDIAX_792452-T1/images/I/61ZA59Q2OIL._AC_SY395_.jpg",
                "https://m.media-amazon.com/images/W/MEDIAX_792452-T1/images/I/61LHvwnjTDL._AC_SY395_.jpg",
                "https://m.media-amazon.com/images/W/MEDIAX_792452-T1/images/I/616N2AANkvL._AC_SY395_.jpg",
                "https://m.media-amazon.com/images/W/MEDIAX_792452-T1/images/I/61kA6cVXMQL._AC_SY395_.jpg",
                "https://m.media-amazon.com/images/W/MEDIAX_792452-T1/images/I/61FL-ek2ySL._AC_SY395_.jpg",
                "https://m.media-amazon.com/images/W/MEDIAX_792452-T1/images/I/613ZJpbNhkS._AC_SX395_.jpg"
            }),
            CreateProductImageEntity(shoesProductOne, white, new List<string>
            {
                "https://m.media-amazon.com/images/W/MEDIAX_792452-T1/images/I/51+49v7ksXL._AC_SY395_.jpg",
                "https://m.media-amazon.com/images/W/MEDIAX_792452-T1/images/I/612UEFO7nlL._AC_SY395_.jpg",
                "https://m.media-amazon.com/images/W/MEDIAX_792452-T1/images/I/51XONIw08vL._AC_SY395_.jpg",
                "https://m.media-amazon.com/images/W/MEDIAX_792452-T1/images/I/61iPFTm-nzL._AC_SY395_.jpg",
                "https://m.media-amazon.com/images/W/MEDIAX_792452-T1/images/I/61d6rLD8voL._AC_SX395_.jpg",
            } )
            
        };

        shoesProductOne.Materials = new List<ProductMaterial>
        {
            CreateProductMaterialEntity(shoesProductOne, rubberMaterial, 0.4),
            CreateProductMaterialEntity(shoesProductOne, polyesterMaterial, 0.6),
        };

        shoesProductOne.Stocks = GetProductStocksForSeeding(shoesProductOne, colors: new []{black, white}, 80);

        shoesProducts.Add(shoesProductOne);
        
        // Shoes product two
        var shoesProductTwo = new Product
        {
            Id = Guid.NewGuid(),
            Category = shoesCategory,
            Name = "Unisex Casual Sneakers",
            Description = "Comfortable and stylish casual sneakers for both men and women.",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            Season = Season.DemiSeason,
            MainMaterial = cottonMaterial,
            CollectionYear = 2022,
            Gender = Gender.Unisex,
            Occasion = streetOccasion,
        };

        shoesProductTwo.Images = new List<ProductImage>
        {
            CreateProductImageEntity(shoesProductTwo, violet, new List<string>
            {
                "https://www.nike.org.ua/files/resized/products/85_1.700x800.png",
            }),
            
            CreateProductImageEntity(shoesProductTwo, white, new List<string>
            {
                "https://www.nike.org.ua/files/resized/products/84_1.700x800.png",
            }),
            CreateProductImageEntity(shoesProductTwo, black,  new List<string>
            {
                "https://www.nike.org.ua/files/resized/products/80_1.700x800.png",
            })
        };

        shoesProductTwo.Materials = new List<ProductMaterial>
        {
            CreateProductMaterialEntity(shirtsProductTwo, cottonMaterial, 0.8),
            CreateProductMaterialEntity(shirtsProductTwo, rubberMaterial, 0.2),
        };

        shoesProducts.Add(shoesProductTwo);

        // Shoes product three
        var shoesProductThree = new Product
        {
            Id = Guid.NewGuid(),
            Category = shoesCategory,
            Name = "Women's Casual Sneakers",
            Description = "Stylish and comfortable casual sneakers for women.",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            Season = Season.DemiSeason,
            Discount = 0.3,
            MainMaterial = cottonMaterial,
            CollectionYear = 2022,
            Gender = Gender.Female,
            Occasion = streetOccasion,
        };

        shoesProductThree.Images = new List<ProductImage>
        {
            CreateProductImageEntity(shirtsProductThree, black, new List<string>
            {
                "https://images.puma.com/image/upload/f_auto,q_auto,b_rgb:fafafa/global/397549/01/sv01/fnd/UKR/w/1000/h/1000/fmt/png",

            })
        };

        shoesProductThree.Materials = new List<ProductMaterial>
        {
            CreateProductMaterialEntity(shirtsProductThree, suedeMaterial, 0.8),
            CreateProductMaterialEntity(shirtsProductThree, rubberMaterial, 0.2),
        };

        shoesProductThree.Stocks = GetProductStocksForSeeding(shoesProductThree, colors: new []{black}, 140);
        
        shoesProducts.Add(shoesProductThree);

        // Products with category "Pants"
        var pantsProducts = new List<Product>();
        
        // Pants product one
        var pantsProductOne = new Product
        {
            Id = Guid.NewGuid(),
            Category = pantsCategory,
            Name = "Men's Casual Pants",
            Description = "Comfortable and stylish pants for everyday wear.",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            Season = Season.DemiSeason,
            MainMaterial = cottonMaterial,
            CollectionYear = 2022,
            Gender = Gender.Male,
            Occasion = casualOccasion,
            SizeChartImageUrl = "https://m.media-amazon.com/images/W/MEDIAX_792452-T1/images/I/61q0QLQ1EFL._AC_SX342_.jpg"
        };

        pantsProductOne.Images = new List<ProductImage>
        {
            CreateProductImageEntity(pantsProductOne, black,  new List<string>
            {
                "https://m.media-amazon.com/images/W/MEDIAX_792452-T1/images/I/81uCZWI6cUL._AC_SY445_.jpg",
                "https://m.media-amazon.com/images/W/MEDIAX_792452-T1/images/I/71UQKwNyieL._AC_SY445_.jpg",
                "https://m.media-amazon.com/images/W/MEDIAX_792452-T1/images/I/71obafsgPfL._AC_SY445_.jpg",
            }),
            CreateProductImageEntity(pantsProductOne, khaki,  new List<string>
            {
                "https://m.media-amazon.com/images/W/MEDIAX_792452-T1/images/I/81uoaWwCjuL._AC_SY445_.jpg",
                "https://m.media-amazon.com/images/W/MEDIAX_792452-T1/images/I/81j44iOBYCL._AC_SY445_.jpg",
                "https://m.media-amazon.com/images/W/MEDIAX_792452-T1/images/I/712wBPy9LkL._AC_SY445_.jpg",
            }),

        };

        pantsProductOne.Materials = new List<ProductMaterial>
        {
            CreateProductMaterialEntity(pantsProductOne, woolMaterial, 1)
            
        };

        pantsProductOne.Stocks = GetProductStocksForSeeding(pantsProductOne, colors: new []{ black, khaki}, 50);

        pantsProducts.Add(pantsProductOne);

        // Continue adding other products and categories...

        // Products with category "Accessories"
        var accessoriesProducts = new List<Product>();

        // Accessories product one
        var accessoriesProductOne = new Product
        {
            Id = Guid.NewGuid(),
            Category = accessoriesCategory,
            Name = "Leather Belt",
            Description = "Stylish leather belt for men and women.",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            Season = Season.All,
            MainMaterial = leatherMaterial,
            CollectionYear = 2022,
            Gender = Gender.Unisex,
            Occasion = businessOccasion,
        };

        accessoriesProductOne.Images = new List<ProductImage>
        {
            CreateProductImageEntity(accessoriesProductOne, brown, new List<string>
            {
                "https://m.media-amazon.com/images/W/MEDIAX_792452-T1/images/I/61u1DkV6u8L._AC_SX679_.jpg",
                "https://m.media-amazon.com/images/W/MEDIAX_792452-T1/images/I/61nfdjkLWwL._AC_SX385_.jpg",
                "https://m.media-amazon.com/images/W/MEDIAX_792452-T1/images/I/61uBafwoEGL._AC_SX342_.jpg",
            })
        };

        accessoriesProductOne.Materials = new List<ProductMaterial>
        {
            CreateProductMaterialEntity(accessoriesProductOne, leatherMaterial, 1)
        };

        accessoriesProductOne.Stocks = GetProductStocksForSeeding(accessoriesProductOne, colors: new []{brown} , 100, new [] { "32", "34", "36", "38"});
        
        accessoriesProducts.Add(accessoriesProductOne);
        await context.Products.AddRangeAsync(shirtsProducts.Concat(shoesProducts));
    }
    private static async Task EnsureInitialProductsAreCreated(ProductDbContext context)
    {

        if (!context.Products.Any())
        {
            // Add 10 products with new Color objects
            await SeedInitialProducts(context);        
        }

        // Save changes to persist the new products and their associated colors
        await context.SaveChangesAsync();
    }

    private static async void SeedData(ProductDbContext context, UserManager<Customer> userManager, RoleManager<CustomerRole> roleManager)
    {
        await context.Database.MigrateAsync();
        
        await EnsureRolesAreCreated(roleManager);
        await EnsureAdminIsCreated(userManager);
        await EnsureInitialProductsAreCreated(context: context);
        
        await context.SaveChangesAsync();
    }
    
}