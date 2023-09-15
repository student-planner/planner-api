namespace Planner.Contracts.Auth;

/// <summary>
/// Модель данных для старта авторизации
/// </summary>
public class AuthStartDto
{
    /// <summary>
    /// Email пользователя
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Информация об устройстве
    /// </summary>
    public string? DeviceDescription { get; set; } = string.Empty;
}