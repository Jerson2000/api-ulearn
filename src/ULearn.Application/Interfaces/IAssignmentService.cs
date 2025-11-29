

using ULearn.Application.DTOs;
using ULearn.Domain.Shared;

namespace ULearn.Application.Interfaces;

public interface IAssignmentService
{
    Task<Result<AssignmentDto>> AddAssignmentAsync(Guid lessonId, CreateAssignmentRequestDto dto);
    Task<Result<AssignmentDto>> UpdateAssignmentAsync(Guid assignmentId, CreateAssignmentRequestDto dto);
    Task<Result<AssignmentDto>> DeleteAssignmentAsync(Guid assignmentId);
    Task<Result<AssignmentSubmissionDto?>> SubmitAssignmentAsync(Guid assignmentId, Guid userId, CreateAssignmentSubmissionRequestDto dto);
    Task<Result> GradeAssignmentAsync(Guid submissionId, CreateGradeAssignmentRequestDto dto);
    Task<Result<List<AssignmentDto>>> GetAssignemnts(Guid lessonId);
    Task<Result<AssignmentDto?>> GetAssignmentWithSubmissions(Guid assignmentId);
}