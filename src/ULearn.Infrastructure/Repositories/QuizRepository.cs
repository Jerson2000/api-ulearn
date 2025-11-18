


using Microsoft.EntityFrameworkCore;
using ULearn.Domain.Entities;
using ULearn.Domain.Enums;
using ULearn.Domain.Interfaces.Repositories;
using ULearn.Infrastructure.Data;

namespace ULearn.Infrastructure.Repositories;


public class QuizRepository : IQuizRepository
{
    private readonly ULearnDbContext _db;
    public QuizRepository(ULearnDbContext db) => _db = db;

    public async Task<Quiz?> GetWithQuestionsAsync(Guid quizId) => await _db.Quizzes
            .Include(q => q.Questions).ThenInclude(qq => qq.Options)
            .FirstOrDefaultAsync(q => q.Id == quizId);

    public async Task<QuizAttempt?> StartAttemptAsync(Guid userId,Guid quizId)
    {
        var quiz = await _db.Quizzes.FindAsync(quizId);
        if (quiz == null) return null;

        var attempt = new QuizAttempt
        {
            UserId = userId,
            QuizId = quizId,
            StartedAt = DateTime.UtcNow
        };

        _db.QuizAttempts.Add(attempt);
        await _db.SaveChangesAsync();
        return attempt;
    }

    public async Task<QuizAttempt?> SubmitAttemptAsync(Guid attemptId, List<QuizAnswer> answers)
    {
        var attempt = await _db.QuizAttempts.AsNoTracking().Include(a => a.Answers).FirstOrDefaultAsync(a => a.Id == attemptId);

        if (attempt == null) return null;

        foreach (var dto in answers)
        {
            var answer = new QuizAnswer
            {
                AttemptId = attemptId,
                QuestionId = dto.QuestionId,
                SelectedOptionId = dto.SelectedOptionId,
                TextAnswer = dto.TextAnswer
            };
            _db.QuizAnswers.Add(answer);
        }

        attempt.SubmittedAt = DateTime.UtcNow;
        attempt.Score = await CalculateScoreAsync(attemptId);
        attempt.IsPassed = attempt.Score >= 70; // assuming 70% passing

        await _db.SaveChangesAsync();
        return attempt;
    }

    public async Task<double> CalculateScoreAsync(Guid attemptId)
    {
        var answers = await _db.QuizAnswers
            .Where(a => a.AttemptId == attemptId)
            .Include(a => a.Question)
            .Include(a => a.SelectedOption)
            .ToListAsync();

        var totalPoints = answers.Sum(a => a.Question.Points);
        var earnedPoints = 0;

        foreach (var ans in answers)
        {
            if (ans.Question.QuestionType == QuizQuestionTypeEnum.MultipleChoice ||
                ans.Question.QuestionType == QuizQuestionTypeEnum.TrueOrFalse)
            {
                if (ans.SelectedOption?.IsCorrect == true)
                    earnedPoints += ans.Question.Points;
            }
            // ShortAnswer: manual grading later
        }

        return totalPoints > 0 ? Math.Round((double)earnedPoints / totalPoints * 100, 2) : 0;
    }
}