using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Planner.API;
using Planner.Models;

namespace Planner.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GoalController : Controller
{
    private readonly DatabaseContext _context;

    public GoalController(DatabaseContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Получить список задач
    /// </summary>
    /// <response code="200">Список задач</response>
    /// <response code="401">Токен доступа истек</response>
    /// <response code="500">Ошибка сервера</response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type =  typeof(Goal))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult Get()
    {

        return Ok(_context.Goals.ToList());
    }
}
