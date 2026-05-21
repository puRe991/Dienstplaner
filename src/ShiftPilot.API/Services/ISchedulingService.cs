using ShiftPilot.API.Repositories;
using ShiftPilot.Core.Models;

namespace ShiftPilot.API.Services;

public interface ISchedulingService
{
    Task<IEnumerable<Shift>> GenerateOptimalScheduleAsync(DateTime weekStart, int employeeCount);
    Task<Dictionary<int, List<Shift>>> GetConflictingShiftsAsync(int userId);
    Task<IEnumerable<Shift>> GetAvailableShiftsForUserAsync(int userId, DateTime startDate, DateTime endDate);
}
