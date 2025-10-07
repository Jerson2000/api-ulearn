
using ULearn.Application.Interfaces;

namespace ULearn.Application.DTOs;

public record UserDto(Guid Id, string FirstName, string LastName, string Email, string Password, DateTime CreatedAt);

public record CreateUserDto(string FirstName, string LastName, string Email, string Password) : IValidateRequest;

#region User response DTO

public record UserResponseDto(Guid Id, string FirstName, string LastName, string Email, DateTime CreatedAt);

#endregion