


using Microsoft.AspNetCore.Mvc;
using ULearn.Api.Extensions;
using ULearn.Application.DTOs;
using ULearn.Application.Interfaces;

namespace ULearn.Api.Controllers;

[ApiController]
[Route("api/quizzes/{quizId}/questions")]
public class QuestionsController : BaseController
{

    private readonly IQuizService _quizService;

    public QuestionsController(IQuizService quizService)
    {
        _quizService = quizService;
    }


    [HttpGet]
    public async Task<IActionResult> GetQuestions([FromRoute] Guid quizId)
    {
        return await _quizService.GetQuestions(quizId).ToActionResult();
    }

    [HttpPost]
    public async Task<IActionResult> CreateQuestion([FromRoute] Guid quizId,[FromBody] CreateQuestionRequesDto dto)
    {
        return await _quizService.AddQuestionToQuizAsync(quizId,dto).ToActionResult();
    }


    [HttpPost("{questioId}")]
    public async Task<IActionResult> CreateQuestionOption([FromRoute] Guid questioId,[FromBody] CreateQuestionOptionRequestDto dto)
    {
        return await _quizService.AddQuestionOptionToQuestion(questioId,dto).ToActionResult();
    }

}