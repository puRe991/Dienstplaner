using Microsoft.EntityFrameworkCore;
using ShiftPilot.Core.Models;
using ShiftPilot.Data;

namespace ShiftPilot.API.Repositories;

public class SickLeaveRepository : ISickLeaveRepository
{
    private readonly ApplicationDbContext _context;

    public SickLeaveRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<SickLeave?> GetSickLeaveByIdAsync(int sickLeaveId)
    {
        return await _context.SickLeaves.Include(s => s.User).FirstOrDefaultAsync(s => s.Id == sickLeaveId);
    }

    public async Task<IEnumerable<SickLeave>> GetAllSickLeavesAsync()
    {
        return await _context.SickLeaves.Include(s => s.User).ToListAsync();
    }

    public async Task<IEnumerable<SickLeave>> GetSickLeavesByUserAsync(int userId)
    {
        return await _context.SickLeaves
            .Where(s => s.UserId == userId)
            .Include(s => s.User)
            .ToListAsync();
    }

    public async Task<SickLeave> AddSickLeaveAsync(SickLeave sickLeave)
    {
        _context.SickLeaves.Add(sickLeave);
        await _context.SaveChangesAsync();
        return sickLeave;
    }

    public async Task<SickLeave> UpdateSickLeaveAsync(SickLeave sickLeave)
    {
        _context.SickLeaves.Update(sickLeave);
        await _context.SaveChangesAsync();
        return sickLeave;
    }

    public async Task<bool> DeleteSickLeaveAsync(int sickLeaveId)
    {
        var sickLeave = await _context.SickLeaves.FirstOrDefaultAsync(s => s.Id == sickLeaveId);
        if (sickLeave == null)
            return false;

        _context.SickLeaves.Remove(sickLeave);
        await _context.SaveChangesAsync();
        return true;
    }
}
