using ECommerce.Models.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Data;

public class ProductDbContext : IdentityDbContext<User, UserRole, string>
{
    public ProductDbContext(DbContextOptions<ProductDbContext> options) : base(options)
    {
    }

    public DbSet<Store> Stores { get; set; }
    public DbSet<Collection> Collections { get; set; }
    public DbSet<Billboard> Billboards { get; set; }
    public DbSet<BillboardFilter> BillboardFilters { get; set; }
    public DbSet<BannedStore> BannedStores { get; set; }
    public DbSet<BannedUser> BannedUsers { get; set; }
    public DbSet<SellerUpgradeRequest> SellerUpgradeRequests { get; set; }
    public DbSet<SellerInfo> Sellers { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<ProductStock> ProductStocks { get; set; }
    public DbSet<ProductCategory> ProductCategories { get; set; }
    public DbSet<ProductImage> ProductImages { get; set; }
    public DbSet<ProductMaterial> ProductMaterials { get; set; }
    public DbSet<Material> Materials { get; set; }

    public DbSet<Category> Categories { get; set; }
    public DbSet<CategorySize> CategorySizes { get; set; }

    public DbSet<Cart> Carts { get; set; }
    public DbSet<CartProduct> CartProducts { get; set; }
    public DbSet<Order> Orders { get; set; }

    public DbSet<OrderItem> OrderDetails { get; set; }
    public DbSet<Color> Colors { get; set; }
    public DbSet<Size> Sizes { get; set; }
    public DbSet<Review> Reviews { get; set; }
    public DbSet<Occasion> Occasions { get; set; }

    public override int SaveChanges()
    {
        UpdateTimestamps();
        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        UpdateTimestamps();
        return base.SaveChangesAsync(cancellationToken);
    }


    private void UpdateTimestamps()
    {
        var entries = ChangeTracker
            .Entries()
            .Where(e => (e.Entity is BaseEntity || e.Entity is User) && (
                e.State == EntityState.Added
                || e.State == EntityState.Modified));

        foreach (var entityEntry in entries)
            if (entityEntry.Entity is BaseEntity baseEntity)
            {
                baseEntity.UpdatedAt = DateTime.UtcNow;

                if (entityEntry.State == EntityState.Added) baseEntity.CreatedAt = DateTime.UtcNow;
            }
            else if (entityEntry.Entity is User user)
            {
                user.UpdatedAt = DateTime.UtcNow;

                if (entityEntry.State == EntityState.Added) user.CreatedAt = DateTime.UtcNow;
            }
    }
}