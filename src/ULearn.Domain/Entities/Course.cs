

using ULearn.Domain.Enums;

namespace ULearn.Domain.Entities;

public class Course
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Thumbnail { get; set; } = string.Empty;
    public double Price { get; set; }
    public bool IsPublished { get; set; }
    public double DurationHours { get; set; }
    public CourseLevelEnum CourseLevel { get; set; }
    public Guid InstructorId { get; set; }
    public User? Instructor { get; set; }
    public Guid? CategoryId { get; set; }
    public Category? Category { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; }


    public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
    public ICollection<Module> Modules { get; set; } = new List<Module>();
    public ICollection<Payment> Payments { get; set; } = new List<Payment>();
    public ICollection<CourseReview> Reviews { get; set; } = new List<CourseReview>();
    public ICollection<Certificate> Certificates { get; set; } = new List<Certificate>();

}