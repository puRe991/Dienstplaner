using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShiftPilot.API.Services;

namespace ShiftPilot.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ReportsController : ControllerBase
{
    private readonly IReportService _reportService;
    private readonly ILogger<ReportsController> _logger;

    public ReportsController(IReportService reportService, ILogger<ReportsController> logger)
    {
        _reportService = reportService;
        _logger = logger;
    }

    [HttpGet("shifts")]
    public async Task<IActionResult> GetShiftReport([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
    {
        try
        {
            if (startDate >= endDate)
                return BadRequest(new { message = "Start date must be before end date" });

            var report = await _reportService.GenerateShiftReportAsync(startDate, endDate);
            return Ok(new { period = new { startDate, endDate }, report });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating shift report");
            return StatusCode(500, new { message = "Error generating report" });
        }
    }

    [HttpGet("sick-leaves")]
    public async Task<IActionResult> GetSickLeaveReport([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
    {
        try
        {
            if (startDate >= endDate)
                return BadRequest(new { message = "Start date must be before end date" });

            var report = await _reportService.GenerateSickLeaveReportAsync(startDate, endDate);
            return Ok(new { period = new { startDate, endDate }, report });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating sick leave report");
            return StatusCode(500, new { message = "Error generating report" });
        }
    }

    [HttpGet("employee-performance/{userId}")]
    public async Task<IActionResult> GetEmployeePerformance(int userId, [FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
    {
        try
        {
            if (startDate >= endDate)
                return BadRequest(new { message = "Start date must be before end date" });

            var report = await _reportService.GenerateEmployeePerformanceReportAsync(userId, startDate, endDate);
            return Ok(report);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating employee performance report");
            return StatusCode(500, new { message = "Error generating report" });
        }
    }
}
