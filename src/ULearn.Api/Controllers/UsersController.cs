using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using ULearn.Application.DTOs;
using ULearn.Application.Interfaces;
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
        var users = await _userService.GetAllAsync();
        return users.ToActionResult();
    }


    [HttpGet("{id:guid}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetUserById([FromRoute] Guid id)
    {
        var user = await _userService.GetByIdAsync(id);
        return user.ToActionResult();
    }

    [HttpPost]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserDto dto)
    {
        var user = await _userService.CreateAsync(dto);
        return user.ToActionResult();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAsync([FromRoute] Guid id, [FromBody] CreateUserDto dto)
    {
        var user = await _userService.UpdateAsync(id, dto);
        return user.ToActionResult();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        var user = await _userService.DeleteAsync(id);
        return user.ToActionResult();
    }
}