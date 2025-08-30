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
        var cacheKey = CacheHelper.GenerateCacheKey<User>("all");
        var cacheDuration = TimeSpan.FromMinutes(1);
        return await _cacheService.GetOrSetAsync<List<User>?>(
            cacheKey,
            async () => await _dbContext.Users.AsNoTracking().ToListAsync(),
            cacheDuration
        ) ?? new();
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        var cacheKey = CacheHelper.GenerateCacheKey<User>($"email:{email}");
        var cacheDuration = TimeSpan.FromMinutes(1);
        return await _cacheService.GetOrSetAsync(
            cacheKey,
            async () => await _dbContext.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Email == email),
            cacheDuration
        );
    }
    public async Task<User?> GetByIdAsync(Guid id)
    {
        var cacheKey = CacheHelper.GenerateCacheKey<User>($"id:{id}");
        var cacheDuration = TimeSpan.FromMinutes(1);
        return await _cacheService.GetOrSetAsync(
            cacheKey,
            async () => await _dbContext.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id),
            cacheDuration
        );
    }
    public async Task UpdateAsync(User user)
    {
        _dbContext.Set<User>().Update(user);
        await _dbContext.SaveChangesAsync();
    }
}
