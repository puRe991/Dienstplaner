using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShiftPilot.API.Services;
using ShiftPilot.Core.Models;

namespace ShiftPilot.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AvailabilityController : ControllerBase
{
    private readonly IAvailabilityService _availabilityService;

    public AvailabilityController(IAvailabilityService availabilityService)
    {
        _availabilityService = availabilityService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllAvailabilities()
    {
        var availabilities = await _availabilityService.GetAllAvailabilitiesAsync();
        return Ok(availabilities);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetAvailabilityById(int id)
    {
        var availability = await _availabilityService.GetAvailabilityByIdAsync(id);
        if (availability == null)
            return NotFound(new { message = "Availability not found" });

        return Ok(availability);
    }

    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetAvailabilitiesByUser(int userId)
    {
        var availabilities = await _availabilityService.GetAvailabilitiesByUserAsync(userId);
        return Ok(availabilities);
    }

    [HttpGet("range")]
    public async Task<IActionResult> GetAvailabilitiesForDateRange([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
    {
        var availabilities = await _availabilityService.GetAvailabilitiesForDateRangeAsync(startDate, endDate);
        return Ok(availabilities);
    }

    [HttpPost]
    public async Task<IActionResult> CreateAvailability([FromBody] Availability availability)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var createdAvailability = await _availabilityService.CreateAvailabilityAsync(availability);
        return CreatedAtAction(nameof(GetAvailabilityById), new { id = createdAvailability.Id }, createdAvailability);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAvailability(int id, [FromBody] Availability availability)
    {
        if (id != availability.Id)
            return BadRequest(new { message = "ID mismatch" });

        var existingAvailability = await _availabilityService.GetAvailabilityByIdAsync(id);
        if (existingAvailability == null)
            return NotFound(new { message = "Availability not found" });

        var updatedAvailability = await _availabilityService.UpdateAvailabilityAsync(availability);
        return Ok(updatedAvailability);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAvailability(int id)
    {
        var result = await _availabilityService.DeleteAvailabilityAsync(id);
        if (!result)
            return NotFound(new { message = "Availability not found" });

        return NoContent();
    }
}
