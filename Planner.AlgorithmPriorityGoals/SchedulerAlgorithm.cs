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
    /// База данных
    /// </summary>
    private readonly DatabaseContext _context;

    /// <summary>
    /// Конструктор алгоритма
    /// </summary>
    /// <param name="goals">Задачи</param>
    /// <param name="context">База данных</param>
    public SchedulerAlgorithm(List<Goal> goals, DatabaseContext context)
    {
        _goals = goals;
        _context = context;
    }

    /// <summary>
    /// Начать планирование
    /// </summary>
    public List<Goal?> Run()
    {
        var dataPreparer = new AlgorithmDataPreparer(_goals);
        var algorithmItems = dataPreparer.GetData();

        var solver = new AlgorithmImportanceSolver(algorithmItems);
        var mostImportantGoalsIds = solver.GetIdsMostImportantGoals();

        return mostImportantGoalsIds.Select(x => _context.Goals.FirstOrDefault(g => g.Id == x)).ToList();
    }
}