


using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ULearn.Api.Extensions;
using ULearn.Application.DTOs;
using ULearn.Application.Interfaces;
using ULearn.Domain.Entities;

namespace ULearn.Api.Controllers;


[ApiController]
[Route("api/courses/{courseId}/modules/{moduleId}/lessons")]
public class LessonsController : BaseController
{
    private ILessonService _lessonService;
    public LessonsController(ILessonService lessonService)
    {
        _lessonService = lessonService;
    }

    [HttpGet]
    public async Task<IActionResult> GetLessons([FromRoute] Guid courseId, [FromRoute] Guid moduleId)
    {
        return await _lessonService.GetLessonsByModuleOrderedAsync(courseId,moduleId,UserId).ToActionResult();
    }

    [HttpGet("{lessonId}")]
    public async Task<IActionResult> GetLesson([FromRoute] Guid courseId, [FromRoute] Guid moduleId, [FromRoute] Guid lessonId)
    {
        return await _lessonService.GetLessonWithContentAsync(lessonId).ToActionResult();
    }

    [HttpPost]
    public async Task<IActionResult> CreateLesson([FromRoute] Guid courseId, [FromRoute] Guid moduleId, [FromBody]CreateLessonRequestDto dto)
    {
        return await _lessonService.AddLessonAsync(moduleId,dto,UserId).ToActionResult();
    }
    [HttpPut("{lessonId}")]
    [Authorize(Roles ="Instructor, Admin")]
    public async Task<IActionResult> UpdateLesson([FromRoute] Guid courseId, [FromRoute] Guid moduleId, [FromRoute] Guid lessonId, CreateLessonRequestDto dto)
    {
        return await _lessonService.UpdateLessonAsync(lessonId,dto,UserId).ToActionResult();
    }
    [HttpDelete("{lessonId}")]
    public async Task<IActionResult> DeleteLesson([FromRoute] Guid courseId, [FromRoute] Guid moduleId, [FromRoute] Guid lessonId)
    {
        throw new NotImplementedException();
    }
}