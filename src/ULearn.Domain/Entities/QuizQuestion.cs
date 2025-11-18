

using ULearn.Domain.Enums;

namespace ULearn.Domain.Entities;

public class QuizQuestion
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid QuizId { get; set; }
    public Quiz? Quiz { get; set; }
    public string QuestionText { get; set; } = string.Empty;
    public QuizQuestionTypeEnum QuestionType { get; set; }
    public int Points { get; set; }
    public int OrderIndex { get; set; }

    public ICollection<QuizOption> Options { get; set; } = new List<QuizOption>();
    public ICollection<QuizAnswer> Answers { get; set; } = new List<QuizAnswer>();
}