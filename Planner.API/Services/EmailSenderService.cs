using Planner.Models;
using Planner.API.Options;
using MimeKit;
using MailKit.Net.Smtp;

namespace Planner.API.Services;

/// <summary>
/// Сервис отправки кода подтверждения на почту
/// </summary>
public class EmailSenderService
{
    private readonly IConfiguration _configuration;
    private readonly SmtpClientOptions _smtpClientOptions;

    /// <summary>
    /// Конструктор сервиса отправки кода подтверждения на почту
    /// </summary>
    /// <param name="configuration">Конфигуратор</param>
    public EmailSenderService(IConfiguration configuration)
    {
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        _smtpClientOptions = _configuration.GetSection(nameof(SmtpClientOptions)).Get<SmtpClientOptions>() ?? throw new ArgumentNullException(nameof(_smtpClientOptions));
    }

    /// <summary>
    /// Отправляет код подтверждения авторизации
    /// </summary>
    /// <param name="ticket">Тикет</param>
    /// <returns>Успешность отправки</returns>
    /// <exception cref="ArgumentNullException">Не удалось получить настройки шаблона сообщения</exception>
    public async Task<bool> SendAuthTicket(AuthTicket ticket)
    {
        var codeTemplateOptions = _configuration.GetSection(nameof(CodeTemplateOptions));
        var authCodeTemplateOptions = codeTemplateOptions.GetSection("Auth").Get<CodeTemplateOptions>();
        if (authCodeTemplateOptions == null)
            throw new ArgumentNullException(nameof(authCodeTemplateOptions));
        
        return await _send(ticket, authCodeTemplateOptions);
    }
    
    /// <summary>
    /// Отправляет код подтверждения регистрации
    /// </summary>
    /// <param name="ticket">Тикет</param>
    /// <returns>Успешность отправки</returns>
    /// <exception cref="ArgumentNullException">Не удалось получить настройки шаблона сообщения</exception>
    public async Task<bool> SendRegisterTicket(AuthTicket ticket)
    {
        var codeTemplateOptions = _configuration.GetSection(nameof(CodeTemplateOptions));
        var registerCodeTemplateOptions = codeTemplateOptions.GetSection("Register").Get<CodeTemplateOptions>();
        if (registerCodeTemplateOptions == null)
            throw new ArgumentNullException(nameof(registerCodeTemplateOptions));
        
        return await _send(ticket, registerCodeTemplateOptions);
    }
    
    private async Task<bool> _send(AuthTicket ticket, CodeTemplateOptions templateOptions)
    {
        var emailMessage = new MimeMessage();

        emailMessage.From.Add(new MailboxAddress(templateOptions.From, _smtpClientOptions.Email));
        emailMessage.To.Add(new MailboxAddress("", ticket.Login));
        emailMessage.Subject = string.Format(templateOptions.Subject, ticket.Code);
        emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
        {
            Text = string.Format(templateOptions.Body, ticket.Code)
        };

        using var client = new SmtpClient();
        try
        {
            await client.ConnectAsync(_smtpClientOptions.Host, _smtpClientOptions.Port, _smtpClientOptions.EnableSsl);
            await client.AuthenticateAsync(_smtpClientOptions.Email, _smtpClientOptions.Password);
            await client.SendAsync(emailMessage);

            await client.DisconnectAsync(true);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new Exception("Ошибка отправки сообщения");
        }

        return true;
    }
}
