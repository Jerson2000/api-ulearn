

using ULearn.Application.DTOs;
using ULearn.Application.Interfaces;
using ULearn.Application.Mappers;
using ULearn.Domain.Entities;
using ULearn.Domain.Shared;

namespace ULearn.Application.Services;

public class AssignmentService : IAssignmentService
{
    private readonly IUnitOfWork _unitOfWork;

    public AssignmentService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<AssignmentDto>> AddAssignmentAsync(Guid lessonId, CreateAssignmentRequestDto dto)
    {
        var assignmentRepo = _unitOfWork.Repository<Assignment>();
        var exist = await _unitOfWork.Repository<Lesson>().GetByIdAsync(lessonId);
        if (exist == null) return Result.FailureBadRequest<AssignmentDto>("The specified lesson could not be found.");

        var assignment = new Assignment
        {
            LessonId = lessonId,
            Title = dto.Title,
            Instructions = dto.Instructions,
            DueDate = dto.DueDate,
            MaxPoints = dto.MaxPoints
        };

        await assignmentRepo.AddAsync(assignment);
        await _unitOfWork.SaveChangesAsync();

        return Result.Success(assignment.ToAssignmentDto());
    }

    public async Task<Result<AssignmentDto>> UpdateAssignmentAsync(Guid assignmentId, CreateAssignmentRequestDto dto)
    {
        var assignmentRepo = _unitOfWork.Repository<Assignment>();
        var assignment = await assignmentRepo.GetByIdAsync(assignmentId);

        if (assignment == null)
            return Result.FailureBadRequest<AssignmentDto>("The specified assignment could not be found.");

        assignment.Title = dto.Title;
        assignment.Instructions = dto.Instructions;
        assignment.DueDate = dto.DueDate;
        assignment.MaxPoints = dto.MaxPoints;

        assignmentRepo.Update(assignment);
        await _unitOfWork.SaveChangesAsync();

        return Result.Success(assignment.ToAssignmentDto());
    }

    public Task<Result<AssignmentDto>> DeleteAssignmentAsync(Guid assignmentId)
    {
        throw new NotImplementedException();
    }

    public async Task<Result> GradeAssignmentAsync(Guid submissionId,CreateGradeAssignmentRequestDto dto)
    {
        await _unitOfWork.Assignments.GradeAsync(submissionId, dto.Grade, dto.Feedback??string.Empty);
        return Result.Success();
    }

    public async Task<Result<AssignmentSubmissionDto?>> SubmitAssignmentAsync(Guid assignmentId, Guid userId, CreateAssignmentSubmissionRequestDto dto)
    {
        var assignmentRepo = _unitOfWork.Repository<Assignment>();
        var assignment = await assignmentRepo.GetByIdAsync(assignmentId);
        if (assignment == null)
            return Result.FailureBadRequest<AssignmentSubmissionDto?>("The specified assignment could not be found.");

        var submission = await _unitOfWork.Assignments.SubmitAsync(assignmentId, userId, dto.FileUrl, dto.TextSubmission);
        return Result.Success(submission?.ToAssignmentSubmissionDto());
    }

    public async Task<Result<List<AssignmentDto>>> GetAssignemnts(Guid lessonId)
    {
        var assignments = await _unitOfWork.Assignments.GetAssignemnts(lessonId);
        return Result.Success(assignments.ToAssignmentDtoList());
    }

    public async Task<Result<AssignmentDto?>> GetAssignmentWithSubmissions(Guid assignmentId)
    {
        var assignment = await _unitOfWork.Assignments.GetAssignmentWithSubmissions(assignmentId);
        return Result.Success(assignment?.ToAssignmentDto());
    }
}