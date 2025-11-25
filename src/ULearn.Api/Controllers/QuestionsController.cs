


using Microsoft.AspNetCore.Authorization;
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
    [Authorize(Roles = "Instructor, Admin")]
    public async Task<IActionResult> CreateQuestion([FromRoute] Guid quizId, [FromBody] CreateQuestionRequesDto dto)
    {
        return await _quizService.AddQuestionToQuizAsync(quizId, dto).ToActionResult();
    }

    [HttpPut("{questionId}")]
    [Authorize(Roles = "Instructor, Admin")]
    public async Task<IActionResult> UpdateQuestion([FromRoute] Guid questionId, [FromBody] CreateQuestionRequesDto dto)
    {
        return await _quizService.UpdateQuestionAsync(questionId, dto).ToActionResult();
    }

    [HttpPost("{questionId}/options")]
    [Authorize(Roles = "Instructor, Admin")]
    public async Task<IActionResult> CreateQuestionOption([FromRoute] Guid questionId, [FromBody] CreateQuestionOptionRequestDto dto)
    {
        return await _quizService.AddQuestionOptionToQuestion(questionId, dto).ToActionResult();
    }

    [HttpPut("{questioId}/options/{optionId}")]
    [Authorize(Roles = "Instructor, Admin")]
    public async Task<IActionResult> UpdateQuestionOption([FromRoute] Guid optionId, [FromBody] CreateQuestionOptionRequestDto dto)
    {
        return await _quizService.UpdateQuestionOption(optionId, dto).ToActionResult();
    }

}