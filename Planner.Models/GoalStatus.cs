namespace Planner.Models;

/// <summary>
/// Статус задачи
/// </summary>
public enum GoalStatus
{
    /// <summary>
    /// Новая
    /// </summary>
    New,
    
    /// <summary>
    /// В процессе
    /// </summary>
    InProgress,
    
    /// <summary>
    /// Выполнена
    /// </summary>
    Done,
    
    /// <summary>
    /// Просрочена
    /// </summary>
    Overdue,
}