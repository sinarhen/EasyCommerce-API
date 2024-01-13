using System.Security.Claims;
using System.Text;
using ECommerce.Config;
using ECommerce.Data;
using ECommerce.Data.Repositories.Category;
using ECommerce.Data.Repositories.Collection;
using ECommerce.Data.Repositories.Product;
using ECommerce.Data.Repositories.Store;
using ECommerce.Models.Entities;
using ECommerce.Services;
using Lib;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;




var builder = WebApplication.CreateBuilder(args);

var envFile = builder.Environment.IsDevelopment() ? ".env.dev" : ".env";
Env.LoadFile(Path.Combine(Directory.GetCurrentDirectory(), envFile));


builder.Services.AddControllers().AddJsonOptions(
    x => {
        // x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve

    }
);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ProductDbContext>(options =>
{
    var connString = Secrets.DbConnectionString;
    options.UseNpgsql(connString);
});
builder.Services.AddIdentity<User, UserRole>(options =>
    {
        options.Password.RequireDigit = true;
        options.Password.RequiredLength = 8;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireUppercase = true;
        options.Password.RequireLowercase = false;
        options.User.RequireUniqueEmail = true;
        
    })
    .AddRoles<UserRole>()
    .AddEntityFrameworkStores<ProductDbContext>()
    .AddDefaultTokenProviders();


builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultForbidScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            RoleClaimType = ClaimTypes.Role,
            NameClaimType = CustomClaimTypes.Username,
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = Secrets.JwtIssuer,
            ValidAudience = Secrets.JwtAudience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Secrets.JwtKey))
        };       
    });

builder.Services.AddAuthorization(options => 
{
    options.AddPolicy(Policies.AdminPolicy, policy => 
    {
        policy.RequireRole(UserRoles.Admin, UserRoles.SuperAdmin);
    });

    options.AddPolicy(Policies.SellerPolicy, policy => 
    {
        policy.RequireRole(UserRoles.Seller, UserRoles.Admin, UserRoles.SuperAdmin);
    });

    options.AddPolicy(Policies.SuperAdminPolicy, policy => 
    {
        policy.RequireRole(UserRoles.SuperAdmin);
    });
});

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddJwtService();

builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IStoreRepository, StoreRepository>();
builder.Services.AddScoped<ICollectionRepository, CollectionRepository>();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


// app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();


try
{
    // Seeding data to database if not exists 
    await InitDb.InitializeAsync(app);
}
catch (Exception ex)
{
    Console.WriteLine("Error happened while seeding data: ", ex.Message);
    throw;
}

app.Run();