using System.Text.Json;
using Planner.AlgorithmPriorityGoals;
using Planner.Models;
using Xunit.Abstractions;

namespace Planner.GoalsPriorityAlgorithm.Tests;

public class ImportanceAlgorithmTest
{
    private readonly ITestOutputHelper _testOutputHelper;

    [Fact]
    public void Test1()
    {
        var goals = _testGoalsBase1
            .Where(goal => !goal.SubGoalsIds.Any() && goal.Status != GoalStatus.Done && goal.Status != GoalStatus.Overdue)
            .ToList();
        var alg = new ImportanceAlgorithm();
        var result = alg.Run(goals).ToList();

        var json = JsonSerializer.Serialize(result);
        _testOutputHelper.WriteLine(json);
    }

    private readonly List<Goal> _testGoalsBase1 = new()
    {
        new Goal()
        {
            Id = Guid.Parse("00000000-0000-0000-0000-000000000001"),
            Name = "ЛР №1 по БД",
            Description = "Установка БД",
            Deadline = DateTime.Parse("2023-09-24 10:00"),
            Labor = 60 * 60 * 2,
            Priority = GoalPriority.High
        },
        new Goal()
        {
            Id = Guid.Parse("00000000-0000-0000-0000-000000000002"),
            Name = "ЛР №2 по БД",
            Deadline = DateTime.Parse("2023-10-01 10:00"),
            Labor = 60 * 60 * 4,
            Priority = GoalPriority.Medium,
            DependGoalsIds = new List<Guid>()
            {
                Guid.Parse("00000000-0000-0000-0000-000000000001")
            }
        },
        new Goal
        {
            Id = Guid.Parse("00000000-0000-0000-0000-000000000003"),
            Name = "Написать устав для КП",
            Deadline = DateTime.Parse("2023-11-10 10:00"),
            Labor = 60 * 60 * 8,
            Priority = GoalPriority.Medium
        },
        new Goal()
        {
            Id = Guid.Parse("00000000-0000-0000-0000-000000000004"),
            Name = "ЛР №1 по Сетям",
            Deadline = DateTime.Parse("2023-10-04 10:00"),
            Labor = 60 * 60 * 4,
            Priority = GoalPriority.High
        },
        new Goal()
        {
            Id = Guid.Parse("00000000-0000-0000-0000-000000000005"),
            Name = "Пройти курс по АД",
            Deadline = DateTime.Parse("2023-12-25 10:00"),
            Priority = GoalPriority.High,
            SubGoalsIds = new List<Guid>()
            {
                Guid.Parse("00000000-0000-0000-0000-000000000013"),
                Guid.Parse("00000000-0000-0000-0000-000000000014"),
                Guid.Parse("00000000-0000-0000-0000-000000000016"),
                Guid.Parse("00000000-0000-0000-0000-000000000017"),
                Guid.Parse("00000000-0000-0000-0000-000000000018"),
                Guid.Parse("00000000-0000-0000-0000-000000000019"),
            }
        },
        new Goal()
        {
            Id = Guid.Parse("00000000-0000-0000-0000-000000000006"),
            Name = "Курсовой проект по БД",
            Deadline = DateTime.Parse("2023-12-25 10:00"),
            Priority = GoalPriority.ExtraHigh,
            SubGoalsIds = new List<Guid>()
            {
                Guid.Parse("00000000-0000-0000-0000-000000000007"),
                Guid.Parse("00000000-0000-0000-0000-000000000008"),
                Guid.Parse("00000000-0000-0000-0000-000000000009"),
                Guid.Parse("00000000-0000-0000-0000-000000000010"),
                Guid.Parse("00000000-0000-0000-0000-000000000011"),
            }
        },
        new Goal()
        {
            Id = Guid.Parse("00000000-0000-0000-0000-000000000007"),
            Name  = "Введение",
            Deadline = DateTime.Parse("2023-11-01 10:00"),
            Labor = 60 * 60 * 1.5,
            Priority = GoalPriority.High,
        },
        new Goal()
        {
            Id = Guid.Parse("00000000-0000-0000-0000-000000000008"),
            Name = "Технический проект",
            Deadline = DateTime.Parse("2023-11-15 10:00"),
            Labor = 60 * 60 * 3,
            Priority = GoalPriority.High,
        },
        new Goal()
        {
            Id = Guid.Parse("00000000-0000-0000-0000-000000000009"),
            Name = "Программный продукт",
            Deadline = DateTime.Parse("2023-12-02 10:00"),
            Labor = 60 * 60 * 4,
            Priority = GoalPriority.High,
        },
        new Goal()
        {
            Id = Guid.Parse("00000000-0000-0000-0000-000000000010"),
            Name = "Методика испытания и тестирования",
            Deadline = DateTime.Parse("2023-12-06 10:00"),
            Labor = 60 * 60 * 1,
            Priority = GoalPriority.High,
        },
        new Goal()
        {
            Id = Guid.Parse("00000000-0000-0000-0000-000000000011"),
            Name = "Заключение",
            Deadline = DateTime.Parse("2023-12-08 10:00"),
            Labor = 60 * 60 * 0.5,
            Priority = GoalPriority.High,
        },
        new Goal()
        {
            Id = Guid.Parse("00000000-0000-0000-0000-000000000012"),
            Name = "ЛР №3 по БД",
            Deadline = DateTime.Parse("2023-10-04 09:00"),
            Labor = 60 * 60 * 4,
            Priority = GoalPriority.High,
            DependGoalsIds = new List<Guid>()
            {
                Guid.Parse("00000000-0000-0000-0000-000000000002")
            },
        },
        new Goal()
        {
            Id = Guid.Parse("00000000-0000-0000-0000-000000000013"),
            Name = "ЛР №1 по АД",
            Description = "Установка Linux-сервера",
            Deadline = DateTime.Parse("2023-09-28 09:00"),
            Labor = 60 * 60 * 2,
            Priority = GoalPriority.High
        },
        new Goal()
        {
            Id = Guid.Parse("00000000-0000-0000-0000-000000000014"),
            Name = "ЛР №2 по АД",
            Deadline = DateTime.Parse("2023-10-05 09:00"),
            Labor = 60 * 60 * 4,
            Priority = GoalPriority.High,
            DependGoalsIds = new List<Guid>()
            {
                Guid.Parse("00000000-0000-0000-0000-000000000013"),
            }
        },
        new Goal()
        {
            Id = Guid.Parse("00000000-0000-0000-0000-000000000015"),
            Name = "ЛР №2 по Сетям",
            Deadline = DateTime.Parse("2023-10-10 09:00"),
            Labor = 60 * 60 * 4,
            Priority = GoalPriority.High,
            DependGoalsIds = new List<Guid>()
            {
                Guid.Parse("00000000-0000-0000-0000-000000000004"),
            },
        },
        new Goal()
        {
            Id = Guid.Parse("00000000-0000-0000-0000-000000000016"),
            Name = "ЛР №3 по АД",
            Deadline = DateTime.Parse("2023-10-15 09:00"),
            Labor = 60 * 60 * 2,
            Priority = GoalPriority.Medium,
            DependGoalsIds = new List<Guid>()
            {
                Guid.Parse("00000000-0000-0000-0000-000000000014"),
            }
        },
        new Goal()
        {
            Id = Guid.Parse("00000000-0000-0000-0000-000000000017"),
            Name = "ЛР №4 по АД",
            Deadline = DateTime.Parse("2023-10-25 09:00"),
            Labor = 60 * 60 * 2.5,
            Priority = GoalPriority.Medium,
            DependGoalsIds = new List<Guid>()
            {
                Guid.Parse("00000000-0000-0000-0000-000000000016"),
            }
        },
        new Goal()
        {
            Id = Guid.Parse("00000000-0000-0000-0000-000000000018"),
            Name = "ЛР №5 по АД",
            Deadline = DateTime.Parse("2023-11-05 09:00"),
            Labor = 60 * 60 * 3,
            Priority = GoalPriority.Medium,
            DependGoalsIds = new List<Guid>()
            {
                Guid.Parse("00000000-0000-0000-0000-000000000017"),
            }
        },
        new Goal()
        {
            Id = Guid.Parse("00000000-0000-0000-0000-000000000019"),
            Name = "ЛР №6 по АД",
            Deadline = DateTime.Parse("2023-11-15 09:00"),
            Labor = 60 * 60 * 2,
            Priority = GoalPriority.Medium,
            DependGoalsIds = new List<Guid>()
            {
                Guid.Parse("00000000-0000-0000-0000-000000000018"),
            }
        },
        new Goal()
        {
            Id = Guid.Parse("00000000-0000-0000-0000-000000000020"),
            Name = "ЛР №4 по БД",
            Deadline = DateTime.Parse("2023-10-11 09:00"),
            Labor = 60 * 60 * 4,
            Priority = GoalPriority.High,
            DependGoalsIds = new List<Guid>()
            {
                Guid.Parse("00000000-0000-0000-0000-000000000012")
            },
        },
        new Goal()
        {
            Id = Guid.Parse("00000000-0000-0000-0000-000000000021"),
            Name = "ЛР №5 по БД",
            Deadline = DateTime.Parse("2023-10-23 09:00"),
            Labor = 60 * 60 * 2,
            Priority = GoalPriority.High,
            DependGoalsIds = new List<Guid>()
            {
                Guid.Parse("00000000-0000-0000-0000-000000000020")
            },
        },
        new Goal()
        {
            Id = Guid.Parse("00000000-0000-0000-0000-000000000022"),
            Name = "ЛР №6 по БД",
            Deadline = DateTime.Parse("2023-11-02 09:00"),
            Labor = 60 * 60 * 5,
            Priority = GoalPriority.High,
            DependGoalsIds = new List<Guid>()
            {
                Guid.Parse("00000000-0000-0000-0000-000000000021")
            },
        },
        new Goal()
        {
            Id = Guid.Parse("00000000-0000-0000-0000-000000000023"),
            Name = "Пройти курс по Базам Данных",
            Deadline = DateTime.Parse("2023-12-25 10:00"),
            Priority = GoalPriority.High,
            DependGoalsIds = new List<Guid>()
            {
                Guid.Parse("00000000-0000-0000-0000-000000000001"),
                Guid.Parse("00000000-0000-0000-0000-000000000002"),
                Guid.Parse("00000000-0000-0000-0000-000000000012"),
                Guid.Parse("00000000-0000-0000-0000-000000000020"),
                Guid.Parse("00000000-0000-0000-0000-000000000021"),
                Guid.Parse("00000000-0000-0000-0000-000000000022"),
            },
        },
    };

    public ImportanceAlgorithmTest(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }
}