
using Microsoft.EntityFrameworkCore;
using ULearn.Domain.Entities;

namespace ULearn.Infrastructure.Data;

public class ULearnDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<User> Users { set; get; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
    }


}