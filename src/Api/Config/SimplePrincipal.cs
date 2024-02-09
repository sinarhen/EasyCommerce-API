using ECommerce.Models.DTOs.Auth;

namespace ECommerce.Config;

public class SimplePrincipal
{
    public IEnumerable<ClaimDto> Claims { get; set; }
}