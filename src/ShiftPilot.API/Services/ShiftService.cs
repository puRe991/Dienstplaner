using ShiftPilot.API.Repositories;
using ShiftPilot.Core.Models;

namespace ShiftPilot.API.Services;

public class ShiftService : IShiftService
{
    private readonly IShiftRepository _shiftRepository;

    public ShiftService(IShiftRepository shiftRepository)
    {
        _shiftRepository = shiftRepository;
    }

    public async Task<IEnumerable<Shift>> GetAllShiftsAsync()
    {
        return await _shiftRepository.GetAllShiftsAsync();
    }

    public async Task<Shift?> GetShiftByIdAsync(int shiftId)
    {
        return await _shiftRepository.GetShiftByIdAsync(shiftId);
    }

    public async Task<Shift> CreateShiftAsync(Shift shift)
    {
        shift.CreatedAt = DateTime.UtcNow;
        shift.UpdatedAt = DateTime.UtcNow;
        shift.Status = ShiftStatus.Unassigned;
        return await _shiftRepository.AddShiftAsync(shift);
    }

    public async Task<Shift> UpdateShiftAsync(Shift shift)
    {
        shift.UpdatedAt = DateTime.UtcNow;
        return await _shiftRepository.UpdateShiftAsync(shift);
    }

    public async Task<bool> DeleteShiftAsync(int shiftId)
    {
        return await _shiftRepository.DeleteShiftAsync(shiftId);
    }

    public async Task<IEnumerable<Shift>> GetShiftsByUserAsync(int userId)
    {
        return await _shiftRepository.GetShiftsByUserAsync(userId);
    }

    public async Task<IEnumerable<Shift>> GetUnassignedShiftsAsync()
    {
        return await _shiftRepository.GetUnassignedShiftsAsync();
    }

    public async Task<bool> AssignShiftAsync(int shiftId, int userId)
    {
        var shift = await _shiftRepository.GetShiftByIdAsync(shiftId);
        if (shift == null)
            return false;

        shift.AssignedUserId = userId;
        shift.Status = ShiftStatus.Assigned;
        shift.UpdatedAt = DateTime.UtcNow;
        await _shiftRepository.UpdateShiftAsync(shift);
        return true;
    }
}
