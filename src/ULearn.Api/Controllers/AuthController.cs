

using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ULearn.Api.Extensions;
using ULearn.Application.DTOs;
using ULearn.Application.Interfaces;

namespace ULearn.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IAuthService authService) : ControllerBase
{
    private readonly IAuthService _authService = authService;

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {        
        return await _authService.Login(dto).ToActionResult();
    }

    [HttpPost("signup")]
    [AllowAnonymous]
    public async Task<IActionResult> Signup([FromBody] CreateUserDto dto)
    {
        return await _authService.Signup(dto).ToActionResult();
    }

    [HttpGet("me")]
    public IActionResult Me()
    {
          string? userId = User.FindFirst(ClaimTypes.Email)?.Value;
        return Ok(new { Hello = "Hello world.",User=userId });
    }
}