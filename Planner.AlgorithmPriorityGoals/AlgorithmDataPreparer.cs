using Planner.Models;

namespace Planner.AlgorithmPriorityGoals;

/// Подготовщик данных для планирования
public class AlgorithmDataPreparer
{
    private List<Goal> Goals { get; set; } = new List<Goal>();

    /// <summary>
    /// Получить подготовленные данные для работы алгоритма
    /// </summary>
    /// <returns></returns>
    public List<AlgorithmItem> GetData()
    {
        return _preparingDataForAlgorithm(Goals).ToList();
    }

    /// <summary>
    /// Подготовка данных для получения
    /// </summary>
    /// <param name="goals">Список задач</param>
    /// <returns></returns>
    public static IEnumerable<AlgorithmItem> _preparingDataForAlgorithm(List<Goal> goals)
    {
        var goalsDependsPriorities = _getTasksDependsPriorities(goals);
        var goalForPlanning = _getTasksForPlanning(goals).ToList();

        return goalForPlanning.Select(goal => new AlgorithmItem()
        {
            Id = goal.Id,
            Deadline = goal.Deadline,
            Labor = goal.Labor,
            Priority = (int?)goal.Priority,
            DependsIds = goal.DependGoalsIds,
            DependsPriority = goalsDependsPriorities[goal.Id]
        }).ToList();
    }
    
    /// <summary>
    /// Получить данные, которые можно запланировать
    /// </summary>
    /// <param name="goals">Список задач</param>
    /// <returns></returns>
    public static IEnumerable<Goal> _getTasksForPlanning(List<Goal> goals)
    {
        var goalsIds = goals.Select(goal => goal.Id);
        return goals.Where(goal =>
            goal.DependGoalsIds.Any(id => goalsIds.Contains(id)));
    }
    
    /// <summary>
    /// Получить идентификаторы заказов, от которых зависят другие заказы с их приоритетом зависимости
    /// </summary>
    /// <param name="goals">Список задач</param>
    /// <returns></returns>
    public static Dictionary<Guid, int> _getTasksDependsPriorities(List<Goal> goals)
    {
        Dictionary<Guid, int> dependsPriorities = new Dictionary<Guid, int>();
        Dictionary<Guid, Goal> goalsByIds = new Dictionary<Guid, Goal>();

        foreach (var goal in goals)
        {
            dependsPriorities.Add(goal.Id, 0);
            goalsByIds.Add(goal.Id, goal);
        }

        var isDependsIds = goals
            .Where(goal => goal.DependGoalsIds.Count > 0)
            .SelectMany(goal => goal.DependGoalsIds)
            .ToList();

        foreach (var dependsId in isDependsIds)
        {
            if (goalsByIds.ContainsKey(dependsId))
            {
                dependsPriorities[dependsId]++;
            }
        }

        return dependsPriorities;
    }
}