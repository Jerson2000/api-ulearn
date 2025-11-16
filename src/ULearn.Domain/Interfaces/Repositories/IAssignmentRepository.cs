using ULearn.Domain.Entities;

namespace ULearn.Domain.Interfaces.Repositories;


public interface IAssignmentRepository
{
    Task<AssignmentSubmission?> SubmitAsync(Guid assignmentId,Guid userId, string fileUrl, string? text);
    Task GradeAsync(Guid submissionId,double grade, string feedback);
}