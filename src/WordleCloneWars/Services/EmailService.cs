using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace WordleCloneWars.Services;

public class EmailService : IEmailSender
{
    private readonly ILogger _logger;
    private readonly EmailSettings _emailSettings;

    public EmailService(IOptions<EmailSettings> optionsAccessor, ILogger<EmailService> logger)
    {
        _emailSettings = optionsAccessor.Value;
        _logger = logger;
    }

    public async Task SendEmailAsync(string toEmail, string subject, string message)
    {
        if (string.IsNullOrEmpty(_emailSettings.SmtpUser) || string.IsNullOrEmpty(_emailSettings.SmtpPassword))
        {
            throw new InvalidOperationException("Missing SMTP credentials (EmailSettings:SmtpUser / SmtpPassword).");
        }

        var msg = new MimeMessage();
        msg.From.Add(new MailboxAddress(_emailSettings.FromName, _emailSettings.FromEmail));
        msg.To.Add(MailboxAddress.Parse(toEmail));
        msg.Subject = subject;
        msg.Body = new BodyBuilder { HtmlBody = message, TextBody = message }.ToMessageBody();

        using var client = new SmtpClient();
        try
        {
            await client.ConnectAsync(_emailSettings.SmtpHost, _emailSettings.SmtpPort, SecureSocketOptions.StartTls);
            await client.AuthenticateAsync(_emailSettings.SmtpUser, _emailSettings.SmtpPassword);
            await client.SendAsync(msg);
            _logger.LogInformation("Email to {ToEmail} sent via {Host}.", toEmail, _emailSettings.SmtpHost);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "SMTP failure sending to {ToEmail} via {Host} as {User} (from {From}).",
                toEmail, _emailSettings.SmtpHost, _emailSettings.SmtpUser, _emailSettings.FromEmail);
            throw;
        }
        finally
        {
            if (client.IsConnected)
            {
                await client.DisconnectAsync(true);
            }
        }
    }
}
