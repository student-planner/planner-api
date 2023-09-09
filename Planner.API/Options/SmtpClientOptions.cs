namespace Planner.API.Options;

/// <summary>
/// Настройки SMTP-сервера
/// </summary>
public class SmtpClientOptions
{
    /// <summary>
    /// Хост SMTP-сервера
    /// </summary>
    public string Host { get; set; } = string.Empty;

    /// <summary>
    /// Порт SMTP-сервера
    /// </summary>
    public int Port { get; set; }

    /// <summary>
    /// Email-адрес отправителя
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Пароль отправителя
    /// </summary>
    public string Password { get; set; } = string.Empty;

    /// <summary>
    /// Поддерживается ли SSL
    /// </summary>
    public bool EnableSsl { get; set; }
}
