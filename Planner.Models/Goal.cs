namespace Planner.Models;

/// <summary>
/// Модель задачи
/// </summary>
public class Goal
{
    /// <summary>
    /// Идентификатор
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    /// Название
    /// </summary>
    public string Name { get; set; } = string.Empty;
    
    /// <summary>
    /// Описание
    /// </summary>
    public string Description { get; set; } = string.Empty;
    
    /// <summary>
    /// Крайний срок выполнения
    /// </summary>
    public DateTime Deadline { get; set; } = DateTime.UtcNow;
    
    /// <summary>
    /// Трудоёмкость в секундах
    /// </summary>
    public double Labor { get; set; } = 0;
    
    /// <summary>
    /// Приоритет
    /// </summary>
    public GoalPriority Priority { get; set; } = GoalPriority.Low;
    
    /// <summary>
    /// Статус
    /// </summary>
    public GoalStatus Status { get; set; }
    
    /// <summary>
    /// Коллекция идентификаторов подзадач
    /// </summary>
    public List<Guid> SubGoalsIds { get; set; } = new();
    
    /// <summary>
    /// Коллекция идентификаторов зависимых задач
    /// </summary>
    public List<Guid> DependGoalsIds { get; set; } = new();
    
    /// <summary>
    /// Идентификатор пользователя, кому принадлежит задача
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Пользователь
    /// </summary>
    public virtual User User { get; set; } = null!;
}