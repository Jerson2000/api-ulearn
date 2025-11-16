
using System.Linq.Expressions;
using ULearn.Domain.ValueObjects;

namespace ULearn.Domain.Interfaces.Repositories;

public interface IRepository<T> where T : class
{
    // READ
    Task<T?> GetByIdAsync(Guid id, bool asNoTracking = true);
    Task<List<T>> ListAsync(bool asNoTracking = true);
    Task<List<T>> ListAsync(Expression<Func<T, bool>> predicate, bool asNoTracking = true);
    Task<PagedResult<T>> PagedAsync(int page, int pageSize, bool asNoTracking = true);

    // WRITE
    Task AddAsync(T entity);
    Task AddRangeAsync(IEnumerable<T> entities);
    void Update(T entity);
    void UpdateRange(IEnumerable<T> entities);
    void Delete(T entity);
    void DeleteRange(IEnumerable<T> entities);

    // Persistence
    Task<int> SaveChangesAsync();
}