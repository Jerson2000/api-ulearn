

using ULearn.Application.DTOs;
using ULearn.Domain.Shared;

namespace ULearn.Application.Interfaces;


public interface IQuizService
{
    Task<Result<QuizDto>> AddQuizToLessonAsync(Guid lessonId,CreateQuizRequestDto dto);
    Task<Result<QuizDto>> UpdateQuizAsync(Guid quizId, CreateQuizRequestDto dto);
    Task<Result> DeleteQuizAsync(Guid quizId);
    Task<Result<QuizQuestionDto>> AddQuestionToQuizAsync(Guid quizId, CreateQuestionRequesDto dto);
    Task<Result<QuizQuestionDto>> UpdateQuestionAsync(Guid questionId, CreateQuestionRequesDto dto);
    Task<Result> DeleteQuestionAsync(Guid questionId);
    Task<Result<QuizOptionDto>> AddQuestionOptionToQuestion(Guid questionId,CreateQuestionOptionRequestDto dto);
    Task<Result<QuizOptionDto>> UpdateQuestionOption(Guid optionId,CreateQuestionOptionRequestDto dto);
    Task<Result> DeleteQuestionOptionAsync(Guid questionOptionId);
    Task<Result<QuizDto?>> GetQuizWithQuestionsAsync(Guid quizId);
    Task<Result<List<QuizDto>>> GetQuizzes(Guid lessonId);
    Task<Result<List<QuizQuestionDto>>> GetQuestions(Guid quizId);
    Task<Result<QuizAttemptDto>> StartQuizAttemptAsync(Guid userId,Guid quizId);
    Task<Result<QuizAttemptDto>> SubmitAttemptAsync(Guid attemptId, List<QuizAnswerDto> answers);
}