


namespace ULearn.Domain.Entities;

public class QuizAnswer
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid AttemptId { get; set; }
    public QuizAttempt Attempt { get; set; } = null!;
    public Guid QuestionId { get; set; }
    public QuizQuestion Question { get; set; } = null!;
    public Guid? SelectedOptionId { get; set; }
    public QuizOption? SelectedOption { get; set; }
    public string TextAnswer { get; set; } = string.Empty;
}