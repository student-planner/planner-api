namespace Planner.Models;

/// <summary>
/// Статус задачи
/// </summary>
public enum GoalStatus
{
    /// <summary>
    /// В процессе
    /// </summary>
    InProgress,
    
    /// <summary>
    /// Новая
    /// </summary>
    New,
    
    /// <summary>
    /// Просрочена
    /// </summary>
    Overdue,
    
    /// <summary>
    /// Выполнена
    /// </summary>
    Done,
}