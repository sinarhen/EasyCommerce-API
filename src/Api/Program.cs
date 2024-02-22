using System.Security.Claims;
using System.Text;
using System.Text.Json;
using ECommerce.Config;
using ECommerce.Data;
using ECommerce.Hubs;
using ECommerce.Middleware;
using ECommerce.Models.Entities;
using ECommerce.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(options => { options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase; });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ProductDbContext>(options =>
{
    var connString = Secrets.DbConnectionString;
    Console.WriteLine("Connection string: " + connString);
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

builder.Services.AddAuthorizationWithPolicies();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddJwtService();
builder.Services.AddRepositories();
builder.Services.AddScoped<ValidationService>();
builder.Services.EnableModelStateInvalidFilterSuppression();
builder.Services.AddSignalR();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<ExceptionMiddleware>();

app.MapHub<OrderHub>("/order");
app.MapControllers();


if (app.Environment.IsDevelopment())
{
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
}

app.Run();