using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Planner.API.Helpers;
using Planner.API.Options;
using Planner.API.Services;
using Planner.Contracts.Auth;
using Planner.Contracts.Register;
using Planner.Models;
using Seljmov.AspNet.Commons.Options;

namespace Planner.API.Controllers;

/// <summary>
/// Контроллер для работы с регистрацией
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class RegisterController : Controller
{
    private readonly DatabaseContext _context;
    private readonly ILogger<RegisterController> _logger;
    private readonly IOptions<JwtOptions> _jwtOptions;
    private readonly JwtHelper _jwtHelper;

    /// <summary>
    /// Конструктор класса <see cref="RegisterController" />
    /// </summary>
    /// <param name="context">Контекст базы данных</param>
    /// <param name="logger">Логгер</param>
    /// <param name="jwtOptions"></param>
    /// <param name="jwtHelper"></param>
    public RegisterController(DatabaseContext context, ILogger<RegisterController> logger, IOptions<JwtOptions> jwtOptions, JwtHelper jwtHelper)
    {
        _context = context;
        _logger = logger;
        _jwtHelper = jwtHelper;
        _jwtOptions = jwtOptions;
    }

    /// <summary>
    /// Регистрация пользователя
    /// </summary>
    /// <param name="registerStartDto">Данные для старта регистрации</param>
    /// <param name="emailCodeSender">Сервис отправки смс-кода по электронной почте</param>
    /// <response code="200">Регистрация успешно начата</response>
    /// <response code="400">Передан некорректный логин</response>
    /// <response code="404">Пользователь c таким логином уже существует</response>
    /// <response code="500">Ошибка сервера</response>
    /// <response code="501">Метод отправки смс-кода не реализован</response>
    [HttpPost("register/start")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TicketDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(AuthStartDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(User))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status501NotImplemented)]
    public async Task<IActionResult> RegisterStart([FromBody] RegisterStartDto registerStartDto, [FromServices] EmailSenderService emailCodeSender)
    {
        if (string.IsNullOrEmpty(registerStartDto.Email))
            return BadRequest($"Поле {nameof(registerStartDto.Email)} не может быть пустым");

        var user = await _context.Users.FirstOrDefaultAsync(c => c.Email == registerStartDto.Email);
        if (user is not null)
            return NotFound($"Пользователь с таким логином {registerStartDto.Email} уже существует");

        try
        {
            var ticket = AuthTicket.Create(registerStartDto.Email);
            await emailCodeSender.SendRegisterTicket(ticket);
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
    /// Завершает процесс регистрации. Проверяет тикет и секретный код. Возвращает токен доступа и обновления.
    /// </summary>
    /// <param name="completeDto">Данные для завершения регистрации</param>
    /// <response code="200">Регистрация успешно завершена</response>
    /// <response code="400">Передан некорректный тикет</response>
    /// <response code="404">Не найден тикет или клиент</response>
    /// <response code="409">Передан некорректный секретный код</response>
    /// <response code="500">Ошибка сервера</response>
    [HttpPost("register/complete")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TokensDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(AuthCompleteDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> RegisterComplete([FromBody] RegisterCompleteDto registerCompleteDto)
    {
        if (string.IsNullOrEmpty(registerCompleteDto.TicketId) ||
            !Guid.TryParse(registerCompleteDto.TicketId, out var ticketId) ||
            string.IsNullOrEmpty(registerCompleteDto.Code))
            return BadRequest($"Некорректный тикет или код");

        var ticket = _context.AuthTickets.FirstOrDefault(t => t.Id == ticketId);
        if (ticket is null)
            return NotFound($"Не найден тикет. TicketId: {ticketId}");

        if (ticket.Code != registerCompleteDto.Code)
            return Conflict($"Код не совпадает. Code: {registerCompleteDto.Code}");

        var newUser = new User
        {
            Email = registerCompleteDto.Email,
            Created = DateTime.UtcNow,
            RefreshToken = JwtHelper.CreateRefreshToken(),
            RefreshTokenExpires = DateTime.UtcNow.AddMinutes(_jwtOptions.Value.RefreshTokenLifetime)
        };
        await _context.Users.AddAsync(newUser);
        _context.AuthTickets.Remove(ticket);
        await _context.SaveChangesAsync();

        var tokens = new TokensDto
        {
            AccessToken = _jwtHelper.CreateAccessToken(newUser, _jwtOptions.Value.AccessTokenLifetime),
            RefreshToken = newUser.RefreshToken,
        };

        return Ok(tokens);
    }
}
