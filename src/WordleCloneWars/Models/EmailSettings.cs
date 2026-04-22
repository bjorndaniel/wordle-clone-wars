namespace WordleCloneWars.Models;

public class EmailSettings
{
    public string SmtpHost { get; set; } = "mailcluster.loopia.se";
    public int SmtpPort { get; set; } = 587;
    public string? SmtpUser { get; set; }
    public string? SmtpPassword { get; set; }
    public string FromName { get; set; } = string.Empty;
    public string FromEmail { get; set; } = string.Empty;
}
