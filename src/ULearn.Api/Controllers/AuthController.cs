

using System.Security.Claims;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ULearn.Api.Extensions;
using ULearn.Application.DTOs;
using ULearn.Application.Interfaces;

namespace ULearn.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IAuthService authService, IAntiforgery antiforgery) : ControllerBase
{
    private readonly IAntiforgery _antiforgery = antiforgery;

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
        return Ok(new { Hello = "Hello world.", User = userId });
    }

    [HttpGet("token")]
    [AllowAnonymous]
    public IActionResult GetCSRFToken()
    {
        var tokens = _antiforgery.GetAndStoreTokens(HttpContext);
        Response.Headers.Append("X-CSRF-TOKEN", tokens.RequestToken!);
        return Ok(new { Token = tokens.RequestToken!.ToString() });
    }

    [HttpPost("sample-request")]
    [AllowAnonymous]
    public IActionResult DoSomething()
    {
        return Ok();
    }

}