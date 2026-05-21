using ShiftPilot.API.Repositories;
using ShiftPilot.Core.Models;

namespace ShiftPilot.API.Services;

public class ShiftSwapService : IShiftSwapService
{
    private readonly IShiftSwapRepository _swapRepository;
    private readonly IShiftRepository _shiftRepository;

    public ShiftSwapService(IShiftSwapRepository swapRepository, IShiftRepository shiftRepository)
    {
        _swapRepository = swapRepository;
        _shiftRepository = shiftRepository;
    }

    public async Task<IEnumerable<ShiftSwapRequest>> GetAllSwapRequestsAsync()
    {
        return await _swapRepository.GetAllSwapRequestsAsync();
    }

    public async Task<ShiftSwapRequest?> GetSwapRequestByIdAsync(int swapRequestId)
    {
        return await _swapRepository.GetSwapRequestByIdAsync(swapRequestId);
    }

    public async Task<ShiftSwapRequest> CreateSwapRequestAsync(ShiftSwapRequest swapRequest)
    {
        swapRequest.CreatedAt = DateTime.UtcNow;
        swapRequest.UpdatedAt = DateTime.UtcNow;
        swapRequest.Status = ShiftSwapStatus.Pending;
        swapRequest.RequestedAt = DateTime.UtcNow;
        return await _swapRepository.AddSwapRequestAsync(swapRequest);
    }

    public async Task<bool> ApproveSwapRequestAsync(int swapRequestId)
    {
        var swapRequest = await _swapRepository.GetSwapRequestByIdAsync(swapRequestId);
        if (swapRequest == null)
            return false;

        var initiatorShift = await _shiftRepository.GetShiftByIdAsync(swapRequest.InitiatorShiftId);
        var targetShift = await _shiftRepository.GetShiftByIdAsync(swapRequest.TargetShiftId);

        if (initiatorShift == null || targetShift == null)
            return false;

        // Swap the assignments
        var tempUserId = initiatorShift.AssignedUserId;
        initiatorShift.AssignedUserId = targetShift.AssignedUserId;
        targetShift.AssignedUserId = tempUserId;

        await _shiftRepository.UpdateShiftAsync(initiatorShift);
        await _shiftRepository.UpdateShiftAsync(targetShift);

        swapRequest.Status = ShiftSwapStatus.Approved;
        swapRequest.RespondedAt = DateTime.UtcNow;
        swapRequest.UpdatedAt = DateTime.UtcNow;
        await _swapRepository.UpdateSwapRequestAsync(swapRequest);

        return true;
    }

    public async Task<bool> RejectSwapRequestAsync(int swapRequestId)
    {
        var swapRequest = await _swapRepository.GetSwapRequestByIdAsync(swapRequestId);
        if (swapRequest == null)
            return false;

        swapRequest.Status = ShiftSwapStatus.Rejected;
        swapRequest.RespondedAt = DateTime.UtcNow;
        swapRequest.UpdatedAt = DateTime.UtcNow;
        await _swapRepository.UpdateSwapRequestAsync(swapRequest);

        return true;
    }

    public async Task<IEnumerable<ShiftSwapRequest>> GetPendingSwapRequestsForUserAsync(int userId)
    {
        return await _swapRepository.GetPendingSwapRequestsForUserAsync(userId);
    }
}
