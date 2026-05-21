using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShiftPilot.API.Extensions;
using ShiftPilot.API.Services;

namespace ShiftPilot.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class SchedulingController : ControllerBase
{
    private readonly ISchedulingService _schedulingService;

    public SchedulingController(ISchedulingService schedulingService)
    {
        _schedulingService = schedulingService;
    }

    [HttpPost("generate-weekly-schedule")]
    public async Task<IActionResult> GenerateWeeklySchedule([FromQuery] DateTime weekStart)
    {
        try
        {
            var schedule = await _schedulingService.GenerateOptimalScheduleAsync(weekStart, 10);
            return Ok(new { weekStart, shifts = schedule, count = schedule.Count() });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("conflicts/{userId}")]
    public async Task<IActionResult> GetConflicts(int userId)
    {
        try
        {
            var conflicts = await _schedulingService.GetConflictingShiftsAsync(userId);
            return Ok(conflicts);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("available-shifts/{userId}")]
    public async Task<IActionResult> GetAvailableShifts(int userId, [FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
    {
        try
        {
            var shifts = await _schedulingService.GetAvailableShiftsForUserAsync(userId, startDate, endDate);
            return Ok(new { startDate, endDate, shifts, count = shifts.Count() });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}
