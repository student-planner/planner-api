using Planner.Models;

namespace Planner.AlgorithmPriorityGoals;

/// Подготовщик данных для планирования
public class AlgorithmDataPreparer
{
    private readonly IReadOnlyCollection<Goal> _goals;

    public AlgorithmDataPreparer(IReadOnlyCollection<Goal> goals)
    {
        _goals = goals;
    }

    /// <summary>
    /// Получить подготовленные данные для работы алгоритма
    /// </summary>
    /// <returns></returns>
    public List<AlgorithmItem> GetData()
    {
        return PreparingDataForAlgorithm(_goals).ToList();
    }

    /// <summary>
    /// Подготовка данных для получения
    /// </summary>
    /// <param name="goals">Список задач</param>
    /// <returns></returns>
    private static IEnumerable<AlgorithmItem> PreparingDataForAlgorithm(IReadOnlyCollection<Goal> goals)
    {
        var goalForPlanning = GetTasksForPlanning(goals).ToList();
        var goalsDependsPriorities = GetTasksDependsPriorities(goals);

        return goalForPlanning.Select(goal => new AlgorithmItem()
        {
            Id = goal.Id,
            Deadline = goal.Deadline,
            Labor = goal.Labor,
            Priority = (int)goal.Priority,
            DependsIds = goal.DependGoalsIds,
            DependsPriority = goalsDependsPriorities.GetValueOrDefault(goal.Id, 0),
        }).ToList();
    }
    
    /// <summary>
    /// Получить данные, которые можно запланировать
    /// </summary>
    /// <param name="goals">Список задач</param>
    /// <returns>Коллекция задач, которые можно запланировать</returns>
    private static IEnumerable<Goal> GetTasksForPlanning(IReadOnlyCollection<Goal> goals)
    {
        var goalsIds = goals.Select(goal => goal.Id);
        return goals.Where(goal => !goal.DependGoalsIds.Any(id => goalsIds.Contains(id)));
    }
    
    /// <summary>
    /// Получить идентификаторы заказов, от которых зависят другие заказы с их приоритетом зависимости
    /// </summary>
    /// <param name="goals">Список задач</param>
    /// <returns>Коллекция идентификаторов заказов, от которых зависят другие заказы с их приоритетом зависимости</returns>
    private static Dictionary<Guid, int> GetTasksDependsPriorities(IEnumerable<Goal> goals)
    {
        var hasDependIds = goals
            .Where(goal => goal.DependGoalsIds.Any())
            .SelectMany(goal => goal.DependGoalsIds)
            .ToList();

        var dependIdCount = hasDependIds
            .GroupBy(item => item)
            .ToDictionary(item => item.Key, item => item.Count());

        var dependPrioritiesByTaskId = dependIdCount.Keys
            .ToDictionary(item => item, item => dependIdCount[item] * 2);

        return dependPrioritiesByTaskId;
    }
}