namespace Planner.Models;

/// <summary>
/// Тикет для авторизации пользователя через смс
/// </summary>
public class AuthTicket
{
    /// <summary>
    /// Идентификатор тикета
    /// </summary>
    public Guid Id { get; set; }
	
    /// <summary>
    /// Код подтверждения
    /// </summary>
    public string Code { get; set; } = string.Empty;
	
    /// <summary>
    /// Логин пользователя
    /// </summary>
    /// <remarks>Может быть номер телефона или адрес электронной почты</remarks>
    public string Login { get; set; } = string.Empty;
	
    /// <summary>
    /// Описание устройства
    /// </summary>
    public string? DeviceDescription { get; set; }
	
    /// <summary>
    /// Дата истечения тикета
    /// </summary>
    public DateTime ExpiresAt { get; set; }
	
    /// <summary>
    /// Создать тикет
    /// </summary>
    /// <param name="login">Логин пользователя</param>
    /// <param name="deviceDescription">Описание устройства</param>
    /// <returns>Тикет со случайным кодом</returns>
    public static AuthTicket Create(string login, string? deviceDescription = null)
    {
        return new AuthTicket
        {
            Id = Guid.NewGuid(),
            Code = new Random().Next(100000, 999999).ToString(),
            Login = login,
            DeviceDescription = deviceDescription,
            ExpiresAt = DateTime.UtcNow.AddMinutes(10)
        };
    }
}