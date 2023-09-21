using Planner.Models;

namespace Planner.Contracts.Goal;

/// <summary>
/// Модель данных для отображения задач
/// </summary>
public class GoalDto
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
    /// Коллекция идентификаторов подзадач
    /// </summary>
    public ICollection<Guid> SubGoalsIds { get; set; } = new List<Guid>();
    
    /// <summary>
    /// Коллекция идентификаторов зависимых задач
    /// </summary>
    public ICollection<Guid> DependGoalsIds { get; set; } = new List<Guid>();
    
    /// <summary>
    /// Статус
    /// </summary>
    public GoalStatus Status { get; set; }
}
