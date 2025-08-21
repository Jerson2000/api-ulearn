
using ULearn.Application.DTOs;
using ULearn.Domain.Shared;

namespace ULearn.Application.Interfaces;

public interface IUserService
{
    Task<Result<IReadOnlyList<UserDto>>> GetAllAsync();
    Task<Result<UserDto?>> GetByIdAsync(Guid id);
    Task<Result<UserDto?>> GetByEmailAsync(string email);
    Task<Result<Guid>> CreateAsync(CreateUserDto dto);
    Task<Result> UpdateAsync(Guid id, CreateUserDto dto);
    Task<Result> DeleteAsync(Guid id);
}