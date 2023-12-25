using Microsoft.AspNetCore.Identity;

namespace Ecommerce.Entities;

public class Customer : IdentityUser
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Address { get; set; }
    public string City { get; set; }
    public string Country { get; set; }
    public string PostalCode { get; set; }
    public ICollection<Order> Orders { get; set; } = new List<Order>();
}