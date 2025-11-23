

using ULearn.Application.DTOs;
using ULearn.Application.Interfaces;
using ULearn.Application.Mappers;
using ULearn.Domain.Entities;
using ULearn.Domain.Shared;
using ULearnq.Application.Mappers;

namespace ULearn.Application.Services;

public class QuizService : IQuizService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IParallelUnitOfWork _parallelUnitOfWork;

    public QuizService(IUnitOfWork unitOfWork, IParallelUnitOfWork parallelUnitOfWork)
    {
        _unitOfWork = unitOfWork;
        _parallelUnitOfWork = parallelUnitOfWork;
    }

    public async Task<Result<QuizOptionDto>> AddQuestionOptionToQuestion(Guid questionId, CreateQuestionOptionRequestDto dto)
    {
        var exist = await _unitOfWork.Repository<QuizQuestion>().GetByIdAsync(questionId);
        if (exist == null) return Result.FailureBadRequest<QuizOptionDto>("The specified question could not be found.");

        var option = new QuizOption
        {
            QuestionId = questionId,
            OptionText = dto.OptionText,
            IsCorrect = dto.IsCorrect
        };

        await _unitOfWork.Repository<QuizOption>().AddAsync(option);
        await _unitOfWork.SaveChangesAsync();

        return Result.Success(option.ToQuizOptionDto());
    }

    public async Task<Result<QuizQuestionDto>> AddQuestionToQuizAsync(Guid quizId, CreateQuestionRequesDto dto)
    {
        var exist = await _unitOfWork.Repository<Quiz>().GetByIdAsync(quizId);
        if (exist == null) return Result.FailureBadRequest<QuizQuestionDto>("The specified quiz could not be found.");

        var question = new QuizQuestion
        {
            QuizId = quizId,
            QuestionText = dto.Question,
            QuestionType = dto.QuestionType,
            Points = dto.Points,
            OrderIndex = dto.OrderIndex
        };

        await _unitOfWork.Repository<QuizQuestion>().AddAsync(question);
        await _unitOfWork.SaveChangesAsync();

        return Result.Success(question.ToQuizQuestionDto());
    }

    public async Task<Result<QuizDto>> AddQuizToLessonAsync(Guid lessonId, CreateQuizRequestDto dto)
    {
        var exist = await _unitOfWork.Repository<Lesson>().GetByIdAsync(lessonId);
        if (exist == null) return Result.FailureBadRequest<QuizDto>("The specified lesson could not be found.");

        var quiz = new Quiz
        {
            LessonId = lessonId,
            Title = dto.Title,
            PassingScore = dto.PassingScore,
            TimeLimitMinutes = dto.TimeLimitInMinutes
        };

        await _unitOfWork.Repository<Quiz>().AddAsync(quiz);
        await _unitOfWork.SaveChangesAsync();

        return Result.Success(quiz.ToQuizDto());
    }

    public async Task<Result> DeleteQuestionAsync(Guid questionId)
    {
        _unitOfWork.Repository<QuizQuestion>().Delete(new QuizQuestion { Id = questionId });
        await _unitOfWork.Repository<QuizQuestion>().SaveChangesAsync();
        return Result.Success();
    }

    public async Task<Result> DeleteQuestionOptionAsync(Guid questionOptionId)
    {
        _unitOfWork.Repository<QuizOption>().Delete(new QuizOption { Id = questionOptionId });
        await _unitOfWork.Repository<QuizOption>().SaveChangesAsync();
        return Result.Success();
    }

    public async Task<Result> DeleteQuizAsync(Guid quizId)
    {
        _unitOfWork.Repository<Quiz>().Delete(new Quiz { Id = quizId });
        await _unitOfWork.Repository<Quiz>().SaveChangesAsync();
        return Result.Success();
    }

    public async Task<Result<QuizDto?>> GetQuizWithQuestionsAsync(Guid quizId)
    {
        var quiz = await _unitOfWork.Quizzes.GetQuizWithQuestionsAsync(quizId);
        return Result.Success(quiz?.ToQuizDto());
    }

    public async Task<Result<List<QuizDto>>> GetQuizzes(Guid lessonId)
    {
        var quizzes = await _unitOfWork.Quizzes.GetQuizzes(lessonId);
        return Result.Success(quizzes.ToQuizDtoList());
    }

    public async Task<Result<List<QuizQuestionDto>>> GetQuestions(Guid quizId)
    {
        var questions = await _unitOfWork.Quizzes.GetQuestions(quizId);
        return Result.Success(questions.ToQuizQuestionDtoList());
    }

    public async Task<Result<QuizAttemptDto>> StartQuizAttemptAsync(Guid userId, Guid quizId)
    {
        var attempt = await _unitOfWork.Quizzes.StartAttemptAsync(userId, quizId);
        if (attempt == null) return Result.FailureBadRequest<QuizAttemptDto>("Unable to start the quiz. Please try again.");
        return Result.Success(attempt.ToQuizAttemptDto());
    }

    public async Task<Result<QuizAttemptDto>> SubmitAttemptAsync(Guid attemptId, List<QuizAnswerDto> answers)
    {
        var attempt = await _unitOfWork.Quizzes.SubmitAttemptAsync(attemptId, answers.ToQuizAnswerEntityList());
        if (attempt == null) return Result.FailureBadRequest<QuizAttemptDto>("We couldn't process your quiz submission. Please try again later.");
        return Result.Success(attempt.ToQuizAttemptDto());
    }

    public async Task<Result<QuizQuestionDto>> UpdateQuestionAsync(Guid questionId, CreateQuestionRequesDto dto)
    {
        var question = await _unitOfWork.Repository<QuizQuestion>().GetByIdAsync(questionId);
        if (question == null)
            return Result.FailureBadRequest<QuizQuestionDto>("The specified question could not be found.");

        question.QuestionText = dto.Question;
        question.QuestionType = dto.QuestionType;
        question.Points = dto.Points;
        question.OrderIndex = dto.OrderIndex;

        _unitOfWork.Repository<QuizQuestion>().Update(question);
        await _unitOfWork.SaveChangesAsync();

        return Result.Success(question.ToQuizQuestionDto());
    }


    public async Task<Result<QuizOptionDto>> UpdateQuestionOption(Guid optionId, CreateQuestionOptionRequestDto dto)
    {
        var option = await _unitOfWork.Repository<QuizOption>().GetByIdAsync(optionId);
        if (option == null)
            return Result.FailureBadRequest<QuizOptionDto>("The specified question option could not be found.");

        option.OptionText = dto.OptionText;
        option.IsCorrect = dto.IsCorrect;

        _unitOfWork.Repository<QuizOption>().Update(option);
        await _unitOfWork.SaveChangesAsync();

        return Result.Success(option.ToQuizOptionDto());
    }

    public async Task<Result<QuizDto>> UpdateQuizAsync(Guid quizId, CreateQuizRequestDto dto)
    {
        var quiz = await _unitOfWork.Repository<Quiz>().GetByIdAsync(quizId);
        if (quiz == null)
            return Result.FailureBadRequest<QuizDto>("The specified quiz could not be found.");

        quiz.Title = dto.Title;
        quiz.PassingScore = dto.PassingScore;
        quiz.TimeLimitMinutes = dto.TimeLimitInMinutes;

        _unitOfWork.Repository<Quiz>().Update(quiz);
        await _unitOfWork.SaveChangesAsync();

        return Result.Success(quiz.ToQuizDto());
    }
}