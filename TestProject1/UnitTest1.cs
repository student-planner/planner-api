using System.Text.Json;
using Planner.AlgorithmPriorityGoals;
using Planner.Models;
using Xunit.Abstractions;

namespace TestProject1;

public class UnitTest1
{
    private readonly ITestOutputHelper _testOutputHelper;

    [Fact]
    public void Test1()
    {
        var alg = new SchedulerAlgorithm(_testGoalsBase1);
        var result = alg.Run();
        var goalsImportant = result.
            Select(id => _testGoalsBase1.FirstOrDefault(x => x.Id == id)).ToList();
        var json = JsonSerializer.Serialize(goalsImportant);
        _testOutputHelper.WriteLine(json);
    }
    
    public List<Goal> _testGoalsBase1 = new List<Goal>()
    {
        new Goal()
        {
            Id = Guid.Parse("4b147382-429b-4585-aa35-a52ef6344c9c"),
            Name = "ЛР №1 по БД",
            Description = "Установка БД",
            Deadline = DateTime.Parse("2023-11-02 10:00"),
            Labor = 150,
            Priority = GoalPriority.High
        },
        new Goal()
        {
            Id = Guid.Parse("5b14b5b7-3b15-42a5-8c26-1125d6cbf692"),
            Name = "ЛР №2 по БД",
            Deadline = DateTime.Parse("2023-11-07 10:00"),
            Labor = 150,
            Priority = GoalPriority.High,
            DependGoalsIds = new List<Guid>()
            {
                Guid.Parse("4b147382-429b-4585-aa35-a52ef6344c9c")
            }
        },
        new Goal()
        {
            Id = Guid.Parse("43d96a54-561e-4645-b61b-f5de3ad49ab5"),
            Name = "Написать устав для КП",
            Deadline = DateTime.Parse("2023-11-10 10:00"),
            Labor = 150,
            Priority = GoalPriority.High
        },
        new Goal()
        {
            Id = Guid.Parse("898ac714-c39f-45ad-b173-f40b2c5e536b"),
            Name = "ЛР №1 по Сетям",
            Deadline = DateTime.Parse("2023-10-15 10:00"),
            Labor = 90,
            Priority = GoalPriority.ExtraHigh
        },
        new Goal()
        {
            Id = new Guid(),
            Name = "Пройти курс по АД",
            Deadline = DateTime.Parse("2023-12-25 10:00"),
            SubGoalsIds = new List<Guid>()
            {
                Guid.Parse("e8bf162e-a200-4260-8fd0-24d532cca59e"),
                Guid.Parse("572a8d83-b7d8-432c-9078-d28487099622")
            }
        },
        new Goal()
        {
            Id = Guid.Parse("e8bf162e-a200-4260-8fd0-24d532cca59e"),
            Name = "ЛР №1 по АД",
            Description = "Установка Linux-сервера",
            Deadline = DateTime.Parse("2023-10-05 09:00"),
            Labor = 60,
            Priority = GoalPriority.Low
        },
        new Goal()
        {
            Id = Guid.Parse("572a8d83-b7d8-432c-9078-d28487099622"),
            Name = "ЛР №2 по АД",
            Deadline = DateTime.Parse("2023-10-05 09:00"),
            Labor = 60,
            Priority = GoalPriority.Low,
            DependGoalsIds = new List<Guid>()
            {
                Guid.Parse("e8bf162e-a200-4260-8fd0-24d532cca59e"),
            }
        },
        new Goal()
        {
            Id = Guid.Parse("22ca517a-36a4-4fa9-a451-d397d84d5425"),
            Name = "Курсовой проект по БД",
            Deadline = DateTime.Parse("2023-12-25 10:00"),
            SubGoalsIds = new List<Guid>()
            {
                Guid.Parse("36bd7188-85f9-4d70-93bf-e72f38cbd9d0"),
                Guid.Parse("2a663381-553f-47d4-96ae-1516c259308b"),
                Guid.Parse("d83f908b-2cb1-49fd-a9a5-d893ab8941b9"),
                Guid.Parse("bb07aaa4-caaa-46fe-87aa-e9b0638118b4"),
                Guid.Parse("5af5e75c-25ac-4c54-9717-0d655859a0f5")
            }
        },
        new Goal()
        {
            Id = Guid.Parse("36bd7188-85f9-4d70-93bf-e72f38cbd9d0"),
            Name  = "Введение",
            Deadline = DateTime.Parse("2023-11-10 10:00"),
            Labor = 180,
            Priority = GoalPriority.Low,
        },
        new Goal()
        {
            Id = Guid.Parse("2a663381-553f-47d4-96ae-1516c259308b"),
            Name = "Технический проект",
            Deadline = DateTime.Parse("2023-11-20 10:00"),
            Labor = 300,
            Priority = GoalPriority.Low,
        },
        new Goal()
        {
            Id = Guid.Parse("d83f908b-2cb1-49fd-a9a5-d893ab8941b9"),
            Name = "Программный продукт",
            Deadline = DateTime.Parse("2023-12-03 10:00"),
            Labor = 480,
            Priority = GoalPriority.Low,
        },
        new Goal()
        {
            Id = Guid.Parse("bb07aaa4-caaa-46fe-87aa-e9b0638118b4"),
            Name = "Методика испытания и тестирования",
            Deadline = DateTime.Parse("2023-12-06 10:00"),
            Labor = 120,
            Priority = GoalPriority.Low,
        },
        new Goal()
        {
            Id = Guid.Parse("5af5e75c-25ac-4c54-9717-0d655859a0f5"),
            Name = "Заключение",
            Deadline = DateTime.Parse("2023-12-08 10:00"),
            Labor = 60,
            Priority = GoalPriority.Low,
        },
        new Goal()
        {
            Id = Guid.Parse("4cb1bada-24e4-4634-90e4-3a54deb3fa8a"),
            Name = "ЛР №2 по Сетям",
            Deadline = DateTime.Parse("2023-10-03 09:00"),
            Labor = 360,
            Priority = GoalPriority.High,
            DependGoalsIds = new List<Guid>()
            {
                Guid.Parse("898ac714-c39f-45ad-b173-f40b2c5e536b")
            },
        },
        new Goal()
        {
            Id = Guid.Parse("e76ac903-3baf-47d7-a598-d7605a256885"),
            Name = "ЛР №3 по БД",
            Deadline = DateTime.Parse("2023-10-04 09:00"),
            Labor = 240,
            Priority = GoalPriority.High,
            DependGoalsIds = new List<Guid>()
            {
                Guid.Parse("5b14b5b7-3b15-42a5-8c26-1125d6cbf692")
            },
        },
    };

    public UnitTest1(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }
}