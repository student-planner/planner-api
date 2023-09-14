using Microsoft.EntityFrameworkCore;
using Planner.Models;
using System.Data;

namespace Planner.API;

public sealed class DatabaseContext : DbContext
{
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Goal> Goals { get; set; } = null!;
    public DbSet<AuthTicket> AuthTickets { get; set; } = null!;

    public DatabaseContext() { }

    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
    {
        if (Database.GetPendingMigrations().Any())
            Database.Migrate();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(e =>
        {
            e.HasKey(e => e.Id);
            e.Property(e => e.Email).IsRequired();
            e.Property(e => e.RefreshToken);
            e.Property(e => e.RefreshTokenExpires);
            e.Property(e => e.Created).IsRequired();

            e.HasMany(u => u.Goals).WithOne(g => g.User).HasForeignKey(g => g.UserId);

            e.HasData(new User
            {
                Id = Guid.NewGuid(),
                Email = "seljmov@list.ru",
                Created = DateTime.UtcNow,
            });
        });

        modelBuilder.Entity<Goal>(e =>
        {
            e.HasKey(e => e.Id);
            e.Property(e => e.Name).IsRequired();
            e.Property(e => e.Description).IsRequired();
            e.Property(e => e.Deadline);
            e.Property(e => e.Labor);
            e.Property(e => e.Priority);
            e.Ignore(e => e.SubGoalsIds);
            e.Ignore(e => e.DependGoalsIds);

            e.HasOne(g => g.User).WithMany(u => u.Goals).HasForeignKey(g => g.UserId);
        });

        modelBuilder.Entity<AuthTicket>(e =>
        {
            e.HasKey(e => e.Id);
            e.Property(e => e.Code).IsRequired();
            e.Property(e => e.Login).IsRequired();
            e.Property(e => e.DeviceDescription);
            e.Property(e => e.ExpiresAt).IsRequired();
        });
    }
}
