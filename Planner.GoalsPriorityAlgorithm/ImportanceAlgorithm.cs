using Planner.Models;

namespace Planner.AlgorithmPriorityGoals;

/// <summary>
/// Алгоритм вычисления важности задач
/// </summary>
public class ImportanceAlgorithm : IImportanceAlgorithm
{
    /// <inheritdoc cref="IImportanceAlgorithm.Run(IReadOnlyCollection{Planner.Models.Goal},int)"/>
    public IEnumerable<Goal> Run(IReadOnlyCollection<Goal> goals, int neededGoalsCount = 3)
    {
        if (goals.Count < neededGoalsCount) return goals;
        
        var dataPreparer = new AlgorithmDataPreparer(goals);
        var algorithmItems = dataPreparer.GetData();

        var solver = new AlgorithmImportanceSolver(algorithmItems);
        var mostImportantGoalsIds = solver.GetIdsMostImportantGoals(neededGoalsCount);

        return goals.Where(goal => mostImportantGoalsIds.Contains(goal.Id));
    }
}