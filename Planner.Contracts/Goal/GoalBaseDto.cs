using Planner.Models;

namespace Planner.Contracts.Goal;

/// <summary>
/// Модель данных для отображения задач с базовым набором данных
/// </summary>
public class GoalBaseDto
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
    /// Статус
    /// </summary>
    public GoalStatus Status { get; set; }
}