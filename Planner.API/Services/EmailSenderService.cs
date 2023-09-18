using Planner.Models;
using Planner.API.Options;
using MimeKit;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;

namespace Planner.API.Services;

/// <summary>
/// Сервис отправки кода подтверждения на почту
/// </summary>
public class EmailSenderService
{
    private readonly SmtpClientOptions _smtpClientOptions;
    private readonly CodeTemplateOptions _templateOptions;

    /// <summary>
    /// Конструктор сервиса отправки кода подтверждения на почту
    /// </summary>
    /// <param name="smtpClientOptions">Параметры Smtp-клиента</param>
    /// <param name="templateOptions">Параметры сообщения</param>
    public EmailSenderService(IOptions<SmtpClientOptions> smtpClientOptions, IOptions<CodeTemplateOptions> templateOptions)
    {
        _smtpClientOptions = smtpClientOptions.Value;
        _templateOptions = templateOptions.Value;
    }

    /// <summary>
    /// Отправляет код подтверждения на почту
    /// </summary>
    /// <param name="ticket">Тикет с кодом подтверждения</param>
    /// <returns>Успешность отправки</returns>
    /// <exception cref="Exception">Ошибка отправки сообщения</exception>
    public async Task Send(AuthTicket ticket)
    {
        var emailMessage = new MimeMessage();

        emailMessage.From.Add(new MailboxAddress(_templateOptions.From, _smtpClientOptions.Email));
        emailMessage.To.Add(new MailboxAddress("", ticket.Login));
        emailMessage.Subject = string.Format(_templateOptions.Subject, ticket.Code);
        emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
        {
            Text = string.Format(_templateOptions.Body, ticket.Code)
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
    }
}
