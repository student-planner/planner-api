using Planner.Models;
using Planner.API.Options;
using MimeKit;
using MailKit.Net.Smtp;

namespace Planner.API.Services;

public class EmailSenderService
{
    private readonly SmtpClientOptions _smtpClientOptions;
    private readonly CodeTemplateOptions _codeTemplateOptions;

    /// <summary>
    /// Конструктор сервиса отправки кода подтверждения на почту
    /// </summary>
    /// <param name="smtpClientOptions">Параметры Smtp-клиента</param>
    /// <param name="templateOptions">Параметры сообщения</param>
    public EmailSenderService(SmtpClientOptions smtpClientOptions, CodeTemplateOptions templateOptions)
    {
        _smtpClientOptions = smtpClientOptions;
        _codeTemplateOptions = templateOptions;
    }

    /// <inheritdoc cref="IEmailCodeSender.Send(AuthTicket)" />
    public async Task<bool> Send(AuthTicket ticket)
    {
        var emailMessage = new MimeMessage();

        emailMessage.From.Add(new MailboxAddress(_codeTemplateOptions.From, _smtpClientOptions.Email));
        emailMessage.To.Add(new MailboxAddress("", ticket.Login));
        emailMessage.Subject = string.Format(_codeTemplateOptions.Subject, ticket.Code);
        emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
        {
            Text = string.Format(_codeTemplateOptions.Body, ticket.Code)
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
