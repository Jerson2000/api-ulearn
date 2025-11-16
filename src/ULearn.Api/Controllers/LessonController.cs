


using Microsoft.AspNetCore.Mvc;
using ULearn.Api.Extensions;
using ULearn.Application.DTOs;
using ULearn.Application.Interfaces;

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
        throw new NotImplementedException();
    }

    [HttpGet("{lessonId}")]
    public async Task<IActionResult> GetLesson([FromRoute] Guid courseId, [FromRoute] Guid moduleId, [FromRoute] Guid lessonId)
    {
        throw new NotImplementedException();
    }

    [HttpPost]
    public async Task<IActionResult> CreateLesson([FromRoute] Guid courseId, [FromRoute] Guid moduleId, CreateLessonRequestDto dto)
    {
        throw new NotImplementedException();
    }
    [HttpPut("{lessonId}")]
    public async Task<IActionResult> UpdateLesson([FromRoute] Guid courseId, [FromRoute] Guid moduleId, [FromRoute] Guid lessonId, CreateLessonRequestDto dto)
    {
        throw new NotImplementedException();
    }
    [HttpDelete("{lessonId}")]
    public async Task<IActionResult> DeleteLesson([FromRoute] Guid courseId, [FromRoute] Guid moduleId, [FromRoute] Guid lessonId)
    {
        throw new NotImplementedException();
    }
}