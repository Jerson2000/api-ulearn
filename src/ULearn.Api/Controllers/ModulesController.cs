using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ULearn.Api.Extensions;
using ULearn.Application.DTOs;
using ULearn.Application.Interfaces;
using ULearn.Domain.Entities;

namespace ULearn.Api.Controllers;

[ApiController]
[Route("api/courses/{courseId}/modules")]
public class ModulesController : BaseController
{
    private readonly IModuleService _moduleService;
    public ModulesController(IModuleService moduleService)
    {
        _moduleService = moduleService;
    }

    [HttpGet]
    public async Task<IActionResult> GetModulesForCourse([FromRoute] Guid courseId)
    {
        return await _moduleService.GetModulesByCourseOrderedAsync(courseId,UserId).ToActionResult();
    }

    [HttpGet("{moduleId}")]
    public async Task<IActionResult> GetModule([FromRoute] Guid courseId, [FromRoute] Guid moduleId)
    {
        return await _moduleService.GetModuleAsync(moduleId,UserId).ToActionResult();
    }

    [HttpPost]
    [Authorize(Roles ="Instructor, Admin")]
    public async Task<IActionResult> CreateModule([FromRoute] Guid courseId,[FromBody] CreateModuleRequestDto dto)
    {
        return await _moduleService.AddModuleAsync(courseId,dto,UserId).ToActionResult();
    }

    [HttpPut("{moduleId}")]
    [Authorize(Roles ="Instructor, Admin")]
    public async Task<IActionResult> UpdateModule([FromRoute] Guid courseId, [FromRoute] Guid moduleId, CreateModuleRequestDto dto)
    {
        return await _moduleService.UpdateModuleAsync(moduleId,dto,UserId).ToActionResult();
    }

    [HttpDelete("{moduleId}")]
    public async Task<IActionResult> DeleteModule(Guid courseId, Guid moduleId)
    {
        throw new NotImplementedException();
    }
}