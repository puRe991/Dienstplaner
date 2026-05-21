namespace ShiftPilot.API.Services;

public interface IEmailService
{
    Task SendEmailAsync(string to, string subject, string body);
    Task SendShiftNotificationAsync(string userEmail, string shiftDetails);
    Task SendSickLeaveNotificationAsync(string userEmail, string status);
    Task SendSwapRequestNotificationAsync(string userEmail, string swapDetails);
}

public class EmailService : IEmailService
{
    private readonly ILogger<EmailService> _logger;
    private readonly IConfiguration _configuration;

    public EmailService(ILogger<EmailService> logger, IConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;
    }

    public Task SendEmailAsync(string to, string subject, string body)
    {
        // TODO: Implement actual email sending using SMTP
        _logger.LogInformation($"Email would be sent to {to} with subject: {subject}");
        return Task.CompletedTask;
    }

    public Task SendShiftNotificationAsync(string userEmail, string shiftDetails)
    {
        var subject = "New Shift Assignment";
        var body = $"You have been assigned a new shift:\n{shiftDetails}";
        return SendEmailAsync(userEmail, subject, body);
    }

    public Task SendSickLeaveNotificationAsync(string userEmail, string status)
    {
        var subject = "Sick Leave Status Update";
        var body = $"Your sick leave request has been {status}";
        return SendEmailAsync(userEmail, subject, body);
    }

    public Task SendSwapRequestNotificationAsync(string userEmail, string swapDetails)
    {
        var subject = "Shift Swap Request Notification";
        var body = $"There is a new shift swap request for you:\n{swapDetails}";
        return SendEmailAsync(userEmail, subject, body);
    }
}
