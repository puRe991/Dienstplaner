using ShiftPilot.API.Repositories;
using ShiftPilot.Core.Models;

namespace ShiftPilot.API.Services;

public class ReportService : IReportService
{
    private readonly IShiftRepository _shiftRepository;
    private readonly ISickLeaveRepository _sickLeaveRepository;
    private readonly IShiftSwapRepository _swapRepository;
    private readonly IUserRepository _userRepository;

    public ReportService(
        IShiftRepository shiftRepository,
        ISickLeaveRepository sickLeaveRepository,
        IShiftSwapRepository swapRepository,
        IUserRepository userRepository)
    {
        _shiftRepository = shiftRepository;
        _sickLeaveRepository = sickLeaveRepository;
        _swapRepository = swapRepository;
        _userRepository = userRepository;
    }

    public async Task<IEnumerable<ShiftReport>> GenerateShiftReportAsync(DateTime startDate, DateTime endDate)
    {
        var shifts = await _shiftRepository.GetAllShiftsAsync();
        var periodShifts = shifts.Where(s => s.StartTime >= startDate && s.EndTime <= endDate);

        var report = periodShifts
            .GroupBy(s => s.Type)
            .Select(g => new ShiftReport
            {
                ShiftType = g.Key.ToString(),
                TotalShifts = g.Count(),
                AssignedShifts = g.Count(s => s.Status == ShiftStatus.Assigned),
                UnassignedShifts = g.Count(s => s.Status == ShiftStatus.Unassigned),
                AssignmentRate = g.Count(s => s.Status == ShiftStatus.Assigned) / (double)g.Count() * 100
            })
            .ToList();

        return report;
    }

    public async Task<IEnumerable<SickLeaveReport>> GenerateSickLeaveReportAsync(DateTime startDate, DateTime endDate)
    {
        var sickLeaves = await _sickLeaveRepository.GetAllSickLeavesAsync();
        var periodSickLeaves = sickLeaves.Where(sl => sl.StartDate >= startDate && sl.EndDate <= endDate);

        var report = periodSickLeaves
            .GroupBy(sl => sl.Status)
            .Select(g => new SickLeaveReport
            {
                Status = g.Key.ToString(),
                Count = g.Count(),
                TotalDays = (int)g.Sum(sl => (sl.EndDate - sl.StartDate).TotalDays)
            })
            .ToList();

        return report;
    }

    public async Task<EmployeePerformanceReport> GenerateEmployeePerformanceReportAsync(int userId, DateTime startDate, DateTime endDate)
    {
        var user = await _userRepository.GetUserByIdAsync(userId);
        if (user == null)
            throw new KeyNotFoundException($"User with ID {userId} not found");

        var employeeShifts = await _shiftRepository.GetShiftsByUserAsync(userId);
        var periodShifts = employeeShifts.Where(s => s.StartTime >= startDate && s.EndTime <= endDate);
        var employeeSickLeaves = await _sickLeaveRepository.GetSickLeavesByUserAsync(userId);
        var employeeSwaps = await _swapRepository.GetAllSwapRequestsAsync();
        var employeeSwapsFiltered = employeeSwaps.Where(s => (s.InitiatorId == userId || s.TargetUserId == userId));

        var totalSickDays = (int)employeeSickLeaves
            .Where(sl => sl.Status == SickLeaveStatus.Approved && sl.StartDate >= startDate && sl.EndDate <= endDate)
            .Sum(sl => (sl.EndDate - sl.StartDate).TotalDays);

        var reliability = (periodShifts.Count() - totalSickDays) / (double)(periodShifts.Count() + totalSickDays) * 100;

        return new EmployeePerformanceReport
        {
            UserId = user.Id,
            EmployeeName = $"{user.FirstName} {user.LastName}",
            TotalShiftsWorked = periodShifts.Count(),
            TotalSwapRequests = employeeSwapsFiltered.Count(),
            ApprovedSwaps = employeeSwapsFiltered.Count(s => s.Status == ShiftSwapStatus.Approved),
            SickDays = totalSickDays,
            Reliability = Math.Max(0, reliability)
        };
    }
}
