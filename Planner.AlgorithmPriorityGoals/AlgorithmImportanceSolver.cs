namespace Planner.AlgorithmPriorityGoals;

/// <summary>
/// Алгоритм вычисления самых важных задач
/// </summary>
public class AlgorithmImportanceSolver
{
    /// Задачи для планирования
    private readonly List<AlgorithmItem> _algorithmItems;
    
    /// <summary>
    /// Конструктор класса
    /// </summary>
    /// <param name="algorithmItems">Подготовленные задачи для работы алгоритма</param>
    public AlgorithmImportanceSolver(List<AlgorithmItem> algorithmItems)
    {
        _algorithmItems = algorithmItems;
    }
    
    /// <summary>
    /// олучить идентификаторы самых важных задач
    /// </summary>
    /// <param name="goalsCount">Количество задач</param>
    /// <returns></returns>
    public List<Guid> GetIdsMostImportantGoals(int goalsCount = 3)
    {
        var items = GetAlgorithmItemsWithImportance();
        var itemsSortedImportance = new List<double>(items.Keys);
        itemsSortedImportance.Sort();
        var taskIds = itemsSortedImportance.OrderByDescending(item => item)
            .ToList()
            .GetRange(0, goalsCount);
            
        return taskIds.Select(key => items[key]!.Id).ToList();
    }
    
    /// <summary>
    /// Получить задачи с их коэфициентом важности
    /// </summary>
    /// <returns></returns>
    private Dictionary<double, AlgorithmItem> GetAlgorithmItemsWithImportance()
    {
        return _algorithmItems.ToDictionary(FindImportance, item => item);
    }
    
    /// <summary>
    /// Получить важность задачи
    /// </summary>
    /// <param name="item">Модель задачи для алгоритма</param>
    /// <returns></returns>
    private static double FindImportance(AlgorithmItem item)
    {
        return (item.Priority + item.DependsPriority + LaborToHours(item.Labor)) / 
            (FindHoursBetweenDates(DateTime.UtcNow, item.Deadline) + 1);
    }
    
    /// <summary>
    /// Получить разницу между датами в секундах
    /// </summary>
    /// <param name="first">Начало времени</param>
    /// <param name="second">Конец времени</param>
    /// <returns></returns>
    private static double FindHoursBetweenDates(DateTime first, DateTime second)
    {
        return second.Subtract(first).TotalSeconds;
    }
    
    /// <summary>
    /// Получить трудоёмкость в часах
    /// </summary>
    /// <param name="labor">Трудоёмкость в секундах</param>
    private static double LaborToHours(double labor) => labor / 3600;
}