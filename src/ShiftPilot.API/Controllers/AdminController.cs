using Microsoft.AspNetCore.Mvc;
using ShiftPilot.API.Extensions;
using ShiftPilot.API.Services;
using ShiftPilot.API.DTOs;
using ShiftPilot.Core.Models;
using ShiftPilot.API.Validators;

namespace ShiftPilot.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AdminController : ControllerBase
{
    private readonly IUserRepository _userRepository;
    private readonly IShiftService _shiftService;
    private readonly ISickLeaveService _sickLeaveService;
    private readonly ILogger<AdminController> _logger;

    public AdminController(
        IUserRepository userRepository,
        IShiftService shiftService,
        ISickLeaveService sickLeaveService,
        ILogger<AdminController> logger)
    {
        _userRepository = userRepository;
        _shiftService = shiftService;
        _sickLeaveService = sickLeaveService;
        _logger = logger;
    }

    [HttpGet("dashboard")]
    public async Task<IActionResult> GetDashboard()
    {
        try
        {
            var users = await _userRepository.GetAllUsersAsync();
            var allShifts = await _shiftService.GetAllShiftsAsync();
            var allSickLeaves = await _sickLeaveService.GetAllSickLeavesAsync();

            var dashboard = new
            {
                totalEmployees = users.Count(),
                totalShifts = allShifts.Count(),
                assignedShifts = allShifts.Count(s => s.Status == ShiftStatus.Assigned),
                unassignedShifts = allShifts.Count(s => s.Status == ShiftStatus.Unassigned),
                pendingSickLeaves = allSickLeaves.Count(sl => sl.Status == SickLeaveStatus.Pending),
                approvedSickLeaves = allSickLeaves.Count(sl => sl.Status == SickLeaveStatus.Approved),
                timestamp = DateTime.UtcNow
            };

            return Ok(dashboard);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving dashboard");
            return StatusCode(500, new { message = "Error retrieving dashboard" });
        }
    }

    [HttpGet("users/roles")]
    public async Task<IActionResult> GetUsersByRole([FromQuery] string role)
    {
        try
        {
            var allUsers = await _userRepository.GetAllUsersAsync();
            var filteredUsers = allUsers.Where(u => u.Role.ToString() == role).ToList();

            return Ok(filteredUsers);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error filtering users by role");
            return StatusCode(500, new { message = "Error filtering users" });
        }
    }

    [HttpPost("generate-report")]
    public async Task<IActionResult> GenerateReport([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
    {
        try
        {
            var shifts = await _shiftService.GetAllShiftsAsync();
            var shiftReport = shifts
                .Where(s => s.StartTime >= startDate && s.EndTime <= endDate)
                .GroupBy(s => s.Type)
                .Select(g => new
                {
                    type = g.Key.ToString(),
                    count = g.Count(),
                    assigned = g.Count(s => s.Status == ShiftStatus.Assigned)
                })
                .ToList();

            var report = new
            {
                period = new { startDate, endDate },
                shiftsByType = shiftReport,
                generatedAt = DateTime.UtcNow
            };

            return Ok(report);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating report");
            return StatusCode(500, new { message = "Error generating report" });
        }
    }
}
