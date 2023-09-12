using System.ComponentModel.DataAnnotations;

namespace Planner.Contracts.Goal;

/// <summary>
/// Модель данных для обновления задачи
/// </summary>
public class GoalUpdateDto
{
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
}
