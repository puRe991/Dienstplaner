using ShiftPilot.Core.Models;

namespace ShiftPilot.API.Repositories;

public interface ISickLeaveRepository
{
    Task<SickLeave?> GetSickLeaveByIdAsync(int sickLeaveId);
    Task<IEnumerable<SickLeave>> GetAllSickLeavesAsync();
    Task<IEnumerable<SickLeave>> GetSickLeavesByUserAsync(int userId);
    Task<SickLeave> AddSickLeaveAsync(SickLeave sickLeave);
    Task<SickLeave> UpdateSickLeaveAsync(SickLeave sickLeave);
    Task<bool> DeleteSickLeaveAsync(int sickLeaveId);
}
