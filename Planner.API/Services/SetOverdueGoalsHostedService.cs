using Microsoft.EntityFrameworkCore;
using Planner.Models;

namespace Planner.API.Services;

/// <summary>
/// Сервис проверки статуса цели на истечение срока
/// </summary>
public class SetOverdueGoalsHostedService : IHostedService
{
    private const int ServiceDefaultDelay = 1;
    private readonly IServiceProvider _services;
    private readonly TimeSpan _period;
    private Timer _timer = null!;

    /// <summary>
    /// Конструктор класса <seealso cref="SetOverdueGoalsHostedService"/>
    /// </summary>
    /// <param name="configuration">Конфигурация</param>
    /// <param name="services">Поставщик сервисов</param>
    /// <exception cref="ArgumentException">Период времени проверки из конфигурации не получен</exception>
    /// <exception cref="ArgumentNullException">Проблемы с провайдером</exception>
    public SetOverdueGoalsHostedService(IConfiguration configuration, IServiceProvider services)
    {
        var delay = configuration.GetValue("OverdueGoalsServiceDelay", ServiceDefaultDelay);
        if (delay <= 0)
            throw new ArgumentException("Invalid delay value");

        _period = TimeSpan.FromHours(delay);
        _services = services ?? throw new ArgumentNullException(nameof(services));
    }

    /// <inheritdoc />
    public Task StartAsync(CancellationToken cancellationToken)
    {
        _timer = new Timer(SetOverdueGoals, null, TimeSpan.Zero, _period);
        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public Task StopAsync(CancellationToken cancellationToken)
    {
        _timer.Change(Timeout.Infinite, 0);
        return Task.CompletedTask;
    }

    private async void SetOverdueGoals(object? state)
    {
        using var scope = _services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
        var logger = _services.GetRequiredService<ILogger<SetOverdueGoalsHostedService>>();

        logger.LogInformation($"Background job (SetOverdueGoalsHostedService) started at {DateTime.Now}");

        try 
        {
            var goals = await context.Goals.Where(goal => goal.Deadline <= DateTime.UtcNow).ToListAsync();
            foreach (var goal in goals)
            {
                goal.Status = GoalStatus.Overdue;
                context.Update(goal);
            }
            await context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            logger.LogError(e, "Background service error");
        }
        
        logger.LogInformation($"Background job (SetOverdueGoalsHostedService) finished at {DateTime.Now}");
    }
}