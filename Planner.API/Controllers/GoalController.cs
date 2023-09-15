using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Planner.API.Helpers;
using Planner.Contracts.Goal;
using Planner.Models;
using System.Security.Claims;

namespace Planner.API.Controllers;

/// <summary>
/// Контроллер для работы с задачами
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class GoalController : Controller
{
    private readonly DatabaseContext _context;
    private readonly JwtHelper _jwtHelper;

    /// <summary>
    /// Конструктор класса <see cref="GoalController"/>
    /// </summary>
    /// <param name="context"></param>
    /// <param name="jwtHelper"></param>
    public GoalController(DatabaseContext context, JwtHelper jwtHelper)
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
    [ProducesResponseType(StatusCodes.Status200OK, Type=typeof(List<GoalDto>))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Get()
    {
        var userInfo = GetAuthUserInfo();
        if (userInfo is null)
            return Unauthorized();

        return Ok(await _context.Goals.Where(x => x.UserId == userInfo.GuidId)
            .Select(item => new GoalDto
            {
                Id = item.Id,
                Deadline = item.Deadline,
                Description = item.Description,
                Labor = item.Labor,
                Name = item.Name,
                Priority = (int?)item.Priority
            }).ToListAsync());
    }

    /// <summary>
    /// Получить задачу
    /// </summary>
    /// <param name="id">Идентификатор задачи</param>
    /// <response code="200">Задач</response>
    /// <response code="401">Токен доступа истек</response>
    /// <response code="404">Задача не найдена</response>
    /// <response code="500">Ошибка сервера</response>
    [Route("{id:guid}"), HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GoalDto))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetGoalById([FromRoute] Guid id)
    {
        var userInfo = GetAuthUserInfo();
        if (userInfo is null)
            return Unauthorized();

        var goal = await _context.Goals.FirstOrDefaultAsync(x => x.Id == id);
        if (goal is null)
            return NotFound();

        var goalDto = new GoalDto
        {
            Id = goal.Id,
            Deadline = goal.Deadline,
            Priority = (int?)goal.Priority,
            Name = goal.Name,
            Labor = goal.Labor,
            Description = goal.Description
        };

        return Ok(goalDto);
    }

    /// <summary>
    /// Вставить задачу
    /// </summary>
    /// <param name="putDto">Модель данных для вставки задачи</param>
    /// <response code="201">Задача добавлена</response>
    /// <response code="204">Задача обновлена</response>
    /// <response code="400">Неверный входные данные</response>
    /// <response code="401">Токен доступа истек</response>
    /// <response code="404">Задача не найдена</response>
    /// <response code="500">Ошибка сервера</response>
    [HttpPut]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(GoalDto))]
    [ProducesResponseType(StatusCodes.Status204NoContent, Type = typeof(GoalDto))]
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
                Description = putDto.Description,
                Name = putDto.Name,
                UserId = userInfo.GuidId,
            };

            await _context.Goals.AddAsync(newGoal);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(Get), new { id = newGoal.Id }, newGoal);
        }
        
        var goal = await _context.Goals.FirstOrDefaultAsync(x => x.Id == putDto.Id);
        if (goal is null)
            return NotFound(putDto.Id);

        goal.Name = putDto.Name;
        goal.Description = putDto.Description;

        _context.Goals.Update(goal);
        await _context.SaveChangesAsync();

        return NoContent();
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

        var goal = await _context.Goals.FirstOrDefaultAsync(x => x.Id == id);
        if (goal is null)
            return NotFound();

        _context.Goals.Remove(goal);
        await _context.SaveChangesAsync();

        return NoContent();
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
