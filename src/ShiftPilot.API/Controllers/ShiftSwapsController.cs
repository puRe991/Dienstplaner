using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShiftPilot.API.Services;
using ShiftPilot.Core.Models;

namespace ShiftPilot.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ShiftSwapsController : ControllerBase
{
    private readonly IShiftSwapService _shiftSwapService;

    public ShiftSwapsController(IShiftSwapService shiftSwapService)
    {
        _shiftSwapService = shiftSwapService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllSwapRequests()
    {
        var requests = await _shiftSwapService.GetAllSwapRequestsAsync();
        return Ok(requests);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetSwapRequestById(int id)
    {
        var request = await _shiftSwapService.GetSwapRequestByIdAsync(id);
        if (request == null)
            return NotFound(new { message = "Swap request not found" });

        return Ok(request);
    }

    [HttpGet("pending/{userId}")]
    public async Task<IActionResult> GetPendingSwapRequestsForUser(int userId)
    {
        var requests = await _shiftSwapService.GetPendingSwapRequestsForUserAsync(userId);
        return Ok(requests);
    }

    [HttpPost]
    public async Task<IActionResult> CreateSwapRequest([FromBody] ShiftSwapRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var createdRequest = await _shiftSwapService.CreateSwapRequestAsync(request);
        return CreatedAtAction(nameof(GetSwapRequestById), new { id = createdRequest.Id }, createdRequest);
    }

    [HttpPost("{id}/approve")]
    public async Task<IActionResult> ApproveSwapRequest(int id)
    {
        var result = await _shiftSwapService.ApproveSwapRequestAsync(id);
        if (!result)
            return NotFound(new { message = "Swap request not found" });

        return Ok(new { message = "Swap request approved" });
    }

    [HttpPost("{id}/reject")]
    public async Task<IActionResult> RejectSwapRequest(int id)
    {
        var result = await _shiftSwapService.RejectSwapRequestAsync(id);
        if (!result)
            return NotFound(new { message = "Swap request not found" });

        return Ok(new { message = "Swap request rejected" });
    }
}
