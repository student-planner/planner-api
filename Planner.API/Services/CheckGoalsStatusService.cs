using Microsoft.EntityFrameworkCore;
using Planner.API.Options;
using Planner.Models;

namespace Planner.API.Services;

public class CheckGoalsStatusService : IHostedService, IDisposable
{
    private ILogger<CheckGoalsStatusService> _logger;
    private readonly DatabaseContext _context;
    private const int ServiceDefaultDelay = 1;
    private int _delay;
    private Timer? _timer = null;

    public CheckGoalsStatusService(ILogger<CheckGoalsStatusService> logger, IServiceScopeFactory factory, IConfiguration configuration)
    {
        _logger = logger;
        _context = factory.CreateScope().ServiceProvider.GetRequiredService<DatabaseContext>();
        _delay = configuration.GetValue("ServiceDefaultDelay", ServiceDefaultDelay);
        if (_delay <= 0)
            throw new ArgumentException("Invalid delay value");
    }
    
    public Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("{time}: Goal checking service has started.", DateTime.UtcNow);
        _timer = new Timer(CheckStatus, null, TimeSpan.Zero,
            TimeSpan.FromHours(_delay));

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("{time}: Goal checking service stopped.", DateTime.UtcNow);
        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _timer?.Dispose();
        _logger.LogInformation("{time}: Goal service has freed up resources.", DateTime.UtcNow);
    }

    private void CheckStatus(object state)
    {
        _logger.LogInformation("{time}: Goal service started checking.", DateTime.UtcNow);
        var goals = _context.Goals.ToListAsync().Result;
        foreach (var goal in goals)
        {
            if (goal.Deadline < DateTime.UtcNow)
            {
                _logger.LogInformation($"{DateTime.UtcNow}: Goal {goal.Id} is overdue. Her status has been changed to {GoalStatus.Overdue}");
                goal.Status = GoalStatus.Overdue;
                _context.Goals.Update(goal);
                _context.SaveChangesAsync();
            }
        }
        _logger.LogInformation("{time}: All goals verified. Goal service has completed the check. Next check in {nextTime}.", DateTime.UtcNow, DateTime.UtcNow.AddHours(_delay));
    }
}