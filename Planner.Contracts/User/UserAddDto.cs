using System.ComponentModel.DataAnnotations;

namespace Planner.Contracts.User;

/// <summary>
/// Модель данных для добавления пользователя
/// </summary>
public class UserAddDto
{
    /// <summary>
    /// Электронная почта
    /// </summary>
    [Required(ErrorMessage = "Не указан адрес электронной почты")]
    public string Email { get; set; } = string.Empty;
}