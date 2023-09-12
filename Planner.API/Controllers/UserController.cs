using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Planner.Models;
using Planner.Contracts.User;

namespace Planner.API.Controllers;

/// <summary>
/// Контроллер для работы с пользователями
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UsersController : ControllerBase
{
    private readonly DatabaseContext _context;
    private readonly ILogger<UsersController> _logger;

    /// <summary>
    /// Конструктор класса <see cref="UsersController" />
    /// </summary>
    /// <param name="context">Контекст базы данных</param>
    /// <param name="logger">Логгер</param>
    public UsersController(DatabaseContext context, ILogger<UsersController> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <summary>
    /// Получить список пользователей
    /// </summary>
    /// <response code="200">Список пользователей</response>
    /// <response code="401">Токен доступа истек</response>
    /// <response code="500">Ошибка сервера</response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(User))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult Get() 
    {
        return Ok(_context.Users.ToList());
    }

    /// <summary>
    /// Получить пользователя по идентификатору
    /// </summary>
    /// <param name="id">Идентификатор пользователя</param>
    /// <response code="200">Пользователь успешно добавлен</response>
    /// <response code="401">Токен доступа истек</response>
    /// <response code="404">Пользователь с такой почтой не найден</response>
    /// <response code="500">Ошибка сервера</response>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(User))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult Get([FromRoute] Guid id)
    {
        var user = _context.Users.FirstOrDefault(u => u.Id == id);
        return user is not null ? Ok(user) : NotFound("Пользователь не найден");
    }

    /// <summary>
    /// Добавить пользователя
    /// </summary>
    /// <param name="userAddDto">Данные пользователя</param>
    /// <response code="201">Пользователь успешно добавлен</response>
    /// <response code="400">Пользователь с такой почтой уже существует</response>
    /// <response code="401">Токен доступа истек</response>
    /// <response code="500">Ошибка сервера</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(User))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Add([FromBody] UserAddDto userAddDto)
    {
        if (_context.Users.Any(user => user.Email == userAddDto.Email))
            return BadRequest("Пользователь с такой почтой уже существует");

        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = userAddDto.Email,
            Created = DateTime.UtcNow,
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(Get), new { id = user.Id }, user);
    }

    /// <summary>
    /// Обновить пользователя
    /// </summary>
    /// <param name="id">Идентификатор пользователя</param>
    /// <param name="userUpdateDto">Данные пользователя</param>
    /// <response code="200">Пользователь успешно обновлен</response>
    /// <response code="400">Пользователь с такой почтой уже существует</response>
    /// <response code="401">Токен доступа истек</response>
    /// <response code="404">Пользователь не найден</response>
    /// <response code="500">Ошибка сервера</response>
    [HttpPatch("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(User))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UserUpdateDto userUpdateDto)
    {
        if (_context.Users.Any(user => user.Email == userUpdateDto.Email))
            return BadRequest("Пользователь с такой почтой уже существует");

        var user = _context.Users.FirstOrDefault(u => u.Id == id);
        if (user is null)
            return NotFound("Пользователь не найден");
            
        user.Email = userUpdateDto.Email;

        _context.Users.Update(user);
        await _context.SaveChangesAsync();
        return Get(user.Id);
    }

    /// <summary>
    /// Регистрация пользователя
    /// </summary>
    /// <param name="userRegisterDto"></param>
    /// <returns></returns>
/*    public async Task<IActionResult> Register([FromBody] UserRegisterDto userRegisterDto)
    {
        return Ok();
    }*/
    // Регистарция (шлет код, тикет, почту)
    // Пишет почту -> отправляет код ->
    // код подтверждает -> все ок - регаем, иначе - нет -> возвращаем токен
}
