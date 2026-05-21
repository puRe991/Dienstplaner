namespace ShiftPilot.API.Services;

public interface INotificationService
{
    Task SendShiftAssignmentNotificationAsync(int userId, string shiftDetails);
    Task SendSickLeaveNotificationAsync(int userId, string message);
    Task SendSwapRequestNotificationAsync(int userId, string swapDetails);
}
