using ShiftPilot.Core.Models;

namespace ShiftPilot.API.Services;

public interface ISickLeaveService
{
    Task<IEnumerable<SickLeave>> GetAllSickLeavesAsync();
    Task<SickLeave?> GetSickLeaveByIdAsync(int sickLeaveId);
    Task<SickLeave> CreateSickLeaveAsync(SickLeave sickLeave);
    Task<SickLeave> UpdateSickLeaveAsync(SickLeave sickLeave);
    Task<bool> DeleteSickLeaveAsync(int sickLeaveId);
    Task<IEnumerable<SickLeave>> GetSickLeavesByUserAsync(int userId);
    Task<bool> ApproveSickLeaveAsync(int sickLeaveId);
    Task<bool> RejectSickLeaveAsync(int sickLeaveId);
}
