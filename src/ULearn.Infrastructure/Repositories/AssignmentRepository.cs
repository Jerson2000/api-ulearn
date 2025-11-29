

using Microsoft.EntityFrameworkCore;
using ULearn.Domain.Entities;
using ULearn.Domain.Interfaces.Repositories;
using ULearn.Infrastructure.Data;

namespace ULearn.Infrastructure.Repositories;

public class AssignmentRepository : IAssignmentRepository
{
    private readonly ULearnDbContext _db;
    public AssignmentRepository(ULearnDbContext db) => _db = db;

    public async Task<List<Assignment>> GetAssignemnts(Guid lessonId)
    {
        return await _db.Assignments.AsNoTracking().Where(x=>x.LessonId == lessonId).ToListAsync();
    }

    public async Task<Assignment?> GetAssignmentWithSubmissions(Guid assignmentId)
    {
        return await _db.Assignments.AsNoTracking().Include(x=>x.Submissions).ThenInclude(xx=>xx.User).FirstOrDefaultAsync(x=>x.Id == assignmentId);
    }

    public async Task<AssignmentSubmission?> SubmitAsync(Guid assignmentId,Guid userId, string fileUrl, string? text)
    {
        var existing = await _db.AssignmentSubmissions.AsNoTracking().FirstOrDefaultAsync(s => s.AssignmentId == assignmentId && s.UserId == userId);

        if (existing != null) return existing;

        var submission = new AssignmentSubmission
        {
            AssignmentId = assignmentId,
            UserId = userId,
            FileUrl = fileUrl,
            TextSubmission = text ?? string.Empty,
            SubmittedAt = DateTime.UtcNow
        };

        _db.AssignmentSubmissions.Add(submission);
        await _db.SaveChangesAsync();
        return submission;
    }

    public async Task GradeAsync(Guid submissionId, double grade, string feedback)
    {
        var sub = await _db.AssignmentSubmissions.FindAsync(submissionId);
        if (sub != null)
        {
            sub.Grade = grade;
            sub.FeedBack = feedback;
            sub.GradedAt = DateTime.UtcNow;
            await _db.SaveChangesAsync();
        }
    }
}