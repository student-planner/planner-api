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
    /// Описание устройства
    /// </summary>
    public string? DeviceDescription { get; set; }
    
    /// <summary>
    /// Коллекция задач
    /// </summary>
    public virtual ICollection<Goal> Goals { get; set; } = new List<Goal>();
}