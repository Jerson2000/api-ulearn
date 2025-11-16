

using ULearn.Domain.Entities;

namespace ULearn.Domain.Interfaces.Repositories;

public interface IQuizRepository
{
    Task<Quiz?> GetWithQuestionsAsync(Guid quizId);
    Task<QuizAttempt?> StartAttemptAsync(Guid userId, Guid quizId);
    Task<QuizAttempt?> SubmitAttemptAsync(Guid attemptId, List<QuizAnswer> answers);
    Task<double> CalculateScoreAsync(Guid attemptId);
}