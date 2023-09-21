using System.ComponentModel.DataAnnotations;
using Planner.Models;

namespace Planner.Contracts.Goal;

/// <summary>
/// Модель данных для создания задачи
/// </summary>
public class GoalPutDto
{
    /// <summary>
    /// Идентификатор
    /// </summary>
    public Guid? Id { get; set; }
    
    /// <summary>
    /// Название
    /// </summary>
    [Required(ErrorMessage = "Не указано название задачи!")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Описание
    /// </summary>
    [Required(ErrorMessage = "Не указано описание задачи!")]
    public string Description { get; set; } = string.Empty;
    
    /// <summary>
    /// Крайний срок выполнения
    /// </summary>
    [Required(ErrorMessage = "Не указан крайний срок выполнения задачи!")]
    public DateTime Deadline { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Коллекция идентификаторов подзадач
    /// </summary>
    public List<Guid> SubGoalsIds { get; set; } = new();
    
    /// <summary>
    /// Коллекция идентификаторов зависимых задач
    /// </summary>
    public List<Guid> DependGoalsIds { get; set; } = new();
    
    /// <summary>
    /// Трудоёмкость в секундах
    /// </summary>
    public double Labor { get; set; } = 0;

    /// <summary>
    /// Приоритет
    /// </summary>
    public GoalPriority Priority { get; set; } = GoalPriority.Low;
}
