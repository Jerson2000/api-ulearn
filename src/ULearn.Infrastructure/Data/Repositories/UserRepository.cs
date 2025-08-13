using Microsoft.EntityFrameworkCore;
using ULearn.Domain.Entities;
using ULearn.Domain.Exceptions;
using ULearn.Domain.Interfaces.Repository;
using ULearn.Infrastructure.Data;

namespace ULearn.Infrastructure.Data.Repositories;

public class UserRepository(ULearnDbContext dbContext) : IUserRepository
{
    private readonly ULearnDbContext _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));

    public async Task<Guid> CreateAsync(User user)
    {
        await _dbContext.Users.AddAsync(user);
        await _dbContext.SaveChangesAsync();
        return user.Id;
    }

    public async Task DeleteAsync(Guid id)
    {
        var entity = await _dbContext.Users.FindAsync(id);
        if (entity is null)
            throw new NotFoundException($"User with ID '{id}' not found.");

        _dbContext.Users.Remove(entity);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<IReadOnlyList<User>> GetAllAsync()
        => await _dbContext.Users.AsNoTracking().ToListAsync();

    public async Task<User?> GetByEmailAsync(string email)
        => await _dbContext.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Email == email);

    public async Task<User?> GetByIdAsync(Guid id)
        => await _dbContext.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == id);

    public async Task UpdateAsync(User user)
    {
        var existing = await _dbContext.Users.FindAsync(user.Id);
        if (existing is null)
            throw new NotFoundException($"User with ID '{user.Id}' not found.");

        _dbContext.Entry(existing).CurrentValues.SetValues(user);
        await _dbContext.SaveChangesAsync();
    }
}
