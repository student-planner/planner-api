using System.ComponentModel.DataAnnotations;

namespace Planner.Contracts.Auth;

/// <summary>
/// Модель данных для завершения регистрации
/// </summary>
public class AuthCompleteDto
{
    /// <summary>
    /// Тикет
    /// </summary>
    [Required(ErrorMessage = "Не указан тикет")]
    public string Email { get; set; } = string.Empty;
    
    /// <summary>
    /// Тикет
    /// </summary>
    [Required(ErrorMessage = "Не указан тикет")]
    public string TicketId { get; set; } = string.Empty;

    /// <summary>
    /// Код подтверждения
    /// </summary>
    [Required(ErrorMessage = "Не указан код подтверждения")]
    public string Code { get; set; } = string.Empty;
}