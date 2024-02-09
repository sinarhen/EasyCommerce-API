using Lib;

namespace ECommerce.Config;

public class Secrets
{
    public static readonly string JwtIssuer;
    public static readonly string JwtKey;
    public static readonly string JwtAudience;
    public static readonly string DbConnectionString;
    public static readonly string AdminEmail;
    public static readonly string AdminPassword;
    public static readonly string AdminUsername;

    static Secrets()
    {
        JwtIssuer = Env.GetRequired("JWT_ISSUER");
        JwtKey = Env.GetRequired("JWT_KEY");
        JwtAudience = Env.GetRequired("JWT_AUDIENCE");
        DbConnectionString = Env.GetRequired("DB_CONNECTION_STRING");
        AdminEmail = Env.GetRequired("ADMIN_EMAIL");
        AdminPassword = Env.GetRequired("ADMIN_PASSWORD");
        AdminUsername = Env.GetRequired("ADMIN_USERNAME");
    }
}