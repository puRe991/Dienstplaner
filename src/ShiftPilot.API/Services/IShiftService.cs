using ShiftPilot.Core.Models;

namespace ShiftPilot.API.Services;

public interface IShiftService
{
    Task<IEnumerable<Shift>> GetAllShiftsAsync();
    Task<Shift?> GetShiftByIdAsync(int shiftId);
    Task<Shift> CreateShiftAsync(Shift shift);
    Task<Shift> UpdateShiftAsync(Shift shift);
    Task<bool> DeleteShiftAsync(int shiftId);
    Task<IEnumerable<Shift>> GetShiftsByUserAsync(int userId);
    Task<IEnumerable<Shift>> GetUnassignedShiftsAsync();
    Task<bool> AssignShiftAsync(int shiftId, int userId);
}
