using System.ComponentModel.DataAnnotations;

namespace Planner.Contracts.User;

/// <summary>
/// Модель данных для регистрации пользователя
/// </summary>
public class UserRegisterDto
{
    /// <summary>
    /// Электронная почта
    /// </summary>
    [Required(ErrorMessage = "Не указан адрес электронной почты")]
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Код подтверждения
    /// </summary>
    [Required(ErrorMessage = "Не указан код подтверждения")]
    public string Code { get; set; } = string.Empty;

    /// <summary>
    /// Тикет
    /// </summary>
    public string TicketId { get; set; } = string.Empty;

    /// <summary>
    /// Информация об устройстве
    /// </summary>
    public string? DeviceDescription { get; set; } = string.Empty;
}
