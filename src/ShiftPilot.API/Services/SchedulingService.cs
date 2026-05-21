using ShiftPilot.API.Repositories;
using ShiftPilot.Core.Models;

namespace ShiftPilot.API.Services;

public class SchedulingService : ISchedulingService
{
    private readonly IShiftRepository _shiftRepository;
    private readonly IAvailabilityRepository _availabilityRepository;
    private readonly ISickLeaveRepository _sickLeaveRepository;

    public SchedulingService(
        IShiftRepository shiftRepository,
        IAvailabilityRepository availabilityRepository,
        ISickLeaveRepository sickLeaveRepository)
    {
        _shiftRepository = shiftRepository;
        _availabilityRepository = availabilityRepository;
        _sickLeaveRepository = sickLeaveRepository;
    }

    public async Task<IEnumerable<Shift>> GenerateOptimalScheduleAsync(DateTime weekStart, int employeeCount)
    {
        var weekEnd = weekStart.AddDays(7);
        var unassignedShifts = await _shiftRepository.GetUnassignedShiftsAsync();
        var availabilities = await _availabilityRepository.GetAvailabilitiesForDateRangeAsync(weekStart, weekEnd);

        var scheduledShifts = new List<Shift>();

        foreach (var shift in unassignedShifts.Where(s => s.StartTime >= weekStart && s.StartTime < weekEnd))
        {
            var availableEmployees = availabilities
                .Where(a => IsEmployeeAvailableForShift(a, shift))
                .GroupBy(a => a.UserId)
                .Select(g => g.Key)
                .ToList();

            if (availableEmployees.Any())
            {
                var selectedEmployee = availableEmployees.First();
                shift.AssignedUserId = selectedEmployee;
                shift.Status = ShiftStatus.Assigned;
                scheduledShifts.Add(shift);
            }
        }

        return scheduledShifts;
    }

    public async Task<Dictionary<int, List<Shift>>> GetConflictingShiftsAsync(int userId)
    {
        var userShifts = await _shiftRepository.GetShiftsByUserAsync(userId);
        var userSickLeaves = await _sickLeaveRepository.GetSickLeavesByUserAsync(userId);
        var conflicts = new Dictionary<int, List<Shift>>();

        foreach (var sickLeave in userSickLeaves.Where(sl => sl.Status == SickLeaveStatus.Approved))
        {
            var conflictingShifts = userShifts
                .Where(s => s.StartTime.Date >= sickLeave.StartDate.Date && s.StartTime.Date <= sickLeave.EndDate.Date)
                .ToList();

            if (conflictingShifts.Any())
            {
                conflicts[sickLeave.Id] = conflictingShifts;
            }
        }

        return conflicts;
    }

    public async Task<IEnumerable<Shift>> GetAvailableShiftsForUserAsync(int userId, DateTime startDate, DateTime endDate)
    {
        var allShifts = await _shiftRepository.GetAllShiftsAsync();
        var availabilities = await _availabilityRepository.GetAvailabilitiesByUserAsync(userId);
        var sickLeaves = await _sickLeaveRepository.GetSickLeavesByUserAsync(userId);

        var availableShifts = allShifts
            .Where(s => s.StartTime >= startDate && s.EndTime <= endDate && s.Status == ShiftStatus.Unassigned)
            .Where(s => !IsConflictWithSickLeave(s, sickLeaves))
            .Where(s => IsUserAvailableForShift(s, availabilities))
            .ToList();

        return availableShifts;
    }

    private bool IsEmployeeAvailableForShift(Availability availability, Shift shift)
    {
        return availability.Date.Date == shift.StartTime.Date &&
               availability.IsAvailable &&
               availability.StartTime <= TimeOnly.FromDateTime(shift.StartTime) &&
               availability.EndTime >= TimeOnly.FromDateTime(shift.EndTime);
    }

    private bool IsUserAvailableForShift(Shift shift, IEnumerable<Availability> availabilities)
    {
        return availabilities.Any(a =>
            a.Date.Date == shift.StartTime.Date &&
            a.IsAvailable &&
            a.StartTime <= TimeOnly.FromDateTime(shift.StartTime) &&
            a.EndTime >= TimeOnly.FromDateTime(shift.EndTime));
    }

    private bool IsConflictWithSickLeave(Shift shift, IEnumerable<SickLeave> sickLeaves)
    {
        return sickLeaves.Any(sl =>
            sl.Status == SickLeaveStatus.Approved &&
            shift.StartTime.Date >= sl.StartDate.Date &&
            shift.EndTime.Date <= sl.EndDate.Date);
    }
}
