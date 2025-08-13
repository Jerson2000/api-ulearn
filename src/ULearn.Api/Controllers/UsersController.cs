using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using ULearn.Application.DTOs;
using ULearn.Application.Interfaces;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore.Diagnostics;
using ULearn.Api.Utils;

namespace ULearn.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController(IUserService userService, IDistributedCache distributedCache) : ControllerBase
{
    private readonly IUserService _userService = userService;
    private readonly IDistributedCache _distributedCache = distributedCache;

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAll()
    {
        var key = "Users";
        var cachedData = await _distributedCache.GetStringAsync(key);

        if (!string.IsNullOrEmpty(cachedData))
        {
            var usersCached = CacheHelper.DeserializeFromBase64<List<UserDto>>(cachedData);
            Console.WriteLine("Cached return");
            return Ok(usersCached);
        }

        var users = await _userService.GetAllAsync();
        if (users.Count > 0)
        {
            var base64String = CacheHelper.SerializeToBase64(users);
            var options = new DistributedCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(1));

            await _distributedCache.SetStringAsync(key, base64String, options);
        }

        Console.WriteLine("Dili cached return");
        return Ok(users);
    }


    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetUserById([FromRoute] Guid id)
    {
        var item = await _userService.GetByIdAsync(id);
        return Ok(item);
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