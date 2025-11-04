using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using ULearn.Domain.Entities;
using ULearn.Domain.Interfaces.Repository;
using ULearn.Domain.Interfaces.Services;
using ULearn.Infrastructure.Utils;

namespace ULearn.Infrastructure.Data.Repositories;

public class UserRepository(ULearnDbContext dbContext, ICacheService cacheService) : IUserRepository
{
    private readonly ULearnDbContext _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    private readonly ICacheService _cacheService = cacheService;

    public async Task<Guid> CreateAsync(User user)
    {
        await _dbContext.Users.AddAsync(user);
        await _dbContext.SaveChangesAsync();
        return user.Id;
    }

    public async Task DeleteAsync(Guid id)
    {
        _dbContext.Users.Remove(new User { Id = id });
        await _dbContext.SaveChangesAsync();
    }

    public async Task<IReadOnlyList<User>> GetAllAsync()
    {
        return await _dbContext.Users.AsNoTracking().ToListAsync();
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _dbContext.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Email == email);
    }
    public async Task<User?> GetByIdAsync(Guid id)
    {
        return await _dbContext.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id);
    }
    public async Task UpdateAsync(User user)
    {
        _dbContext.Set<User>().Update(user);
        await _dbContext.SaveChangesAsync();
    }
}
