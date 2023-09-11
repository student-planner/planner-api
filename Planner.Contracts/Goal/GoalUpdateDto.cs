namespace Planner.Contracts.Goal;

/// <summary>
/// Модель данных для обновления задачи
/// </summary>
public class GoalUpdateDto
{
    /// <summary>
    /// Название
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Описание
    /// </summary>
    public string Description { get; set; } = string.Empty;
}
