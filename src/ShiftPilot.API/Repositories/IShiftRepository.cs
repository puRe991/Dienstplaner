using ShiftPilot.Core.Models;

namespace ShiftPilot.API.Repositories;

public interface IShiftRepository
{
    Task<Shift?> GetShiftByIdAsync(int shiftId);
    Task<IEnumerable<Shift>> GetAllShiftsAsync();
    Task<IEnumerable<Shift>> GetShiftsByUserAsync(int userId);
    Task<IEnumerable<Shift>> GetUnassignedShiftsAsync();
    Task<Shift> AddShiftAsync(Shift shift);
    Task<Shift> UpdateShiftAsync(Shift shift);
    Task<bool> DeleteShiftAsync(int shiftId);
}
