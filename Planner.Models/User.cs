namespace Planner.Models;

/// <summary>
/// Модель пользователя
/// </summary>
public class User
{
    /// <summary>
    /// Идентификатор
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    /// Адрес электронной почты
    /// </summary>
    public string Email { get; set; } = string.Empty;
    
    /// <summary>
    /// Токен обновления
    /// </summary>
    public string? RefreshToken { get; set; }
    
    /// <summary>
    /// Дата истечения токена обновления
    /// </summary>
    public DateTime? RefreshTokenExpires { get; set; }
    
    /// <summary>
    /// Дата создания
    /// </summary>
    public DateTime Created { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Коллекция задач
    /// </summary>
    public virtual ICollection<Goal> Goals { get; set; } = new List<Goal>();

    /// <summary>
    /// Обновить токен обновления
    /// </summary>
    /// <param name="refreshToken">Токен обновления</param>
    /// <param name="refreshTokenExpires">Дата истечения токена обновления</param>
    public void UpdateRefreshToken(string refreshToken, DateTime refreshTokenExpires)
    {
        RefreshToken = refreshToken;
        RefreshTokenExpires = refreshTokenExpires;
    }
    
    /// <summary>
    /// Удалить токен обновления
    /// </summary>
    public void ClearRefreshToken()
    {
        RefreshToken = null;
        RefreshTokenExpires = null;
    }
}