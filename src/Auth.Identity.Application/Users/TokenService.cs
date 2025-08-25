using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Auth.Identity.Domain.Users;
using Microsoft.IdentityModel.Tokens;

namespace Auth.Identity.Application.Users;

public class TokenService
{
    private readonly string _jwtSecret;
    private readonly int _jwtLifespanMinutes;

    public TokenService(string jwtSecret, int jwtLifespanMinutes = 60)
    {
        _jwtSecret = jwtSecret;
        _jwtLifespanMinutes = jwtLifespanMinutes;
    }

    public string GenerateToken(User user)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.UniqueName, user.Name),
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSecret));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: "your-app",
            audience: "your-app",
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_jwtLifespanMinutes),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}