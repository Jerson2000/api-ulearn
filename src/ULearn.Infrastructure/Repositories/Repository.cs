


using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using ULearn.Domain.Interfaces.Repositories;
using ULearn.Domain.ValueObjects;
using ULearn.Infrastructure.Data;

namespace ULearn.Infrastructure.Repositories;

public class Repository<T> : IRepository<T> where T : class
{
    protected readonly ULearnDbContext _db;
    private readonly DbSet<T> _set;

    public Repository(ULearnDbContext db)
    {
        _db = db;
        _set = db.Set<T>();
    }

    public async Task<T?> GetByIdAsync(Guid id, bool asNoTracking = true)
    {
        var query = asNoTracking ? _set.AsNoTracking() : _set;
        return await query.FirstOrDefaultAsync(e => EF.Property<Guid>(e, "Id") == id);
        // NOTE: All entities must have a property named "Id" (or adjust the key name)
    }

    public async Task<List<T>> ListAsync(bool asNoTracking = true)
        => await ApplyTracking(_set, asNoTracking).ToListAsync();

    public async Task<List<T>> ListAsync(Expression<Func<T, bool>> predicate, bool asNoTracking = true)
        => await ApplyTracking(_set, asNoTracking).Where(predicate).ToListAsync();

    public async Task<PagedResult<T>> PagedAsync(int page, int pageSize, bool asNoTracking = true)
    {
        var query = ApplyTracking(_set, asNoTracking);
        var total = await query.CountAsync();
        var items = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
        return new PagedResult<T>(items, total, page, pageSize);
    }

    public async Task AddAsync(T entity) => await _set.AddAsync(entity);
    public async Task AddRangeAsync(IEnumerable<T> entities) => await _set.AddRangeAsync(entities);
    public void Update(T entity) => _set.Update(entity);
    public void UpdateRange(IEnumerable<T> entities) => _set.UpdateRange(entities);
    public void Delete(T entity) => _set.Remove(entity);
    public void DeleteRange(IEnumerable<T> entities) => _set.RemoveRange(entities);

    public async Task<int> SaveChangesAsync() => await _db.SaveChangesAsync();

    private static IQueryable<T> ApplyTracking(DbSet<T> set, bool asNoTracking)
        => asNoTracking ? set.AsNoTracking() : set;
}