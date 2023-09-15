using Microsoft.EntityFrameworkCore;
using Planner.Models;

namespace Planner.API;

/// <summary>
/// Контекст базы данных
/// </summary>
public sealed class DatabaseContext : DbContext
{
    /// <summary>
    /// Таблица пользователей
    /// </summary>
    public DbSet<User> Users { get; set; } = null!;
    
    /// <summary>
    /// Таблица задач
    /// </summary>
    public DbSet<Goal> Goals { get; set; } = null!;
    
    /// <summary>
    /// Таблица хранения тикетов для авторизации и регистрации
    /// </summary>
    public DbSet<AuthTicket> AuthTickets { get; set; } = null!;

    /// <summary>
    /// Конструктор по умолчанию
    /// </summary>
    public DatabaseContext() { }

    /// <summary>
    /// Конструктор с авто-миграцией
    /// </summary>
    /// <param name="options">Опции базы данных</param>
    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
    {
        if (Database.GetPendingMigrations().Any())
            Database.Migrate();
    }

    /// <summary>
    /// Создание сущностей в базе данных на основе моделей
    /// </summary>
    /// <param name="modelBuilder">Конструктор сущностей</param>
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
