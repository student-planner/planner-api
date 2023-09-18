using System.ComponentModel.DataAnnotations;

namespace Planner.Contracts.Auth;

/// <summary>
/// Модель для обновления токенов
/// </summary>
public class RefreshTokensDto
{
    /// <summary>
    /// Refresh-токен
    /// </summary>
    [Required(ErrorMessage = "Не указан токен обновления")]
    public string RefreshToken { get; set; } = string.Empty;
}
