using Microsoft.AspNetCore.Mvc;
using ULearn.Application.DTOs;
using ULearn.Application.Interfaces;

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
    public async Task<IActionResult> GetModulesForCourse(Guid courseId)
    {
        throw new NotImplementedException();
    }

    [HttpGet("{moduleId}")]
    public async Task<IActionResult> GetModule(Guid courseId, Guid moduleId)
    {
        throw new NotImplementedException();
    }

    [HttpPost]
    public async Task<IActionResult> CreateModule(Guid courseId, CreateModuleRequestDto dto)
    {
        throw new NotImplementedException();
    }

    [HttpPut("{moduleId}")]
    public async Task<IActionResult> UpdateModule(Guid courseId, Guid moduleId, CreateModuleRequestDto dto)
    {
        throw new NotImplementedException();
    }

    [HttpDelete("{moduleId}")]
    public async Task<IActionResult> DeleteModule(Guid courseId, Guid moduleId)
    {
        throw new NotImplementedException();
    }
}