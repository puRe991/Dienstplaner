using Microsoft.EntityFrameworkCore;
using ShiftPilot.Core.Models;
using ShiftPilot.Data;

namespace ShiftPilot.API.Repositories;

public class ShiftRepository : IShiftRepository
{
    private readonly ApplicationDbContext _context;

    public ShiftRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Shift?> GetShiftByIdAsync(int shiftId)
    {
        return await _context.Shifts.Include(s => s.AssignedUser).FirstOrDefaultAsync(s => s.Id == shiftId);
    }

    public async Task<IEnumerable<Shift>> GetAllShiftsAsync()
    {
        return await _context.Shifts.Include(s => s.AssignedUser).ToListAsync();
    }

    public async Task<IEnumerable<Shift>> GetShiftsByUserAsync(int userId)
    {
        return await _context.Shifts
            .Where(s => s.AssignedUserId == userId)
            .Include(s => s.AssignedUser)
            .ToListAsync();
    }

    public async Task<IEnumerable<Shift>> GetUnassignedShiftsAsync()
    {
        return await _context.Shifts
            .Where(s => s.AssignedUserId == null)
            .ToListAsync();
    }

    public async Task<Shift> AddShiftAsync(Shift shift)
    {
        _context.Shifts.Add(shift);
        await _context.SaveChangesAsync();
        return shift;
    }

    public async Task<Shift> UpdateShiftAsync(Shift shift)
    {
        _context.Shifts.Update(shift);
        await _context.SaveChangesAsync();
        return shift;
    }

    public async Task<bool> DeleteShiftAsync(int shiftId)
    {
        var shift = await _context.Shifts.FirstOrDefaultAsync(s => s.Id == shiftId);
        if (shift == null)
            return false;

        _context.Shifts.Remove(shift);
        await _context.SaveChangesAsync();
        return true;
    }
}
