
using ULearn.Application.Interfaces;

namespace ULearn.Application.DTOs;


public record LoginDto(string Email, string Password) : IValidateRequest;

public record TokenRenewalDto(string access,string refresh):IValidateRequest;

public record AuthResponse(string AccessToken, string RefreshToken);