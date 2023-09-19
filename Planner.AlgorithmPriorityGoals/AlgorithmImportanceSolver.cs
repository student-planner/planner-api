namespace Planner.AlgorithmPriorityGoals;

/// <summary>
/// Алгоритм вычисления самых важных задач
/// </summary>
public class AlgorithmImportanceSolver
{
    /// Задачи для планирования
    private List<AlgorithmItem> AlgorithmItems { get; }
    
    /// <summary>
    /// Конструктор класса
    /// </summary>
    /// <param name="algorithmItems">Подготовленные задачи для работы алгоритма</param>
    public AlgorithmImportanceSolver(List<AlgorithmItem> algorithmItems)
    {
        AlgorithmItems = algorithmItems;
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
        var taskIds = itemsSortedImportance.OrderByDescending(x => x)
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
        var itemsWithImportance = new Dictionary<double, AlgorithmItem>();
        
        foreach (var item in AlgorithmItems)
        {
            var importance = FindImportance(item);
            itemsWithImportance.Add(importance, item);
        }
        
        return itemsWithImportance;
    }
    
    /// <summary>
    /// Получить важность задачи
    /// </summary>
    /// <param name="item">Модель задачи для алгоритма</param>
    /// <returns></returns>
    private static double FindImportance(AlgorithmItem item)
    {
        return (double)(item.Priority + item.Labor + item.DependsPriority)! / (FindHoursBetweenDates(DateTime.Now, item.Deadline));
    }
    
    /// <summary>
    /// Получить разницу между датами в часах
    /// </summary>
    /// <param name="first">Начало времени</param>
    /// <param name="second">Конец времени</param>
    /// <returns></returns>
    private static double FindHoursBetweenDates(DateTime first, DateTime second)
    {
        var first1 = DateTime.Parse("2022-08-25 09:00");
        return second.Subtract(first1).TotalMinutes / 60;
    }
}