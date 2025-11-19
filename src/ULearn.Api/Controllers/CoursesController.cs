

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Ocsp;
using ULearn.Api.Extensions;
using ULearn.Application.DTOs;
using ULearn.Application.Interfaces;

namespace ULearn.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CoursesController : BaseController
{
    private readonly ICourseService _courseService;
    public CoursesController(ICourseService courseService)
    {
        _courseService = courseService;
       
    }

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] string? search = null)
    {
        return await _courseService.GetPublishedAsync(page, pageSize, search).ToActionResult();
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(Guid id)
    {
        return await _courseService.GetAsync(id).ToActionResult();
    }

    [HttpPost]
    [Authorize(Roles = "Instructor, Admin")]
    public async Task<IActionResult> Create([FromBody] CreateCourseRequestDto request)
    {
        return await _courseService.CreateAsync(request, UserId).ToActionResult();
    }

    [HttpPut("{courseId}")]
    [Authorize(Roles ="Instructor, Admin")]
    public async Task<IActionResult> Update([FromRoute] Guid courseId,[FromBody] CreateCourseRequestDto request)
    {
        return await _courseService.UpdateAsync(courseId,request,UserId).ToActionResult();
    }

    [HttpPost("enroll/{courseId}")]
    [Authorize(Roles = "User")]
    public async Task<IActionResult> Enroll(Guid courseId)
    {
        return await _courseService.EnrollAsync(UserId, courseId).ToActionResult();
    }

}