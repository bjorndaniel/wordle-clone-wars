using System.Net.Http.Headers;
using System.Net.Http.Json;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace WordleCloneWars.Services;

public class EmailService : IEmailSender
{
    private readonly HttpClient _httpClient;
    private readonly ILogger _logger;
    private readonly EmailSettings _emailSettings;

    public EmailService(HttpClient httpClient, IOptions<EmailSettings> optionsAccessor, ILogger<EmailService> logger)
    {
        _httpClient = httpClient;
        _emailSettings = optionsAccessor.Value;
        _logger = logger;
    }

    public async Task SendEmailAsync(string toEmail, string subject, string message)
    {
        if (string.IsNullOrEmpty(_emailSettings.ApiKey))
        {
            throw new InvalidOperationException("Missing Resend API key (EmailSettings:ApiKey).");
        }

        var from = string.IsNullOrWhiteSpace(_emailSettings.FromName)
            ? _emailSettings.FromEmail
            : $"{_emailSettings.FromName} <{_emailSettings.FromEmail}>";

        var payload = new
        {
            from,
            to = new[] { toEmail },
            subject,
            html = message
        };

        using var request = new HttpRequestMessage(HttpMethod.Post, "emails")
        {
            Content = JsonContent.Create(payload)
        };
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _emailSettings.ApiKey);

        var response = await _httpClient.SendAsync(request);
        var body = await response.Content.ReadAsStringAsync();

        if (response.IsSuccessStatusCode)
        {
            _logger.LogInformation("Email to {ToEmail} queued via Resend. Response: {Body}", toEmail, body);
        }
        else
        {
            _logger.LogError("Resend failure sending to {ToEmail}. Status: {Status}. From: {From}. Body: {Body}",
                toEmail, response.StatusCode, _emailSettings.FromEmail, body);
        }
    }
}
