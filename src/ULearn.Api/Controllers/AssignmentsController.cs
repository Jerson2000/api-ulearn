

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ULearn.Api.Extensions;
using ULearn.Application.DTOs;
using ULearn.Application.Interfaces;

namespace ULearn.Api.Controllers;


[ApiController]
[Route("api/lessons/{lessonId}/assignments")]
public class AssignmentsController : BaseController
{
    private readonly IAssignmentService _assignmentService;

    public AssignmentsController(IAssignmentService assignmentService)
    {
        _assignmentService = assignmentService;
    }


    [HttpPost]
    [Authorize(Roles = "Instructor, Admin")]
    public async Task<IActionResult> AddAssignmentAsync([FromRoute] Guid lessonId, CreateAssignmentRequestDto dto)
    {
        return await _assignmentService.AddAssignmentAsync(lessonId, dto).ToActionResult();
    }

    [HttpPut("{assignmentId}")]
    [Authorize(Roles = "Instructor, Admin")]
    public async Task<IActionResult> UpdateAssignmentAsync([FromRoute] Guid assignmentId, CreateAssignmentRequestDto dto)
    {
        return await _assignmentService.UpdateAssignmentAsync(assignmentId, dto).ToActionResult();
    }

    [HttpPost("{assignmentId}/submit")]
    public async Task<IActionResult> SubmitAssignmentAsync([FromRoute] Guid assignmentId, CreateAssignmentSubmissionRequestDto dto)
    {
        return await _assignmentService.SubmitAssignmentAsync(assignmentId, UserId, dto).ToActionResult();
    }

    [HttpGet]
    public async Task<IActionResult> GetAssignments([FromRoute] Guid lessonId)
    {
        return await _assignmentService.GetAssignemnts(lessonId).ToActionResult();
    }

    [HttpGet("{assignmentId}")]
    public async Task<IActionResult> GetAssignmentWithSubmissions([FromRoute] Guid assignmentId)
    {
        return await _assignmentService.GetAssignmentWithSubmissions(assignmentId).ToActionResult();
    }

    [HttpPost("{assignmentId}/submissions/{submissionId}/grade")]
    [Authorize(Roles = "Instructor, Admin")]
    public async Task<IActionResult> GradeAssignmentAsync([FromRoute] Guid submissionId,[FromBody] CreateGradeAssignmentRequestDto dto) 
    {
        return await _assignmentService.GradeAssignmentAsync(submissionId,dto).ToActionResult();
    }

}