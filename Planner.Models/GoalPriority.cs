namespace Planner.Models;

/// <summary>
/// Приоритет задачи
/// </summary>
public enum GoalPriority
{
    /// <summary>
    /// Очень низкий
    /// </summary>
    ExtraLow = 1,
    
    /// <summary>
    /// Низкий
    /// </summary>
    Low = 3,
    
    /// <summary>
    /// Средний
    /// </summary>
    Medium = 7,
    
    /// <summary>
    /// Высокий
    /// </summary>
    High = 13,
    
    /// <summary>
    /// Очень высокий
    /// </summary>
    ExtraHigh = 21,
}