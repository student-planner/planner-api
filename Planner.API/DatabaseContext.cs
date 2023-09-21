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
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Email).IsRequired();
            entity.Property(e => e.RefreshToken);
            entity.Property(e => e.RefreshTokenExpires);
            entity.Property(e => e.Created).IsRequired().HasDefaultValueSql("now()");
            entity.Property(e => e.DeviceDescription);

            entity.HasMany(u => u.Goals).WithOne(g => g.User).HasForeignKey(g => g.UserId);

            entity.HasData(new User
            {
                Id = Guid.NewGuid(),
                Email = "seljmov@list.ru",
                Created = DateTime.UtcNow,
            });
        });

        modelBuilder.Entity<Goal>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired();
            entity.Property(e => e.Description).IsRequired();
            entity.Property(e => e.Deadline).IsRequired();
            entity.Property(e => e.Created).IsRequired().HasDefaultValueSql("now()");
            entity.Property(e => e.Labor).IsRequired();
            entity.Property(e => e.Priority).IsRequired();
            entity.Property(e => e.Status).IsRequired();
            entity.Property(e => e.SubGoalsIds).IsRequired();
            entity.Property(e => e.DependGoalsIds).IsRequired();

            entity.HasOne(g => g.User).WithMany(u => u.Goals).HasForeignKey(g => g.UserId);
        });

        modelBuilder.Entity<AuthTicket>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Code).IsRequired();
            entity.Property(e => e.Login).IsRequired();
            entity.Property(e => e.DeviceDescription);
            entity.Property(e => e.ExpiresAt).IsRequired();
        });
    }
}
