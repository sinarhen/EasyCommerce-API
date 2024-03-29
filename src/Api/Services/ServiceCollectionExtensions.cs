﻿using ECommerce.Config;
using ECommerce.Data.Repositories.Admin;
using ECommerce.Data.Repositories.Billboard;
using ECommerce.Data.Repositories.Category;
using ECommerce.Data.Repositories.Collection;
using ECommerce.Data.Repositories.Customer;
using ECommerce.Data.Repositories.Product;
using ECommerce.Data.Repositories.Review;
using ECommerce.Data.Repositories.Seller;
using ECommerce.Data.Repositories.Store;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Services;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddJwtService(this IServiceCollection services)
    {
        var jwtSecrets = new JwtSecrets(
            Secrets.JwtIssuer,
            Secrets.JwtKey,
            Secrets.JwtAudience
        );
        services.AddSingleton(jwtSecrets);
        services.AddScoped<JwtService>();
        return services;
    }

    public static IServiceCollection EnableModelStateInvalidFilterSuppression(this IServiceCollection services)
    {
        services.Configure<ApiBehaviorOptions>(options => { options.SuppressModelStateInvalidFilter = true; });
        return services;
    }

    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IAdminRepository, AdminRepository>();
        services.AddScoped<IBillboardRepository, BillboardRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<ICollectionRepository, CollectionRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IStoreRepository, StoreRepository>();
        services.AddScoped<ICustomerRepository, CustomerRepository>();
        services.AddScoped<IReviewRepository, ReviewRepository>();
        services.AddScoped<ISellerRepository, SellerRepository>();
        return services;
    }

    public static IServiceCollection AddAuthorizationWithPolicies(this IServiceCollection services)
    {
        services.AddAuthorization(options =>
        {
            options.AddPolicy(Policies.AdminPolicy,
                policy => { policy.RequireRole(UserRoles.Admin, UserRoles.SuperAdmin); });

            options.AddPolicy(Policies.SellerPolicy,
                policy => { policy.RequireRole(UserRoles.Seller, UserRoles.Admin, UserRoles.SuperAdmin); });

            options.AddPolicy(Policies.SuperAdminPolicy, policy => { policy.RequireRole(UserRoles.SuperAdmin); });
        });
        return services;
    }
}