using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Planner.API.Helpers;
using Planner.Contracts.Goal;
using Planner.Models;
using System.Security.Claims;
using Planner.AlgorithmPriorityGoals;

namespace Planner.API.Controllers;

/// <summary>
/// Контроллер для работы с задачами
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class GoalsController : Controller
{
    private readonly DatabaseContext _context;
    private readonly JwtHelper _jwtHelper;

    /// <summary>
    /// Конструктор класса <see cref="GoalsController"/>
    /// </summary>
    /// <param name="context"></param>
    /// <param name="jwtHelper"></param>
    public GoalsController(DatabaseContext context, JwtHelper jwtHelper)
    {
        _context = context;
        _jwtHelper = jwtHelper;
    }

    /// <summary>
    /// Получить список задач
    /// </summary>
    /// <response code="200">Список задач</response>
    /// <response code="401">Токен доступа истек</response>
    /// <response code="500">Ошибка сервера</response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type=typeof(List<GoalImportantDto>))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Get()
    {
        var userInfo = GetAuthUserInfo();
        if (userInfo is null)
            return Unauthorized();

        var goals = await _context.Goals.Where(goal => goal.UserId == userInfo.GuidId)
            .Select(item => new GoalBaseDto
            {
                Id = item.Id,
                Name = item.Name,
                Description = item.Description,
                Deadline = item.Deadline,
                Status = item.Status,
            }).ToListAsync();
        
        goals.Sort((a, b) => ((int)a.Status).CompareTo((int)(b.Status)));
        return Ok(goals);
    }
    
    /// <summary>
    /// Получить задачу с подробной информацией
    /// </summary>
    /// <param name="id">Идентификатор задачи</param>
    /// <response code="200">Задача</response>
    /// <response code="401">Токен доступа истек</response>
    /// <response code="404">Задача не найдена</response>
    /// <response code="500">Ошибка сервера</response>
    [Route("{id:guid}"), HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GoalDetailedDto))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetDetailed([FromRoute] Guid id)
    {
        var userInfo = GetAuthUserInfo();
        if (userInfo is null)
            return Unauthorized();

        var goal = await _context.Goals.FirstOrDefaultAsync(goal => goal.Id == id);
        if (goal is null)
            return NotFound();

        var acceptedStatuses = new[] { GoalStatus.New, GoalStatus.InProgress };
        var subGoals =  !goal.SubGoalsIds.Any()
            ? new List<Goal>()
            : await _context.Goals
                .Where(item => goal.SubGoalsIds.Contains(item.Id) && acceptedStatuses.Contains(item.Status))
                .ToListAsync();
        
        var goalsById = await _context.Goals.ToDictionaryAsync(x => x.Id);
        var keys = goalsById.Keys;
        
        var goalDto = new GoalDetailedDto
        {
            Id = goal.Id,
            Name = goal.Name,
            Description = goal.Description,
            Deadline = goal.Deadline,
            Labor = CalculateLaborArithmeticMean(subGoals, goal.Labor),
            Priority = CalculatePrioritiesArithmeticMean(subGoals, goal.Priority),
            Status = goal.Status,
            SubGoals = goal.SubGoalsIds
                .Where(goalId => keys.Contains(goalId))
                .Select(goalId => goalsById[goalId])
                .Select(item => new GoalBaseDto
                {
                    Id = item.Id,
                    Name = item.Name,
                    Description = item.Description,
                    Deadline = item.Deadline,
                    Status = item.Status,
                }).ToList(),
            DependGoals = goal.DependGoalsIds
                .Where(goalId => keys.Contains(goalId))
                .Select(goalId => goalsById[goalId])
                .Select(item => new GoalBaseDto
                {
                    Id = item.Id,
                    Name = item.Name,
                    Description = item.Description,
                    Deadline = item.Deadline,
                    Status = item.Status,
                }).ToList(),
        };

        return Ok(goalDto);
    }
    
    private static double CalculateLaborArithmeticMean(IReadOnlyCollection<Goal> goals, double goalLabor)
    {
        if (goals.Count == 0) return goalLabor;
        if (goals.Count == 1) return goals.First().Labor;
        
        var laborSumma = goals.Sum(item => item.Labor);
        var laborCount = goals.Count;
        var laborArithmeticMean = laborSumma / laborCount;
        return laborArithmeticMean;
    }
    
    private static GoalPriority CalculatePrioritiesArithmeticMean(IReadOnlyCollection<Goal> goals, GoalPriority goalPriority)
    {
        if (goals.Count == 0) return goalPriority;
        if (goals.Count == 1) return goals.First().Priority;
        
        var prioritiesSumma = goals.Sum(item => (int)item.Priority);
        var prioritiesCount = goals.Count;
        var prioritiesArithmeticMean = prioritiesSumma / prioritiesCount;
        return prioritiesArithmeticMean switch
        {
            > 17 => GoalPriority.ExtraHigh,
            > 10 => GoalPriority.High,
            > 6 => GoalPriority.Medium,
            > 3 => GoalPriority.Low,
            _ => GoalPriority.ExtraLow
        };
    }
    
    /// <summary>
    /// Вставить задачу
    /// </summary>
    /// <param name="putDto">Модель данных для вставки задачи</param>
    /// <response code="201">Задача добавлена</response>
    /// <response code="204">Задача обновлена</response>
    /// <response code="400">Неверные входные данные</response>
    /// <response code="401">Токен доступа истек</response>
    /// <response code="404">Задача не найдена</response>
    /// <response code="500">Ошибка сервера</response>
    [HttpPut]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(GoalImportantDto))]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Put([FromBody] GoalPutDto putDto)
    {
        var userInfo = GetAuthUserInfo();
        if (userInfo is null)
            return Unauthorized();

        if (!ModelState.IsValid) return BadRequest(ModelState);
        
        if (putDto.Id is null || putDto.Id == Guid.Empty)
        {
            var newGoal = new Goal
            {
                Name = putDto.Name,
                Description = putDto.Description,
                Deadline = putDto.Deadline,
                Labor = putDto.Labor,
                Priority = putDto.Priority,
                Status = GoalStatus.New,
                SubGoalsIds = putDto.SubGoalsIds,
                DependGoalsIds = putDto.DependGoalsIds,
                UserId = userInfo.GuidId,
            };

            await _context.Goals.AddAsync(newGoal);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(Get), new { id = newGoal.Id }, newGoal);
        }
        
        var goal = await _context.Goals.FirstOrDefaultAsync(goal => goal.Id == putDto.Id);
        if (goal is null)
            return NotFound(putDto.Id);

        goal.Name = putDto.Name;
        goal.Description = putDto.Description;
        goal.Deadline = putDto.Deadline;
        goal.Labor = putDto.Labor;
        goal.Priority = putDto.Priority;
        goal.Status = putDto.Status;
        goal.SubGoalsIds = putDto.SubGoalsIds;
        goal.DependGoalsIds = putDto.DependGoalsIds;

        _context.Goals.Update(goal);
        await _context.SaveChangesAsync();

        await UpdateParentGoal(goal);
        
        return NoContent();
    }

    private async Task UpdateParentGoal(Goal goal)
    {
        var parents = await _context.Goals
            .Where(item => item.Status != GoalStatus.Overdue && item.SubGoalsIds.Contains(goal.Id))
            .ToListAsync();

        foreach (var parent in parents)
        {
            if (goal.Status != GoalStatus.New)
            {
                parent.Status = GoalStatus.InProgress;
            }
            
            if (goal.Status == GoalStatus.Done && parent.SubGoalsIds.Count == 1)
            {
                parent.Status = GoalStatus.Done;
            }
            
            _context.Goals.Update(parent);
        }
        
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Удалить задачу
    /// </summary>
    /// <param name="id">Идентификатор задачи</param>
    /// <response code="204">Задача удалена</response>
    /// <response code="401">Токен доступа истек</response>
    /// <response code="404">Задача не найдена</response>
    /// <response code="500">Ошибка сервера</response>
    [Route("{id:guid}"), HttpDelete]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        var userInfo = GetAuthUserInfo();
        if (userInfo is null)
            return Unauthorized();

        var goal = await _context.Goals.FirstOrDefaultAsync(goal => goal.Id == id);
        if (goal is null)
            return NotFound();

        // Удалить задачу из списков подзадач и зависимых задач
        var goals = await _context.Goals
            .Where(item => item.SubGoalsIds.Contains(id) || item.DependGoalsIds.Contains(id))
            .ToListAsync();
        
        foreach (var item in goals)
        {
            item.SubGoalsIds.Remove(id);
            item.DependGoalsIds.Remove(id);
            _context.Goals.Update(item);
        }
        
        _context.Goals.Remove(goal);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    /// <summary>
    /// Получить список наиболее важных задач на момент запроса
    /// </summary>
    /// <param name="algorithm">Алгоритм для вычисления приоритета задач</param>
    /// <response code="200">Список задач</response>
    /// <response code="401">Токен доступа истек</response>
    /// <response code="500">Ошибка сервера</response>
    /// <returns>Список задач</returns>
    [Route("important"), HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<GoalImportantDto>))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetImportant([FromServices] IImportanceAlgorithm algorithm)
    {
        var userInfo = GetAuthUserInfo();
        if (userInfo is null)
            return Unauthorized();
        
        var goals = await _context.Goals
            .Where(goal => goal.UserId == userInfo.GuidId && !goal.SubGoalsIds.Any() && goal.Status != GoalStatus.Overdue && goal.Status != GoalStatus.Done)
            .ToListAsync();
        
        var algorithmTask = Task.Run(() => algorithm.Run(goals));
        await algorithmTask;
        var result = algorithmTask.Result;
        
        return Ok(result.Select(goal => new GoalImportantDto
        {
            Id = goal.Id,
            Name = goal.Name,
            Description = goal.Description,
            Deadline = goal.Deadline,
            Labor = goal.Labor,
            Priority = goal.Priority,
            Status = goal.Status,
        }));
    }
    
    #region Claims

    private AuthUserInfo? GetAuthUserInfo()
    {
        string? authHeader = Request.Headers["Authorization"];
        var token = authHeader?.Replace("Bearer ", "") ?? throw new ArgumentNullException($"Bearer token not found");

        _ = _jwtHelper.ReadAccessToken(token, out var claims, out var validTo);
        if (claims is null) return null;

        var userInfo = new AuthUserInfo(
            Id: claims.Claims.FirstOrDefault(a => a.Type == ClaimsIdentity.DefaultIssuer)?.Value ?? throw new ArgumentNullException($"User's id from bearer token not found"),
            Email: claims.Claims.FirstOrDefault(a => a.Type == ClaimTypes.Email)?.Value ?? throw new ArgumentNullException($"User's email from bearer token not found")
        );

        return userInfo;
    }

    private record AuthUserInfo(string Id, string Email)
    {
        public Guid GuidId => Guid.TryParse(Id, out var guidId) ? guidId : throw new ArgumentNullException();
    }

    #endregion
}
