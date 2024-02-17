namespace ECommerce.Config;

public static class UserRoles
{
    public const string Admin = "Admin";
    public const string Seller = "Seller";
    public const string Customer = "Customer";
    public const string SuperAdmin = "SuperAdmin";

    public static readonly Dictionary<string, int> RoleHierarchy = new()
    {
        { SuperAdmin, 4 },
        { Admin, 3 },
        { Seller, 2 },
        { Customer, 1 }
    };

    public static string GetHighestUserRole(IEnumerable<string> roles)
    {
        var highestRole = roles.MaxBy(r => RoleHierarchy[r ?? Customer]);
        return highestRole ?? Customer;
    }

    public static List<string> GetAllRoles()
    {
        return new List<string>
        {
            Customer,
            Seller,
            Admin,
            SuperAdmin
        };
    }
}