
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ULearn.Domain.Entities;
using ULearn.Domain.Interfaces.Services;
using ULearn.Infrastructure.Data;
using ULearn.Infrastructure.Settings;

namespace ULearn.Infrastructure.Services;

public class TokenService(IOptions<JwtSettings> options, ULearnDbContext dbContext) : ITokenService
{
    private readonly JwtSettings _jwtSettings = options.Value;
    private readonly ULearnDbContext _dbContext = dbContext;

    public string GenerateRefreshToken(User user)
    {
        byte[] randomBytes = RandomNumberGenerator.GetBytes(32);
        string refreshToken = Convert.ToBase64String(randomBytes);

        return refreshToken;
    }

    public string GenerateToken(User user)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role.ToString())
        };

        var jwt = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_jwtSettings.AccessTokenExpiryMinutes),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(jwt);
    }
}