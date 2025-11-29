

using ULearn.Application.Interfaces;
using ULearn.Domain.Entities;

namespace ULearn.Application.DTOs;


public record AssignmentDto(Guid Id, string Title, string Instructions, DateTime DueDate, int MaxPoints, List<AssignmentSubmissionDto> Submissions);
public record CreateAssignmentRequestDto(string Title, string Instructions, DateTime DueDate, int MaxPoints) : IValidateRequest;

public record AssignmentSubmissionDto(Guid Id, string FileUrl, string TextSubmission, DateTime SubmittedAt, double Grade, string Feedback, DateTime GradedAt,UserDto? User = null);
public record CreateAssignmentSubmissionRequestDto(string FileUrl, string? TextSubmission = default) : IValidateRequest;
public record CreateGradeAssignmentRequestDto(double Grade, string? Feedback = default);
