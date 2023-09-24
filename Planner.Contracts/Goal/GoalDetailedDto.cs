using Planner.Models;

namespace Planner.Contracts.Goal;

/// <summary>
/// Модель данных для отображения подробной информации о задаче
/// </summary>
public class GoalDetailedDto : GoalBaseDto
{
    /// <summary>
    /// Трудоёмкость в секундах
    /// </summary>
    public double Labor { get; set; }

    /// <summary>
    /// Приоритет
    /// </summary>
    public GoalPriority Priority { get; set; }
    
    /// <summary>
    /// Коллекция идентификаторов подзадач
    /// </summary>
    public List<GoalBaseDto> SubGoals { get; set; } = new();
    
    /// <summary>
    /// Коллекция идентификаторов зависимых задач
    /// </summary>
    public List<GoalBaseDto> DependGoals { get; set; } = new();
}
