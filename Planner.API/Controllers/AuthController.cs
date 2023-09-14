using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Planner.API.Helpers;
using Planner.API.Options;
using Planner.Models;
using Planner.API.Services;
using Planner.Contracts.Auth;
using Seljmov.AspNet.Commons.Options;

namespace Planner.API.Controllers;

/// <summary>
/// Контроллер для работы с авторизацией
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly DatabaseContext _context;
    private readonly IOptions<JwtOptions> _jwtOptions;
    private readonly JwtCreator _jwtCreator;
    private readonly ILogger<AuthController> _logger;

    /// <summary>
    /// Конструктор класса <see cref="AuthController"/>
    /// </summary>
    /// <param name="context">Контекст базы данных</param>
    /// <param name="jwtOptions">Настройки Jwt-токена</param>
    /// <param name="jwtCreator">Класс-создатель Jwt-токена</param>
    /// <param name="logger">Логгер</param>
    public AuthController(DatabaseContext context, IOptions<JwtOptions> jwtOptions, JwtCreator jwtCreator, ILogger<AuthController> logger)
    {
        _context = context;
        _jwtOptions = jwtOptions;
        _jwtCreator = jwtCreator;
        _logger = logger;
    }

    /// <summary>
    /// Начинает процесс авторизации. Отправляет секретный код по SMS. Возвращает тикет для завершения авторизации.
    /// </summary>
    /// <param name="startDto">Данные для старта авторизации</param>
    /// <param name="emailCodeSender">Сервис отправки смс-кода по электронной почте</param>
    /// <response code="200">Авторизация успешно начата</response>
    /// <response code="400">Передан некорректный логин</response>
    /// <response code="404">Пользователь c таким логином не найден</response>
    /// <response code="500">Ошибка сервера</response>
    /// <response code="501">Метод отправки смс-кода не реализован</response>
    [HttpPost("start")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TicketDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(AuthStartDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(User))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status501NotImplemented)]
    public async Task<IActionResult> AuthStart([FromBody] AuthStartDto startDto, [FromServices] EmailSenderService emailCodeSender)
    {
        if (string.IsNullOrEmpty(startDto.Email))
            return BadRequest($"Поле {nameof(startDto.Email)} не может быть пустым");

        var user = await _context.Users.FirstOrDefaultAsync(c => c.Email == startDto.Email);
        if (user is null)
            return NotFound($"Пользователь с логином {startDto.Email} не найден");

        try
        {
            var ticket = AuthTicket.Create(startDto.Email);
            await emailCodeSender.SendAuthTicket(ticket);
            await _context.AuthTickets.AddAsync(ticket);
            await _context.SaveChangesAsync();
            return Ok(new TicketDto
            {
                TicketId = ticket.Id.ToString()
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка отправки кода");
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    /// <summary>
    /// Завершает процесс авторизации. Проверяет тикет и секретный код. Возвращает токен доступа и обновления.
    /// </summary>
    /// <param name="completeDto">Данные для завершения авторизации</param>
    /// <response code="200">Авторизация успешно завершена</response>
    /// <response code="400">Передан некорректный тикет</response>
    /// <response code="404">Не найден тикет или клиент</response>
    /// <response code="409">Передан некорректный секретный код</response>
    /// <response code="500">Ошибка сервера</response>
    [HttpPost("complete")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TokensDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(AuthCompleteDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> AuthComplete([FromBody] AuthCompleteDto completeDto)
    {
        if (string.IsNullOrEmpty(completeDto.TicketId) ||
            !Guid.TryParse(completeDto.TicketId, out var ticketId) ||
            string.IsNullOrEmpty(completeDto.Code))
            return BadRequest($"Некорректный тикет или код");

        var ticket = _context.AuthTickets.FirstOrDefault(t => t.Id == ticketId);
        if (ticket is null)
            return NotFound($"Не найден тикет. TicketId: {ticketId}");

        if (ticket.Code != completeDto.Code)
            return Conflict($"Код не совпадает. Code: {completeDto.Code}");

        var user = _context.Users.FirstOrDefault(c => c.Email == ticket.Login);
        if (user is null)
            return NotFound($"Пользователь с логином {ticket.Login} не найден");

        user.RefreshToken = JwtCreator.CreateRefreshToken();
        user.RefreshTokenExpires = DateTime.UtcNow.AddMinutes(_jwtOptions.Value.RefreshTokenLifetime);
        _context.Users.Update(user);
        _context.AuthTickets.Remove(ticket);
        await _context.SaveChangesAsync();

        var tokens = new TokensDto
        {
            AccessToken = _jwtCreator.CreateAccessToken(user, _jwtOptions.Value.AccessTokenLifetime),
            RefreshToken = user.RefreshToken,
        };
        return Ok(tokens);
    }

    /// <summary>
    /// Обновляет токены с использованием валидного токена обновления. 
    /// </summary>
    /// <param name="refreshTokensDto">Данные токена обновления</param>
    /// <response code="200">Токены успешно обновлены</response>
    /// <response code="400">Передан некорректный токен обновления</response>
    /// <response code="401">Токен обновления устарел</response>
    /// <response code="500">Ошибка сервера</response>
    [HttpPost("refresh")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TokensDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(RefreshTokensDto))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> RefreshTokens([FromBody] RefreshTokensDto refreshTokensDto)
    {
        if (string.IsNullOrEmpty(refreshTokensDto.RefreshToken))
            return BadRequest($"Поле {nameof(refreshTokensDto.RefreshToken)} не может быть пустым");

        var user = _context.Users.FirstOrDefault(c => c.RefreshToken == refreshTokensDto.RefreshToken);
        if (user is null)
            return NotFound($"Пользователь с токеном обновления {refreshTokensDto.RefreshToken} не найден");

        if (user.RefreshTokenExpires < DateTime.UtcNow)
            return Unauthorized($"Токен обновления устарел");

        user.RefreshToken = JwtCreator.CreateRefreshToken();
        user.RefreshTokenExpires = DateTime.UtcNow.AddMinutes(_jwtOptions.Value.RefreshTokenLifetime);
        _context.Users.Update(user);
        await _context.SaveChangesAsync();

        var tokens = new TokensDto
        {
            AccessToken = _jwtCreator.CreateAccessToken(user, _jwtOptions.Value.AccessTokenLifetime),
            RefreshToken = user.RefreshToken,
        };
        return Ok(tokens);
    }
}