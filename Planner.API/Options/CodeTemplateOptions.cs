namespace Planner.API.Options;

/// <summary>
/// Настройки сообщений для авторизации
/// </summary>
/// <param name="From">От кого</param>
/// <param name="Subject">Тема письма</param>
/// <param name="Body">Содержание письма</param>
public record CodeTemplateOptions(string From, string Subject, string Body);