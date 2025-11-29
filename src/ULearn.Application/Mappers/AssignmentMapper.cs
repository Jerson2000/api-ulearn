

using ULearn.Application.DTOs;
using ULearn.Domain.Entities;

namespace ULearn.Application.Mappers;

public static class AssignmentMapper
{
    
    public static AssignmentDto ToAssignmentDto(this Assignment x)
    {
        return new AssignmentDto(x.Id,x.Title,x.Instructions,x.DueDate,x.MaxPoints,x.Submissions.ToList().ToAssignmentSubmissionDtoList());
    }

    public static AssignmentSubmissionDto ToAssignmentSubmissionDto(this AssignmentSubmission x)
    {
        return new AssignmentSubmissionDto(x.Id,x.FileUrl,x.TextSubmission,x.SubmittedAt,x.Grade,x.FeedBack,x.GradedAt,x.User?.ToUserDto());
    }

    public static List<AssignmentDto> ToAssignmentDtoList(this List<Assignment> list)
    {
        return list.Select(x=>x.ToAssignmentDto()).ToList();
    }

    public static List<AssignmentSubmissionDto> ToAssignmentSubmissionDtoList(this List<AssignmentSubmission> list)
    {
        return list.Select(x=>x.ToAssignmentSubmissionDto()).ToList();
    }
}