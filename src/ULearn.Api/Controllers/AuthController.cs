

using System.Security.Claims;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ULearn.Api.Extensions;
using ULearn.Api.Utilities;
using ULearn.Application.DTOs;
using ULearn.Application.Interfaces;
using ULearn.Domain.Enums;
using ULearn.Domain.Shared;

namespace ULearn.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IAuthService authService, IAntiforgery antiforgery) : BaseController
{
    private readonly IAntiforgery _antiforgery = antiforgery;

    private readonly IAuthService _authService = authService;

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        var result = await _authService.Login(dto);
        if (result.IsSuccess)
        {
            CookieManager.AuthTokenSet(Response, result.Value.AccessToken, result.Value.RefreshToken);
        }
        return result.ToActionResult();
    }

    [HttpPost("signup")]
    [AllowAnonymous]
    public async Task<IActionResult> Signup([FromBody] CreateUserDto dto)
    {
        var result = await _authService.Signup(dto);
        if (result.IsSuccess)
        {
            CookieManager.AuthTokenSet(Response, result.Value.AccessToken, result.Value.RefreshToken);
        }
        return result.ToActionResult();
    }

    [HttpPost("refresh")]
    [AllowAnonymous]
    public async Task<IActionResult> Refresh()
    {
        var authHeader = Request.Headers.Authorization.FirstOrDefault();
        var expiredAccessToken = authHeader?.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase) == true
            ? authHeader["Bearer ".Length..].Trim()
            : null;

        var refreshToken = CookieManager.GetCookie(Request, "Secure__h$3rf3r")
                           ?? Request.Headers["x-refresh-token"].FirstOrDefault();

        if (string.IsNullOrWhiteSpace(refreshToken) || string.IsNullOrEmpty(expiredAccessToken))
        {
            return Unauthorized(new Error(ErroCodeEnum.Unauthorized, "Missing refresh token"));
        }

        var result = await _authService.Refresh(refreshToken, expiredAccessToken);
        if (result.IsFailure)
            return result.ToActionResult();

        CookieManager.AuthTokenSet(Response, result.Value.AccessToken, result.Value.RefreshToken);
        return Ok();
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
}