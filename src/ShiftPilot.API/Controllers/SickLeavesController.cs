using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShiftPilot.API.Services;
using ShiftPilot.Core.Models;

namespace ShiftPilot.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class SickLeavesController : ControllerBase
{
    private readonly ISickLeaveService _sickLeaveService;

    public SickLeavesController(ISickLeaveService sickLeaveService)
    {
        _sickLeaveService = sickLeaveService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllSickLeaves()
    {
        var sickLeaves = await _sickLeaveService.GetAllSickLeavesAsync();
        return Ok(sickLeaves);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetSickLeaveById(int id)
    {
        var sickLeave = await _sickLeaveService.GetSickLeaveByIdAsync(id);
        if (sickLeave == null)
            return NotFound(new { message = "Sick leave not found" });

        return Ok(sickLeave);
    }

    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetSickLeavesByUser(int userId)
    {
        var sickLeaves = await _sickLeaveService.GetSickLeavesByUserAsync(userId);
        return Ok(sickLeaves);
    }

    [HttpPost]
    public async Task<IActionResult> CreateSickLeave([FromBody] SickLeave sickLeave)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var createdSickLeave = await _sickLeaveService.CreateSickLeaveAsync(sickLeave);
        return CreatedAtAction(nameof(GetSickLeaveById), new { id = createdSickLeave.Id }, createdSickLeave);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateSickLeave(int id, [FromBody] SickLeave sickLeave)
    {
        if (id != sickLeave.Id)
            return BadRequest(new { message = "ID mismatch" });

        var existingSickLeave = await _sickLeaveService.GetSickLeaveByIdAsync(id);
        if (existingSickLeave == null)
            return NotFound(new { message = "Sick leave not found" });

        var updatedSickLeave = await _sickLeaveService.UpdateSickLeaveAsync(sickLeave);
        return Ok(updatedSickLeave);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSickLeave(int id)
    {
        var result = await _sickLeaveService.DeleteSickLeaveAsync(id);
        if (!result)
            return NotFound(new { message = "Sick leave not found" });

        return NoContent();
    }

    [HttpPost("{id}/approve")]
    public async Task<IActionResult> ApproveSickLeave(int id)
    {
        var result = await _sickLeaveService.ApproveSickLeaveAsync(id);
        if (!result)
            return NotFound(new { message = "Sick leave not found" });

        return Ok(new { message = "Sick leave approved" });
    }

    [HttpPost("{id}/reject")]
    public async Task<IActionResult> RejectSickLeave(int id)
    {
        var result = await _sickLeaveService.RejectSickLeaveAsync(id);
        if (!result)
            return NotFound(new { message = "Sick leave not found" });

        return Ok(new { message = "Sick leave rejected" });
    }
}
