namespace Domain.Options;

public class EmailSenderOptions
{
    public const string SectionName = "EmailSender";

    public string SmtpServer { get; set; } = string.Empty;
    public int SmtpPort { get; set; }
    public string From { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public TimeSpan TokenLifetime { get; set; } = TimeSpan.FromMinutes(10);
}