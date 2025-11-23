

using Microsoft.AspNetCore.Mvc;
using ULearn.Api.Extensions;
using ULearn.Application.DTOs;
using ULearn.Application.Interfaces;

namespace ULearn.Api.Controllers;

[ApiController]
[Route("api/lessons/{lessonId}/quizzes")]
public class QuizzesController : BaseController
{

    private readonly IQuizService _quizService;

    public QuizzesController(IQuizService quizService)
    {
        _quizService = quizService;
    }


    [HttpGet]
    public async Task<IActionResult> GetQuizzes([FromRoute] Guid lessonId)
    {
        return await _quizService.GetQuizzes(lessonId).ToActionResult();
    }


    [HttpGet("{quizId}")]
    public async Task<IActionResult> GetQuizWithQuestionsAsync([FromRoute] Guid quizId)
    {
        return await _quizService.GetQuizWithQuestionsAsync(quizId).ToActionResult();
    }

    [HttpPost]
    public async Task<IActionResult> CreateQuiz([FromRoute] Guid lessonId,[FromBody] CreateQuizRequestDto dto)
    {
        return await _quizService.AddQuizToLessonAsync(lessonId,dto).ToActionResult();
    }


}