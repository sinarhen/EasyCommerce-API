using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ECommerce.Config;
using Microsoft.IdentityModel.Tokens;
namespace ECommerce.Services;

public record JwtSecrets(
    string Issuer,
    string Key,
    string Audience
);

public class JwtService
{
    private readonly JwtSecrets _jwtSecrets;

    
    public JwtService(JwtSecrets jwtSecrets)
    {
        _jwtSecrets = jwtSecrets ?? throw new ArgumentNullException(nameof(jwtSecrets));
    }

    public JwtSecurityToken GenerateToken(string username, IEnumerable<string> roles)
    {

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSecrets.Key));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
     
        var claims = new List<Claim>
        {
            new Claim(CustomClaimTypes.Username, username),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        };
        foreach (var role in roles)
        {
            if (role != null) claims.Add(new Claim(CustomClaimTypes.Role, role));
        }  

        var jwt = new JwtSecurityToken(
            _jwtSecrets.Issuer,
            _jwtSecrets.Audience,
            claims,
            expires: DateTime.Now.AddDays(30),
            signingCredentials: credentials
        );

        return jwt;
    }
    
    public string WriteToken(JwtSecurityToken token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        return tokenHandler.WriteToken(token);
    }

    public SimplePrincipal ValidateToken(string token)
    {
        try
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSecrets.Key));

            var validations = new TokenValidationParameters
            {
                ValidIssuer = _jwtSecrets.Issuer,
                IssuerSigningKey = securityKey,
                ValidateIssuer = true,
                ValidateIssuerSigningKey = true,
                ValidateAudience = true,
                ValidateLifetime = true,
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, validations, out var _);

            var simplePrincipal = new SimplePrincipal
            {
                Claims = principal.Claims.Select(c => new ClaimDto { Type = c.Type, Value = c.Value })
            };

            return simplePrincipal;
        }
        catch (Exception err)
        {
            Console.WriteLine(err);
            return null;
        }
    }
}