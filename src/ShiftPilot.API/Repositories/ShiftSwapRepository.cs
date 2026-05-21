using Microsoft.EntityFrameworkCore;
using ShiftPilot.Core.Models;
using ShiftPilot.Data;

namespace ShiftPilot.API.Repositories;

public class ShiftSwapRepository : IShiftSwapRepository
{
    private readonly ApplicationDbContext _context;

    public ShiftSwapRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ShiftSwapRequest?> GetSwapRequestByIdAsync(int swapRequestId)
    {
        return await _context.ShiftSwapRequests
            .Include(s => s.Initiator)
            .Include(s => s.TargetUser)
            .Include(s => s.InitiatorShift)
            .Include(s => s.TargetShift)
            .FirstOrDefaultAsync(s => s.Id == swapRequestId);
    }

    public async Task<IEnumerable<ShiftSwapRequest>> GetAllSwapRequestsAsync()
    {
        return await _context.ShiftSwapRequests
            .Include(s => s.Initiator)
            .Include(s => s.TargetUser)
            .Include(s => s.InitiatorShift)
            .Include(s => s.TargetShift)
            .ToListAsync();
    }

    public async Task<IEnumerable<ShiftSwapRequest>> GetPendingSwapRequestsForUserAsync(int userId)
    {
        return await _context.ShiftSwapRequests
            .Where(s => (s.TargetUserId == userId || s.InitiatorId == userId) && s.Status == ShiftSwapStatus.Pending)
            .Include(s => s.Initiator)
            .Include(s => s.TargetUser)
            .Include(s => s.InitiatorShift)
            .Include(s => s.TargetShift)
            .ToListAsync();
    }

    public async Task<ShiftSwapRequest> AddSwapRequestAsync(ShiftSwapRequest swapRequest)
    {
        _context.ShiftSwapRequests.Add(swapRequest);
        await _context.SaveChangesAsync();
        return swapRequest;
    }

    public async Task<ShiftSwapRequest> UpdateSwapRequestAsync(ShiftSwapRequest swapRequest)
    {
        _context.ShiftSwapRequests.Update(swapRequest);
        await _context.SaveChangesAsync();
        return swapRequest;
    }

    public async Task<bool> DeleteSwapRequestAsync(int swapRequestId)
    {
        var swapRequest = await _context.ShiftSwapRequests.FirstOrDefaultAsync(s => s.Id == swapRequestId);
        if (swapRequest == null)
            return false;

        _context.ShiftSwapRequests.Remove(swapRequest);
        await _context.SaveChangesAsync();
        return true;
    }
}
