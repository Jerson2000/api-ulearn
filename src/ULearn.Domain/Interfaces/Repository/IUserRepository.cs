using ULearn.Domain.Entities;

namespace ULearn.Domain.Interfaces.Repository;

public interface IUserRepository
{
    Task<IReadOnlyList<User>> GetAllAsync();
    Task<User?> GetByIdAsync(Guid id);
    Task<User?> GetByEmailAsync(string email);
    Task<Guid> CreateAsync(User item);
    Task UpdateAsync(User item);
    Task DeleteAsync(Guid id);
}
