
using ULearn.Application.DTOs;
using ULearn.Domain.Shared;

namespace ULearn.Application.Interfaces;

public interface IAuthService
{
    Task<Result<AuthResponse>> Login(LoginDto dto);
    Task<Result<AuthResponse>> Signup(CreateUserDto dto);
    Task<Result<AuthResponse>> Refresh(string token, string expiredAccessToken);
}