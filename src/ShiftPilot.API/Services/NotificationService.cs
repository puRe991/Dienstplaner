namespace ShiftPilot.API.Services;

public class NotificationService : INotificationService
{
    private readonly ILogger<NotificationService> _logger;

    public NotificationService(ILogger<NotificationService> logger)
    {
        _logger = logger;
    }

    public Task SendShiftAssignmentNotificationAsync(int userId, string shiftDetails)
    {
        _logger.LogInformation($"Sending shift assignment notification to user {userId}: {shiftDetails}");
        // TODO: Implement email/push notification logic
        return Task.CompletedTask;
    }

    public Task SendSickLeaveNotificationAsync(int userId, string message)
    {
        _logger.LogInformation($"Sending sick leave notification to user {userId}: {message}");
        // TODO: Implement email/push notification logic
        return Task.CompletedTask;
    }

    public Task SendSwapRequestNotificationAsync(int userId, string swapDetails)
    {
        _logger.LogInformation($"Sending swap request notification to user {userId}: {swapDetails}");
        // TODO: Implement email/push notification logic
        return Task.CompletedTask;
    }
}
