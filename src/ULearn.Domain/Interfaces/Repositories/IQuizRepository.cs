

using ULearn.Domain.Entities;

namespace ULearn.Domain.Interfaces.Repositories;

public interface IQuizRepository
{
    Task<List<Quiz>> GetQuizzes(Guid lessonId, bool includeQuestion = false, bool includeQuestionOption = false);
    Task<Quiz?> GetQuizWithQuestionsAsync(Guid quizId);
    Task<List<QuizQuestion>> GetQuestions(Guid quizId);
    Task<QuizAttempt?> StartAttemptAsync(Guid userId, Guid quizId);
    Task<QuizAttempt?> SubmitAttemptAsync(Guid attemptId, List<QuizAnswer> answers);
    Task<double> CalculateScoreAsync(Guid attemptId);
}