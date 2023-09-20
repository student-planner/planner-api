using Planner.AlgorithmPriorityGoals;
using Planner.Models;
using TestApp;

var data = DataProvider.GetData()
    .Where(goal => !goal.SubGoalsIds.Any() && goal.Status != GoalStatus.Done && goal.Status != GoalStatus.Overdue)
    .ToList();

Console.WriteLine("Данные для планирования: ");
foreach (var goal in data)
{
    Console.WriteLine($"{goal.Id}\t{goal.Name}\t{goal.Priority}\t{goal.Deadline}");
}
Console.WriteLine();

Console.WriteLine("Запуск алгоритма планирования...");
var alg = new SchedulerAlgorithm(data);
var result = alg.Run();