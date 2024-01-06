using ECommerce.Models.DTOs;

namespace ECommerce.Config;

public class SimplePrincipal
{
    public IEnumerable<ClaimDto> Claims { get; set; }
}