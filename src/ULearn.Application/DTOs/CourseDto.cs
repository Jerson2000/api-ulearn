
using ULearn.Application.Interfaces;

namespace ULearn.Application.DTOs;


public record CourseDto(
    Guid Id,
    string Title,
    string? Description,
    string? Thumbnail,
    double Price,
    string InstructorName,
    int EnrollmentCount,
    string? Category
):IValidateRequest;



public record CreateCourseRequestDto(
    string Title,
    string? Description,
    double Price,
    Guid CategoryId
):IValidateRequest;



public record EnrollRequest(Guid CourseId):IValidateRequest;