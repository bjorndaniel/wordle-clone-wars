
using SendGrid;
using SendGrid.Helpers.Mail;
using ILogger = Microsoft.Extensions.Logging.ILogger;
namespace WordleCloneWars.Services;

public class EmailService: IEmailSender
{
    private readonly ILogger _logger;
    private EmailSettings _emailSettings;

    public EmailService(IOptions<EmailSettings> optionsAccessor, ILogger<EmailService> logger)
    {
        _emailSettings = optionsAccessor.Value;
        _logger = logger;
    }

    public async Task SendEmailAsync(string toEmail, string subject, string message)
    {
        if (string.IsNullOrEmpty(_emailSettings.ApiKey))
        {
            throw new Exception("Null SendGridKey");
        }
        await Execute(_emailSettings.ApiKey, subject, message, toEmail);
    }

    private async Task Execute(string apiKey, string subject, string message, string toEmail)
    {
        var client = new SendGridClient(apiKey);
        var msg = new SendGridMessage()
        {
            From = new EmailAddress(_emailSettings.FromEmail, _emailSettings.FromName),
            Subject = subject,
            PlainTextContent = message,
            HtmlContent = message
        };
        msg.AddTo(new EmailAddress(toEmail));

        // Disable click tracking.
        // See https://sendgrid.com/docs/User_Guide/Settings/tracking.html
        msg.SetClickTracking(false, false);
        var response = await client.SendEmailAsync(msg);
        _logger.LogInformation(response.IsSuccessStatusCode 
            ? $"Email to {toEmail} queued successfully!"
            : $"Failure Email to {toEmail}");
    }
}