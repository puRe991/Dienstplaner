using ShiftPilot.Core.Models;

namespace ShiftPilot.API.Services;

public interface IReportService
{
    Task<IEnumerable<ShiftReport>> GenerateShiftReportAsync(DateTime startDate, DateTime endDate);
    Task<IEnumerable<SickLeaveReport>> GenerateSickLeaveReportAsync(DateTime startDate, DateTime endDate);
    Task<EmployeePerformanceReport> GenerateEmployeePerformanceReportAsync(int userId, DateTime startDate, DateTime endDate);
}

public class ShiftReport
{
    public string ShiftType { get; set; } = string.Empty;
    public int TotalShifts { get; set; }
    public int AssignedShifts { get; set; }
    public int UnassignedShifts { get; set; }
    public double AssignmentRate { get; set; }
}

public class SickLeaveReport
{
    public string Status { get; set; } = string.Empty;
    public int Count { get; set; }
    public int TotalDays { get; set; }
}

public class EmployeePerformanceReport
{
    public int UserId { get; set; }
    public string EmployeeName { get; set; } = string.Empty;
    public int TotalShiftsWorked { get; set; }
    public int TotalSwapRequests { get; set; }
    public int ApprovedSwaps { get; set; }
    public int SickDays { get; set; }
    public double Reliability { get; set; }
}
