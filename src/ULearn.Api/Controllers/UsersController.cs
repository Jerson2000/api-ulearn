using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using ULearn.Application.DTOs;
using ULearn.Application.Interfaces;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore.Diagnostics;
using ULearn.Api.Utils;
using Microsoft.Extensions.Caching.Memory;
using ULearn.Api.Extensions;

namespace ULearn.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController(IUserService userService, IDistributedCache distributedCache, IMemoryCache memoryCache) : ControllerBase
{
    private readonly IUserService _userService = userService;
    private readonly IDistributedCache _distributedCache = distributedCache;
    private readonly IMemoryCache _memoryCache = memoryCache;

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAll()
    {
        string cacheKey = "users:all";
        return await _userService.GetAllAsync()
            .ToDistributedCachedActionResult(
                cache: _distributedCache,
                cacheKey: cacheKey,
                cacheOptions: new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
                });
    }


    [HttpGet("{id:guid}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetUserById([FromRoute] Guid id)
    {
        string cacheKey = id.ToString();

        return await _userService.GetByIdAsync(id).ToMemoryCachedActionResult(_memoryCache, cacheKey);
    }

    [HttpPost]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserDto dto)
    {
        await _userService.CreateAsync(dto);
        return Ok();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAsync([FromRoute] Guid id, [FromBody] CreateUserDto dto)
    {
        await _userService.UpdateAsync(id, dto);
        return Ok();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        await _userService.DeleteAsync(id);
        return Ok();
    }
}