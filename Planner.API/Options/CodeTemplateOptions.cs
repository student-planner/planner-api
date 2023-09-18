namespace Planner.API.Options;

/// <summary>
/// Настройки сообщений
/// </summary>
public class CodeTemplateOptions
{
    /// <summary>
    /// От кого
    /// </summary>
    public string From { get; set; } = string.Empty;
    
    /// <summary>
    /// Тема письма
    /// </summary>
    public string Subject { get; set; } = string.Empty;
    
    /// <summary>
    /// Содержание письма
    /// </summary>
    public string Body { get; set; } = string.Empty;
}