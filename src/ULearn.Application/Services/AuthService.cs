using ULearn.Application.DTOs;
using ULearn.Application.Interfaces;
using ULearn.Domain.Entities;
using ULearn.Domain.Enums;
using ULearn.Domain.Interfaces.Repository;
using ULearn.Domain.Interfaces.Services;
using ULearn.Domain.Shared;
using BBCrypt = BCrypt.Net.BCrypt;


namespace ULearn.Application.Services;

public class AuthService(IUserRepository userRepository, ITokenService tokenService) : IAuthService
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly ITokenService _tokenService = tokenService;

    public async Task<Result<AuthResponse>> Login(LoginDto dto)
    {

        var user = await _userRepository.GetByEmailAsync(dto.Email);

        if (user is null || !BBCrypt.Verify(dto.Password, user.Password))
            return Result.Failure<AuthResponse>(new Error(ErroCodeEnum.BadRequest, "Wrong Email/Password!"));

        var token = _tokenService.GenerateToken(user);
        var refreshToken = _tokenService.GenerateRefreshToken(user);
        var response = new AuthResponse(token, refreshToken);

        return Result.Success(response);
    }

    public async Task<Result<AuthResponse>> Signup(CreateUserDto dto)
    {
        var hashPassword = BBCrypt.HashPassword(dto.Password);
        var newUser = new User { FirstName = dto.FirstName, LastName = dto.LastName, Email = dto.Email, Password = hashPassword };
        await _userRepository.CreateAsync(newUser);

        var token = _tokenService.GenerateToken(newUser);
        var refreshToken = _tokenService.GenerateRefreshToken(newUser);
        var response = new AuthResponse(token, refreshToken);
        
        return Result.Success(response);
    }
}