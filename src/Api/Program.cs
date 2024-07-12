using System.Security.Claims;
using System.Text;
using System.Text.Json;
using ECommerce.Config;
using ECommerce.Data;
using ECommerce.Hubs;
using ECommerce.Middleware;
using ECommerce.Models.Entities;
using ECommerce.Services;
using Lib;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;


Env.LoadFile(".env.dev");

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(options => { options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase; });

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

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder =>
        {
            builder
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
        });
});
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
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo {Title = "ECommerce", Version = "v1"});
});

var app = builder.Build();



// Configure the HTTP request pipeline.


app.UseHttpsRedirection();
app.UseHsts();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}



app.UseAuthentication();
app.UseAuthorization();
app.UseCors("AllowAllOrigins");

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