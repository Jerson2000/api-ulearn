using ULearn.Domain.Entities;

namespace ULearn.Domain.Interfaces.Services;

public interface ITokenService
{
    Task<(string accessToken, string refreshToken)> GenerateJWTToken(User user, DateTime? tokenValidity = null);
    Task<(string accessToken, string refreshToken)> GenerateJWTRefreshToken(string refreshToken, string expiredAccessToken);
}