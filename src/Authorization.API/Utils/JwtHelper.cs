using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Authorization.API.Utils;

public static class JwtHelper
{
   public static string GenerateToken(this WebApplication app, string username, string[] roles)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, username),
            new Claim(ClaimTypes.NameIdentifier, username)
        };

        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, "Admin"));
        }

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
            app.Configuration["Jwt:Key"] ?? "JFEYYFGEVHSgcVFHBvfhc6zGb2zG4eJf"));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            issuer: app.Configuration["Jwt:Issuer"] ?? "AuthorizationAPI",
            audience: app.Configuration["Jwt:Audience"] ?? "AuthorizationAPIClient",
            claims: claims,
            expires: DateTime.Now.AddHours(1),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}