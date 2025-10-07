
using ULearn.Application.DTOs;
using ULearn.Domain.Shared;

namespace ULearn.Application.Interfaces;

public interface IUserService
{
    Task<Result<IReadOnlyList<UserResponseDto>>> GetAllAsync();
    Task<Result<UserResponseDto?>> GetByIdAsync(Guid id);
    Task<Result<UserResponseDto?>> GetByEmailAsync(string email);
    Task<Result<Guid>> CreateAsync(CreateUserDto dto);
    Task<Result> UpdateAsync(Guid id, CreateUserDto dto);
    Task<Result> DeleteAsync(Guid id);
}