
using System.IdentityModel.Tokens.Jwt;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ULearn.Domain.Entities;
using ULearn.Domain.Interfaces.Repositories;
using ULearn.Domain.Interfaces.Services;
using ULearn.Infrastructure.Settings;

namespace ULearn.Infrastructure.Services;

public class TokenService(IOptions<JwtSettings> options, ITokenRepository tokenRepository) : ITokenService
{
    private readonly JwtSettings _jwtSettings = options.Value;
    private readonly ITokenRepository _tokenRepository = tokenRepository;

    public async Task<(string accessToken, string refreshToken)> GenerateJWTToken(User user, DateTime? refreshTokenValidity = null)
    {
        var access = CreateJWTClaim(user);
        var refresh = GenerateRandomString();
        await SaveOrUpdateTokenAsync(access, refresh, user, true, refreshTokenValidity);
        return (access, refresh);
    }


    public async Task<(string accessToken, string refreshToken)> GenerateJWTRefreshToken(string refreshToken, string expiredAccessToken)
    {
        Console.WriteLine($"Refresh = {refreshToken}");
        var hasToken = await _tokenRepository.GetTokenByRefreshAsync(refreshToken);
        if (hasToken == null || !hasToken.IsValid || hasToken.Access != expiredAccessToken)
        {
            throw new ArgumentException("Unauthorized");
        }
        var user = new User { Id = hasToken.UserId };
        var access = CreateJWTClaim(user);
        var refresh = GenerateRandomString();

        await SaveOrUpdateTokenAsync(access, refresh, user, false, null, hasToken);
        return (access, refresh);
    }

    private string CreateJWTClaim(User user)
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

    private string GenerateRandomString()
    {
        byte[] randomBytes = RandomNumberGenerator.GetBytes(32);
        string random = Convert.ToBase64String(randomBytes);
        return random;
    }

    private async Task SaveOrUpdateTokenAsync(string access, string refresh, User user, bool isNewLogin, DateTime? refreshTokenValidity = null, Token? token = null)
    {
        var validUntil = refreshTokenValidity ?? DateTime.Now.AddDays(7);
        var existingToken = token ?? await _tokenRepository.GetTokenByUserAsync(user.Id);

        if (existingToken != null)
        {
            existingToken.Access = access;
            existingToken.Refresh = refresh;
            existingToken.ValidUntil = isNewLogin ? validUntil : existingToken.ValidUntil;

            await _tokenRepository.UpdateToken(existingToken);
        }
        else
        {
            var newToken = new Token
            {
                Access = access,
                Refresh = refresh,
                ValidUntil = validUntil,
                UserId = user.Id
            };

            await _tokenRepository.SaveToken(newToken);
        }
    }

}