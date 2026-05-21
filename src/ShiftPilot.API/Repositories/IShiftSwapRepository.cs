using ShiftPilot.Core.Models;

namespace ShiftPilot.API.Repositories;

public interface IShiftSwapRepository
{
    Task<ShiftSwapRequest?> GetSwapRequestByIdAsync(int swapRequestId);
    Task<IEnumerable<ShiftSwapRequest>> GetAllSwapRequestsAsync();
    Task<IEnumerable<ShiftSwapRequest>> GetPendingSwapRequestsForUserAsync(int userId);
    Task<ShiftSwapRequest> AddSwapRequestAsync(ShiftSwapRequest swapRequest);
    Task<ShiftSwapRequest> UpdateSwapRequestAsync(ShiftSwapRequest swapRequest);
    Task<bool> DeleteSwapRequestAsync(int swapRequestId);
}
