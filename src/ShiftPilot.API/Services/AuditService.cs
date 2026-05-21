using ShiftPilot.API.Models;

namespace ShiftPilot.API.Services;

public interface IAuditService
{
    Task LogActionAsync(int userId, string action, string details, string? changesBefore = null, string? changesAfter = null);
}

public class AuditLog
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Action { get; set; } = string.Empty;
    public string Details { get; set; } = string.Empty;
    public string? ChangesBefore { get; set; }
    public string? ChangesAfter { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}

public class AuditService : IAuditService
{
    private readonly ILogger<AuditService> _logger;

    public AuditService(ILogger<AuditService> logger)
    {
        _logger = logger;
    }

    public Task LogActionAsync(int userId, string action, string details, string? changesBefore = null, string? changesAfter = null)
    {
        var auditLog = new AuditLog
        {
            UserId = userId,
            Action = action,
            Details = details,
            ChangesBefore = changesBefore,
            ChangesAfter = changesAfter,
            Timestamp = DateTime.UtcNow
        };

        _logger.LogInformation(
            "AuditLog - User: {UserId}, Action: {Action}, Details: {Details}, Timestamp: {Timestamp}",
            auditLog.UserId, auditLog.Action, auditLog.Details, auditLog.Timestamp);

        return Task.CompletedTask;
    }
}
