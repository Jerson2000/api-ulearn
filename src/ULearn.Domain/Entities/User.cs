

using System.ComponentModel.DataAnnotations;
using ULearn.Domain.Enums;

namespace ULearn.Domain.Entities;

public class User
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    public string LastName { get; set; } = string.Empty;

    [Required]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string Password { get; set; } = string.Empty;

    public string Avatar { get; set; } = string.Empty;

    public string Bio { get; set; } = string.Empty;

    public bool IsVerified { get; set; } = false;

    public UserRole Role { get; set; } = UserRole.User;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime UpdatedAt { get; set; }

    public string FullName => $"{FirstName} {LastName}";

    public ICollection<Token> Tokens { get; set; } = new List<Token>();
    public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
    public ICollection<Course> InstructedCourses { get; set; } = new List<Course>();
    public ICollection<QuizAttempt> QuizAttempts { get; set; } = new List<QuizAttempt>();
    public ICollection<AssignmentSubmission> AssignmentSubmissions { get; set; } = new List<AssignmentSubmission>();
    public ICollection<Payment> Payments { get; set; } = new List<Payment>();
    public ICollection<CourseReview> CourseReviews { get; set; } = new List<CourseReview>();
    public ICollection<UserProgress> Progress { get; set; } = new List<UserProgress>();
    public ICollection<Certificate> Certificates { get; set; } = new List<Certificate>();
}