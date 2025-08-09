using ULearn.Domain.Entities;

namespace ULearn.Domain.Interfaces.Services;

public interface ITokenService
{
    string GenerateToken(User user);
    string GenerateRefreshToken(User user);
}