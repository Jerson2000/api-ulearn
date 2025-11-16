

using ULearn.Domain.Entities;

namespace ULearn.Domain.Interfaces.Repositories;

public interface ITokenRepository
{
    Task<Token?> GetTokenByIdAsync(Guid id);
    Task<Token?> GetTokenByUserAsync(Guid userId);
    Task<Token?> GetTokenByRefreshAsync(string refresh);
    Task<IReadOnlyList<Token>> GetAllTokenAsync();
    Task<Guid> SaveToken(Token token);
    Task UpdateToken(Token token);
    Task DeleteToken(Guid id);
}