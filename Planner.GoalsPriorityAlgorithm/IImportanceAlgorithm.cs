using Planner.Models;

namespace Planner.AlgorithmPriorityGoals;

/// <summary>
/// Интерфейс алгоритма вычисления важности задач
/// </summary>
public interface IImportanceAlgorithm
{
    /// <summary>
    /// Выполнить алгоритм
    /// </summary>
    /// <param name="goals">Задачи</param>
    /// <param name="neededGoalsCount">Количество наиболее важных задач, которые нужно вернуть</param>
    /// <returns>Наиболее важные задачи на текущий момент</returns>
    IEnumerable<Goal> Run(IReadOnlyCollection<Goal> goals, int neededGoalsCount = 3);
}