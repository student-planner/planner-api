using Planner.Models;

namespace Planner.AlgorithmPriorityGoals;

/// Подготовщик данных для планирования
public class AlgorithmDataPreparer
{
    private readonly List<Goal> _goals;

    public AlgorithmDataPreparer(List<Goal> goals)
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
    private static IEnumerable<AlgorithmItem> PreparingDataForAlgorithm(List<Goal> goals)
    {
        var goalForPlanning = GetTasksForPlanning(goals).ToList();
        var goalsDependsPriorities = GetTasksDependsPriorities(goals);

        return goalForPlanning.Select(goal => new AlgorithmItem()
        {
            Id = goal.Id,
            Deadline = goal.Deadline!.Value,
            Labor = goal.Labor ?? 0,
            Priority = (int)(goal.Priority ?? GoalPriority.ExtraHigh),
            DependsIds = goal.DependGoalsIds,
            DependsPriority = goalsDependsPriorities.GetValueOrDefault(goal.Id, 0),
        }).ToList();
    }
    
    /// <summary>
    /// Получить данные, которые можно запланировать
    /// </summary>
    /// <param name="goals">Список задач</param>
    /// <returns></returns>
    private static IEnumerable<Goal> GetTasksForPlanning(List<Goal> goals)
    {
        var goalsIds = goals.Select(goal => goal.Id);
        return goals.Where(goal => !goal.DependGoalsIds.Any(id => goalsIds.Contains(id)));
    }
    
    /// <summary>
    /// Получить идентификаторы заказов, от которых зависят другие заказы с их приоритетом зависимости
    /// </summary>
    /// <param name="goals">Список задач</param>
    /// <returns></returns>
    private static Dictionary<Guid, int> GetTasksDependsPriorities(List<Goal> goals)
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