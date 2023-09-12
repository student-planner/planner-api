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
    public DateTime? Deadline { get; set; }
    
    /// <summary>
    /// Трудоёмкость
    /// </summary>
    public double? Labor { get; set; }
    
    /// <summary>
    /// Приоритет
    /// </summary>
    public GoalPriority? Priority { get; set; }
    
    /// <summary>
    /// Коллекция идентификаторов подзадач
    /// </summary>
    public ICollection<Guid> SubGoalsIds { get; set; } = new List<Guid>();
    
    /// <summary>
    /// Коллекция идентификаторов зависимых задач
    /// </summary>
    public ICollection<Guid> DependGoalsIds { get; set; } = new List<Guid>();
    
    /// <summary>
    /// Идентификатор пользователя, кому принадлежит задача
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Пользователь
    /// </summary>
    public virtual User User { get; set; } = null!;
}