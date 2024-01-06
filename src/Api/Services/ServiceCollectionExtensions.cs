using ECommerce.Config;

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
}