using Planner.API;
using Planner.Models;

namespace Planner.AlgorithmPriorityGoals;

/// <summary>
/// Алгоритм планирования
/// </summary>
public class SchedulerAlgorithm
{
    /// <summary>
    /// Задачи
    /// </summary>
    private List<Goal> _goals { get; set; }

    /// <summary>
    /// Конструктор алгоритма
    /// </summary>
    /// <param name="goals">Задачи</param>
    public SchedulerAlgorithm(List<Goal> goals)
    {
        _goals = goals;
    }

    /// <summary>
    /// Начать планирование
    /// </summary>
    public List<Guid> Run()
    {
        var dataPreparer = new AlgorithmDataPreparer(_goals);
        var algorithmItems = dataPreparer.GetData();

        var solver = new AlgorithmImportanceSolver(algorithmItems);
        var mostImportantGoalsIds = solver.GetIdsMostImportantGoals();

        return mostImportantGoalsIds;
    }
}