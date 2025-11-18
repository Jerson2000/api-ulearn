

using Microsoft.EntityFrameworkCore;
using ULearn.Domain.Entities;
using ULearn.Domain.Interfaces.Repositories;
using ULearn.Infrastructure.Data;

namespace ULearn.Infrastructure.Repositories;


public class TokenRepository(ULearnDbContext dbContext) : ITokenRepository
{
    private readonly ULearnDbContext _dbContext = dbContext;
    public async Task DeleteToken(Guid id)
    {
        _dbContext.Tokens.Remove(new Token { Id = id });
        await _dbContext.SaveChangesAsync();
    }

    public async Task<Token?> GetTokenByIdAsync(Guid id)
    {
        return await _dbContext.Tokens.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<Token?> GetTokenByUserAsync(Guid userId)
    {
        return await _dbContext.Tokens.AsNoTracking().FirstOrDefaultAsync(x => x.UserId == userId);
    }

    public async Task<Token?> GetTokenByRefreshAsync(string refresh)
    {
        return await _dbContext.Tokens.AsNoTracking().FirstOrDefaultAsync(x => x.Refresh == refresh);
    }

    public async Task<IReadOnlyList<Token>> GetAllTokenAsync()
    {
        return await _dbContext.Tokens.AsNoTracking().ToListAsync();
    }

    public async Task<Guid> SaveToken(Token token)
    {
        await _dbContext.Tokens.AddAsync(token);
        await _dbContext.SaveChangesAsync();
        return token.Id;
    }

    public async Task UpdateToken(Token token)
    {
        _dbContext.Set<Token>().Update(token);
        await _dbContext.SaveChangesAsync();
    }
}