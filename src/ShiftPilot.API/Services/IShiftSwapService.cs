using ShiftPilot.Core.Models;

namespace ShiftPilot.API.Services;

public interface IShiftSwapService
{
    Task<IEnumerable<ShiftSwapRequest>> GetAllSwapRequestsAsync();
    Task<ShiftSwapRequest?> GetSwapRequestByIdAsync(int swapRequestId);
    Task<ShiftSwapRequest> CreateSwapRequestAsync(ShiftSwapRequest swapRequest);
    Task<bool> ApproveSwapRequestAsync(int swapRequestId);
    Task<bool> RejectSwapRequestAsync(int swapRequestId);
    Task<IEnumerable<ShiftSwapRequest>> GetPendingSwapRequestsForUserAsync(int userId);
}
