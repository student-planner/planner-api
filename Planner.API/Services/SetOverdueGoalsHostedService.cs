using Microsoft.EntityFrameworkCore;
using Planner.API.Options;
using Planner.Models;

namespace Planner.API.Services;

/// <summary>
/// Сервис проверки статуса цели на истечение срока
/// </summary>
public class SetOverdueGoalsHostedService : IHostedService
{
    private const int ServiceDefaultDelay = 1;
    private readonly DatabaseContext _context;
    private readonly IServiceProvider _services;
    private readonly TimeSpan _period;
    private Timer _timer;

    /// <summary>
    /// Конструктор сервиса для проверки статуса цели
    /// </summary>
    /// <param name="configuration">Интерфейс конфигурации app</param>
    /// <param name="services">Провайдер app</param>
    /// <exception cref="ArgumentException">Период времени проверки из конфигурации не получено</exception>
    /// <exception cref="ArgumentNullException">Проблемы с провайдером</exception>
    public SetOverdueGoalsHostedService(IConfiguration configuration, IServiceProvider services)
    {
        var delay = configuration.GetValue("OverdueGoalsServiceDelay", ServiceDefaultDelay);
        if (delay <= 0)
            throw new ArgumentException("Invalid delay value");

        _period = TimeSpan.FromHours(delay);
        _services = services ?? throw new ArgumentNullException(nameof(services));
    }
    
    public Task StartAsync(CancellationToken cancellationToken)
    {
        _timer = new Timer(SetOverdueGoals, null, TimeSpan.Zero, _period);
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _timer?.Change(Timeout.Infinite, 0);
        return Task.CompletedTask;
    }

    private async void SetOverdueGoals(object state)
    {
        using var scope = _services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
        var logger = _services.GetRequiredService<ILogger<SetOverdueGoalsHostedService>>();

        logger.LogInformation($"Background job (SetOverdueGoalsHostedService) started at {DateTime.Now}");

        try 
        {
            var goals = await _context.Goals.Where(goal => goal.Deadline <= DateTime.UtcNow).ToListAsync();
            var tasks = goals.Select(goal => Task.Factory.StartNew(() =>
            {
                goal.Status = GoalStatus.Overdue;
                _context.Update(goal);
                _context.SaveChangesAsync();
            }));
            await Task.WhenAll(tasks);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Background service error");
        }
        logger.LogInformation($"Background job (SetOverdueGoalsHostedService) finished at {DateTime.Now}");
    }
}