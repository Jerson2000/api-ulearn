using Microsoft.Extensions.Logging;
using ULearn.Application.DTOs;
using ULearn.Application.Interfaces;
using ULearn.Domain.Entities;
using ULearn.Domain.Enums;
using ULearn.Domain.Interfaces.Repositories;
using ULearn.Domain.Interfaces.Services;
using ULearn.Domain.Shared;
using BBCrypt = BCrypt.Net.BCrypt;


namespace ULearn.Application.Services;

public class AuthService(IUserRepository userRepository, ITokenService tokenService, ILogger<AuthService> logger) : IAuthService
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly ITokenService _tokenService = tokenService;
    private readonly ILogger<AuthService> _logger = logger;

    public async Task<Result<AuthResponse>> Login(LoginDto dto)
    {

        var user = await _userRepository.GetByEmailAsync(dto.Email);

        if (user is null || !BBCrypt.Verify(dto.Password, user.Password))
            return Result.Failure<AuthResponse>(ErroCodeEnum.BadRequest, "Wrong Email/Password!");

        var (access, refresh) = await _tokenService.GenerateJWTToken(user);
        var response = new AuthResponse(access, refresh);

        return Result.Success(response);
    }

    public async Task<Result<AuthResponse>> Signup(CreateUserDto dto)
    {
        var userExist = await _userRepository.GetByEmailAsync(dto.Email);
        if (userExist is not null)
            return Result.Failure<AuthResponse>(ErroCodeEnum.BadRequest, "Please choose another email!");

        var hashPassword = BBCrypt.HashPassword(dto.Password);
        var newUser = new User { FirstName = dto.FirstName, LastName = dto.LastName, Email = dto.Email, Password = hashPassword };
        var userId = await _userRepository.CreateAsync(newUser);
        var (access, refresh) = await _tokenService.GenerateJWTToken(new User { Id = userId });
        var response = new AuthResponse(access, refresh);

        return Result.Success(response);
    }


    public async Task<Result<AuthResponse>> Refresh(string token,string expiredAccessToken)
    {
        try
        {
            var (access, refresh) = await _tokenService.GenerateJWTRefreshToken(token,expiredAccessToken);
            var response = new AuthResponse(access, refresh);
            return Result.Success(response);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Unexpected error: {ex.Message}");
            return Result.Failure<AuthResponse>(ErroCodeEnum.Unauthorized, "Unauthorized");
        }

    }
}