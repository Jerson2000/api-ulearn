
using Microsoft.EntityFrameworkCore;
using ULearn.Domain.Entities;

namespace ULearn.Infrastructure.Data;

public class ULearnDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<User> Users { set; get; } = null!;
    public DbSet<Token> Tokens { set; get; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        UserModelBuilder(modelBuilder);
        TokenModelBuilder(modelBuilder);
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


}