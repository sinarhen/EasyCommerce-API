using System.Text;
using System.Text.Json.Serialization;
using ECommerce.Config;
using Ecommerce.Data;
using ECommerce.Models.Entities;
using ECommerce.Services;
using Lib;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;


Env.LoadFile(Path.Combine(Directory.GetCurrentDirectory(), ".env"));


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddJsonOptions(x =>
    x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ProductDbContext>(options =>
{
    var connString = Secrets.DbConnectionString;
    options.UseNpgsql(connString);
});
builder.Services.AddIdentity<Customer, CustomerRole>(options =>
    {
        options.Password.RequireDigit = true;
        options.Password.RequiredLength = 8;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireUppercase = true;
        options.Password.RequireLowercase = false;
        options.User.RequireUniqueEmail = true;
        
    })
    .AddRoles<CustomerRole>()
    .AddEntityFrameworkStores<ProductDbContext>()
    .AddDefaultTokenProviders();


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = Secrets.JwtIssuer,
            ValidAudience = Secrets.JwtAudience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Secrets.JwtKey))
        };       
    });

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddSingleton<JwtService>(provider =>
{
    var jwtSecrets = new JwtSecrets(
        Issuer: Secrets.JwtIssuer,
        Key: Secrets.JwtKey,
        Audience: Secrets.JwtAudience
    );
    return new JwtService(jwtSecrets);
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


// app.UseHttpsRedirection();
app.UseAuthorization();
app.UseAuthentication();
app.MapControllers();


try
{
    // Seeding data to database if not exists 
    InitDb.Initialize(app);
}
catch (Exception ex)
{
    Console.WriteLine("Error happened while seeding data: ", ex.Message);
    throw;
}

app.Run();