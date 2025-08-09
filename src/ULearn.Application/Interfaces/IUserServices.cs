
using ULearn.Application.DTOs;

namespace ULearn.Application.Interfaces;

public interface IUserService
{
    Task<IEnumerable<UserDto>> GetAllAsync();
    Task<UserDto?> GetByIdAsync(Guid id);
    Task<UserDto?> GetByEmailAsync(string email);
    Task<Guid> CreateAsync(CreateUserDto dto);
    Task UpdateAsync(Guid id, CreateUserDto dto);
    Task DeleteAsync(Guid id);
}