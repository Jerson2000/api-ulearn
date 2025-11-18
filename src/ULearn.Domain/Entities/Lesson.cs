

using ULearn.Domain.Enums;

namespace ULearn.Domain.Entities;

public class Lesson
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid ModuleId { get; set; }
    public Module? Module { get; set; }
    public string Title { get; set; } = string.Empty;
    public string ContentUrl { get; set; } = string.Empty;
    public string ContentText { get; set; } = string.Empty;
    public LessonContentTypeEnum ContentType { get; set; }
    public int DurationMinutes { get; set; }
    public int OrderIndex { get; set; }
    public bool IsPreview { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;


    public Quiz? Quiz { get; set; }
    public Assignment? Assignment { get; set; }
    public ICollection<UserProgress> UserProgress { get; set; } = new List<UserProgress>();
}