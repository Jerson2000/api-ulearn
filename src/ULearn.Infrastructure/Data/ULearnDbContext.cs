
using Microsoft.EntityFrameworkCore;
using ULearn.Domain.Entities;

namespace ULearn.Infrastructure.Data;

public class ULearnDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Token> Tokens => Set<Token>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Course> Courses => Set<Course>();
    public DbSet<Enrollment> Enrollments => Set<Enrollment>();
    public DbSet<Module> Modules => Set<Module>();
    public DbSet<Lesson> Lessons => Set<Lesson>();
    public DbSet<UserProgress> UserProgress => Set<UserProgress>();
    public DbSet<Quiz> Quizzes => Set<Quiz>();
    public DbSet<QuizQuestion> QuizQuestions => Set<QuizQuestion>();
    public DbSet<QuizOption> QuizOptions => Set<QuizOption>();
    public DbSet<QuizAttempt> QuizAttempts => Set<QuizAttempt>();
    public DbSet<QuizAnswer> QuizAnswers => Set<QuizAnswer>();
    public DbSet<Assignment> Assignments => Set<Assignment>();
    public DbSet<AssignmentSubmission> AssignmentSubmissions => Set<AssignmentSubmission>();
    public DbSet<Payment> Payments => Set<Payment>();
    public DbSet<CourseReview> CourseReviews => Set<CourseReview>();
    public DbSet<Certificate> Certificates => Set<Certificate>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        UserModelBuilder(modelBuilder);
        TokenModelBuilder(modelBuilder);

        ElearningModelBuilder(modelBuilder);
    }

    private static void UserModelBuilder(ModelBuilder modelBuilder)
    {
        // index
        modelBuilder.Entity<User>()
        .HasIndex(u => u.Email)
        .IsUnique();

        // relationship
        modelBuilder.Entity<User>()
       .HasMany(u => u.Tokens)
       .WithOne(t => t.User)
       .HasForeignKey(t => t.UserId)
       .OnDelete(DeleteBehavior.Cascade);
    }

    private static void TokenModelBuilder(ModelBuilder modelBuilder)
    {
        // indexes
        modelBuilder.Entity<Token>()
            .HasIndex(t => t.Access)
            .IsUnique();

        modelBuilder.Entity<Token>()
            .HasIndex(t => t.Refresh)
            .IsUnique();
    }
    private static void ElearningModelBuilder(ModelBuilder modelBuilder)
    {
        #region ── Unique Indexes ─────────────────────────────────────────────────────

        modelBuilder.Entity<Enrollment>()
            .HasIndex(e => new { e.UserId, e.CourseId })
            .IsUnique();

        modelBuilder.Entity<Module>()
            .HasIndex(m => new { m.CourseId, m.OrderIndex })
            .IsUnique();

        modelBuilder.Entity<Lesson>()
            .HasIndex(l => new { l.ModuleId, l.OrderIndex })
            .IsUnique();

        modelBuilder.Entity<CourseReview>()
            .HasIndex(r => new { r.CourseId, r.UserId })
            .IsUnique();

        modelBuilder.Entity<AssignmentSubmission>()
            .HasIndex(s => new { s.AssignmentId, s.UserId })
            .IsUnique();

        modelBuilder.Entity<Certificate>()
            .HasIndex(c => new { c.UserId, c.CourseId })
            .IsUnique();

        #endregion


        #region ── Relationships & Cascades ────────────────────────────────────────

        // ── Course ───────────────────────────────────────────────────────────────
        modelBuilder.Entity<Course>()
            .HasOne(c => c.Instructor)
            .WithMany(u => u.InstructedCourses)
            .HasForeignKey(c => c.InstructorId)
            .OnDelete(DeleteBehavior.Restrict);   // Instructor cannot be deleted if courses exist

        modelBuilder.Entity<Course>()
            .HasOne(c => c.Category)
            .WithMany(cat => cat.Courses)
            .HasForeignKey(c => c.CategoryId)
            .OnDelete(DeleteBehavior.SetNull);

        // ── Enrollment ───────────────────────────────────────────────────────────
        modelBuilder.Entity<Enrollment>()
            .HasOne(e => e.User)
            .WithMany(u => u.Enrollments)
            .HasForeignKey(e => e.UserId)
            .OnDelete(DeleteBehavior.Cascade);   // Delete enrollments when user is removed

        modelBuilder.Entity<Enrollment>()
            .HasOne(e => e.Course)
            .WithMany(c => c.Enrollments)
            .HasForeignKey(e => e.CourseId)
            .OnDelete(DeleteBehavior.Cascade);   // Delete enrollments when course is removed

        // ── Module ───────────────────────────────────────────────────────────────
        modelBuilder.Entity<Module>()
            .HasOne(m => m.Course)
            .WithMany(c => c.Modules)
            .HasForeignKey(m => m.CourseId)
            .OnDelete(DeleteBehavior.Cascade);

        // ── Lesson ───────────────────────────────────────────────────────────────
        modelBuilder.Entity<Lesson>()
            .HasOne(l => l.Module)
            .WithMany(m => m.Lessons)
            .HasForeignKey(l => l.ModuleId)
            .OnDelete(DeleteBehavior.Cascade);

        // One-to-one: Lesson → Quiz / Assignment
        modelBuilder.Entity<Lesson>()
            .HasOne(l => l.Quiz)
            .WithOne(q => q.Lesson)
            .HasForeignKey<Quiz>(q => q.LessonId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Lesson>()
            .HasOne(l => l.Assignment)
            .WithOne(a => a.Lesson)
            .HasForeignKey<Assignment>(a => a.LessonId)
            .OnDelete(DeleteBehavior.Cascade);

        // ── UserProgress ────────────────────────────────────────────────────────
        modelBuilder.Entity<UserProgress>()
            .HasOne(p => p.User)
            .WithMany(u => u.Progress)
            .HasForeignKey(p => p.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<UserProgress>()
            .HasOne(p => p.Lesson)
            .WithMany(l => l.UserProgress)
            .HasForeignKey(p => p.LessonId)
            .OnDelete(DeleteBehavior.Cascade);

        // ── Quiz → Question → Option ────────────────────────────────────────────
        modelBuilder.Entity<QuizQuestion>()
            .HasOne(q => q.Quiz)
            .WithMany(z => z.Questions)
            .HasForeignKey(q => q.QuizId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<QuizOption>()
            .HasOne(o => o.Question)
            .WithMany(q => q.Options)
            .HasForeignKey(o => o.QuestionId)
            .OnDelete(DeleteBehavior.Cascade);

        // ── QuizAttempt ────────────────────────────────────────────────────────
        modelBuilder.Entity<QuizAttempt>()
            .HasOne(a => a.User)
            .WithMany(u => u.QuizAttempts)
            .HasForeignKey(a => a.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<QuizAttempt>()
            .HasOne(a => a.Quiz)
            .WithMany(q => q.Attempts)
            .HasForeignKey(a => a.QuizId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<QuizAnswer>()
            .HasOne(ans => ans.Attempt)
            .WithMany(a => a.Answers)
            .HasForeignKey(ans => ans.AttemptId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<QuizAnswer>()
            .HasOne(ans => ans.Question)
            .WithMany(q => q.Answers)
            .HasForeignKey(ans => ans.QuestionId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<QuizAnswer>()
            .HasOne(ans => ans.SelectedOption)
            .WithMany()
            .HasForeignKey(ans => ans.SelectedOptionId)
            .OnDelete(DeleteBehavior.NoAction);

        // ── AssignmentSubmission ───────────────────────────────────────────────
        modelBuilder.Entity<AssignmentSubmission>()
            .HasOne(s => s.Assignment)
            .WithMany(a => a.Submissions)
            .HasForeignKey(s => s.AssignmentId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<AssignmentSubmission>()
            .HasOne(s => s.User)
            .WithMany(u => u.AssignmentSubmissions)
            .HasForeignKey(s => s.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // ── Payment ─────────────────────────────────────────────────────────────
        modelBuilder.Entity<Payment>()
            .HasOne(p => p.User)
            .WithMany(u => u.Payments)
            .HasForeignKey(p => p.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Payment>()
            .HasOne(p => p.Course)
            .WithMany(c => c.Payments)
            .HasForeignKey(p => p.CourseId)
            .OnDelete(DeleteBehavior.Cascade);

        // ── CourseReview ───────────────────────────────────────────────────────
        modelBuilder.Entity<CourseReview>()
            .HasOne(r => r.Course)
            .WithMany(c => c.Reviews)
            .HasForeignKey(r => r.CourseId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<CourseReview>()
            .HasOne(r => r.User)
            .WithMany(u => u.CourseReviews)
            .HasForeignKey(r => r.UserId)
            .OnDelete(DeleteBehavior.Cascade);


        // ── Certificate ────────────────────────────────────────────────────────
        modelBuilder.Entity<Certificate>()
            .HasOne(c => c.User)
            .WithMany(u => u.Certificates)
            .HasForeignKey(c => c.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Certificate>()
            .HasOne(c => c.Course)
            .WithMany(co => co.Certificates)
            .HasForeignKey(c => c.CourseId)
            .OnDelete(DeleteBehavior.Cascade);

        #endregion


        #region ── Default Values & Conventions ────────────────────────────────────

        // Timestamps (CreatedAt / UpdatedAt) – use UTC
        var dateProps = modelBuilder.Model.GetEntityTypes()
            .SelectMany(t => t.GetProperties())
            .Where(p => p.Name is "CreatedAt" or "UpdatedAt");

        foreach (var prop in dateProps)
        {
            prop.SetDefaultValueSql("GETUTCDATE()");
        }

        // UpdatedAt should be refreshed on update
        modelBuilder.Entity<User>().Property(u => u.UpdatedAt)
            .ValueGeneratedOnAddOrUpdate()
            .HasDefaultValueSql("GETUTCDATE()");

        modelBuilder.Entity<Course>().Property(c => c.UpdatedAt)
            .ValueGeneratedOnAddOrUpdate()
            .HasDefaultValueSql("GETUTCDATE()");

        #endregion


        #region ── Auto-Include (optional) ─────────────

        // modelBuilder.Entity<User>()
        //     .Navigation(u => u.InstructedCourses).AutoInclude();

        // modelBuilder.Entity<User>()
        //     .Navigation(u => u.Enrollments).AutoInclude();

        // modelBuilder.Entity<Course>()
        //     .Navigation(c => c.Modules).AutoInclude();

        // modelBuilder.Entity<Module>()
        //     .Navigation(m => m.Lessons).AutoInclude();

        // modelBuilder.Entity<Lesson>()
        //     .Navigation(l => l.Quiz).AutoInclude();

        // modelBuilder.Entity<Lesson>()
        //     .Navigation(l => l.Assignment).AutoInclude();

        #endregion
    }


}