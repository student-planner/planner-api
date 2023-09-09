using System.ComponentModel.DataAnnotations;

namespace Planner.Contracts.User;

/// <summary>
/// Модель данных для обновления пользователя
/// </summary>
public class UserUpdateDto
{
    /// <summary>
    /// Электронная почта
    /// </summary>
    [Required(ErrorMessage = "Не указан адрес электронной почты")]
    public string Email { get; set; } = string.Empty;
}
