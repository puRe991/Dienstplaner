namespace ShiftPilot.API.Logging;

public interface ICustomLogger
{
    void LogAction(string userId, string action, string details);
    void LogError(string userId, string errorMessage, Exception? exception = null);
}

public class CustomLogger : ICustomLogger
{
    private readonly ILogger<CustomLogger> _logger;

    public CustomLogger(ILogger<CustomLogger> logger)
    {
        _logger = logger;
    }

    public void LogAction(string userId, string action, string details)
    {
        _logger.LogInformation($"User: {userId} | Action: {action} | Details: {details} | Timestamp: {DateTime.UtcNow:yyyy-MM-dd HH:mm:ss}");
    }

    public void LogError(string userId, string errorMessage, Exception? exception = null)
    {
        if (exception != null)
        {
            _logger.LogError(exception, $"User: {userId} | Error: {errorMessage} | Timestamp: {DateTime.UtcNow:yyyy-MM-dd HH:mm:ss}");
        }
        else
        {
            _logger.LogError($"User: {userId} | Error: {errorMessage} | Timestamp: {DateTime.UtcNow:yyyy-MM-dd HH:mm:ss}");
        }
    }
}
