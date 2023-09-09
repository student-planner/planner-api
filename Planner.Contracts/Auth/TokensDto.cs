namespace Planner.Contracts.Auth;

/// <summary>
/// Модель токенов
/// </summary>
public class TokensDto
{
    /// <summary>
    /// Access-токен
    /// </summary>
    public string AccessToken { get; set; } = string.Empty;

    /// <summary>
    /// Refresh-токен
    /// </summary>
    public string RefreshToken { get; set; } = string.Empty;
}