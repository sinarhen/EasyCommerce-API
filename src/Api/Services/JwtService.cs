using System.Collections;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Ecommerce.Entities;
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

    private const string UserNameClaim = "UserName";
    private const string RoleClaim = "Role"; 
    
    public JwtService(JwtSecrets jwtSecrets)
    {
        _jwtSecrets = jwtSecrets ?? throw new ArgumentNullException(nameof(jwtSecrets));
    }

    public string GenerateToken(string username, IEnumerable<string> roles)
    {

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSecrets.Key));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
     
        var claims = new List<Claim>
        {
            new Claim(UserNameClaim, username),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        };
        foreach (var role in roles)
        {
            if (role != null) claims.Add(new Claim(RoleClaim, role));
        }  

        var jwt = new JwtSecurityToken(
            _jwtSecrets.Issuer,
            _jwtSecrets.Audience,
            claims,
            expires: DateTime.Now.AddDays(30),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(jwt);
    }

    public ClaimsPrincipal ValidateToken(string token)
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
                ValidateAudience = false,
                ValidateLifetime = true,
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.ValidateToken(token, validations, out var _);
        }
        catch
        {
            return null;
        }
    }
}