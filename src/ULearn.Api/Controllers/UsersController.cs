using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using ULearn.Application.DTOs;
using ULearn.Application.Interfaces;

namespace ULearn.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController(IUserService userService,IDistributedCache distributedCache) : ControllerBase
{
    private readonly IUserService _userService = userService;
    private readonly IDistributedCache _distributedCache = distributedCache;

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var users = await _userService.GetAllAsync();
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