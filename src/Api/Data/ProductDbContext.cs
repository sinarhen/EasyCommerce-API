using ECommerce.Models.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Data;

public class ProductDbContext : IdentityDbContext<Customer, CustomerRole, string>
{
    public ProductDbContext(DbContextOptions<ProductDbContext> options) : base(options)
    {
        
    }
    
    public DbSet<Product> Products { get; set; }
    public DbSet<ProductStock> ProductStocks { get; set; }
    public DbSet<ProductCategory> ProductCategories { get; set; }
    public DbSet<ProductImage> ProductImages { get; set; }
    public DbSet<ProductMaterial> ProductMaterials { get; set; }
    public DbSet<Material> Materials { get; set; }

    public DbSet<Category> Categories { get; set; }
    
    public DbSet<Cart> Carts { get; set; }
    public DbSet<Order> Orders { get; set; }
    
    public DbSet<OrderDetail> OrderDetails { get; set; }
    public DbSet<Color> Colors { get; set; }
    public DbSet<Size> Sizes { get; set; }
    public DbSet<Review> Reviews { get; set; }
    public DbSet<Occasion> Occasions { get; set; }
    
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
    }

}