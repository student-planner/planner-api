using Planner.Models;

namespace Planner.Contracts.Goal;

/// <summary>
/// Модель данных для отображения задач
/// </summary>
public class GoalImportantDto : GoalBaseDto
{
    /// <summary>
    /// Трудоёмкость в секундах
    /// </summary>
    public double Labor { get; set; } = 0;

    /// <summary>
    /// Приоритет
    /// </summary>
    public GoalPriority Priority { get; set; } = GoalPriority.Low;
}
