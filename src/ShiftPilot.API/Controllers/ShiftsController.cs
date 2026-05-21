using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShiftPilot.API.Services;
using ShiftPilot.Core.Models;

namespace ShiftPilot.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ShiftsController : ControllerBase
{
    private readonly IShiftService _shiftService;

    public ShiftsController(IShiftService shiftService)
    {
        _shiftService = shiftService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllShifts()
    {
        var shifts = await _shiftService.GetAllShiftsAsync();
        return Ok(shifts);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetShiftById(int id)
    {
        var shift = await _shiftService.GetShiftByIdAsync(id);
        if (shift == null)
            return NotFound(new { message = "Shift not found" });

        return Ok(shift);
    }

    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetShiftsByUser(int userId)
    {
        var shifts = await _shiftService.GetShiftsByUserAsync(userId);
        return Ok(shifts);
    }

    [HttpGet("unassigned")]
    public async Task<IActionResult> GetUnassignedShifts()
    {
        var shifts = await _shiftService.GetUnassignedShiftsAsync();
        return Ok(shifts);
    }

    [HttpPost]
    public async Task<IActionResult> CreateShift([FromBody] Shift shift)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var createdShift = await _shiftService.CreateShiftAsync(shift);
        return CreatedAtAction(nameof(GetShiftById), new { id = createdShift.Id }, createdShift);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateShift(int id, [FromBody] Shift shift)
    {
        if (id != shift.Id)
            return BadRequest(new { message = "ID mismatch" });

        var existingShift = await _shiftService.GetShiftByIdAsync(id);
        if (existingShift == null)
            return NotFound(new { message = "Shift not found" });

        var updatedShift = await _shiftService.UpdateShiftAsync(shift);
        return Ok(updatedShift);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteShift(int id)
    {
        var result = await _shiftService.DeleteShiftAsync(id);
        if (!result)
            return NotFound(new { message = "Shift not found" });

        return NoContent();
    }

    [HttpPost("{id}/assign/{userId}")]
    public async Task<IActionResult> AssignShift(int id, int userId)
    {
        var result = await _shiftService.AssignShiftAsync(id, userId);
        if (!result)
            return NotFound(new { message = "Shift not found" });

        return Ok(new { message = "Shift assigned successfully" });
    }
}
