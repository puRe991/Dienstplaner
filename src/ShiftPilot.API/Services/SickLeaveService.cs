using ShiftPilot.API.Repositories;
using ShiftPilot.Core.Models;

namespace ShiftPilot.API.Services;

public class SickLeaveService : ISickLeaveService
{
    private readonly ISickLeaveRepository _sickLeaveRepository;

    public SickLeaveService(ISickLeaveRepository sickLeaveRepository)
    {
        _sickLeaveRepository = sickLeaveRepository;
    }

    public async Task<IEnumerable<SickLeave>> GetAllSickLeavesAsync()
    {
        return await _sickLeaveRepository.GetAllSickLeavesAsync();
    }

    public async Task<SickLeave?> GetSickLeaveByIdAsync(int sickLeaveId)
    {
        return await _sickLeaveRepository.GetSickLeaveByIdAsync(sickLeaveId);
    }

    public async Task<SickLeave> CreateSickLeaveAsync(SickLeave sickLeave)
    {
        sickLeave.CreatedAt = DateTime.UtcNow;
        sickLeave.UpdatedAt = DateTime.UtcNow;
        sickLeave.Status = SickLeaveStatus.Pending;
        return await _sickLeaveRepository.AddSickLeaveAsync(sickLeave);
    }

    public async Task<SickLeave> UpdateSickLeaveAsync(SickLeave sickLeave)
    {
        sickLeave.UpdatedAt = DateTime.UtcNow;
        return await _sickLeaveRepository.UpdateSickLeaveAsync(sickLeave);
    }

    public async Task<bool> DeleteSickLeaveAsync(int sickLeaveId)
    {
        return await _sickLeaveRepository.DeleteSickLeaveAsync(sickLeaveId);
    }

    public async Task<IEnumerable<SickLeave>> GetSickLeavesByUserAsync(int userId)
    {
        return await _sickLeaveRepository.GetSickLeavesByUserAsync(userId);
    }

    public async Task<bool> ApproveSickLeaveAsync(int sickLeaveId)
    {
        var sickLeave = await _sickLeaveRepository.GetSickLeaveByIdAsync(sickLeaveId);
        if (sickLeave == null)
            return false;

        sickLeave.Status = SickLeaveStatus.Approved;
        sickLeave.UpdatedAt = DateTime.UtcNow;
        await _sickLeaveRepository.UpdateSickLeaveAsync(sickLeave);
        return true;
    }

    public async Task<bool> RejectSickLeaveAsync(int sickLeaveId)
    {
        var sickLeave = await _sickLeaveRepository.GetSickLeaveByIdAsync(sickLeaveId);
        if (sickLeave == null)
            return false;

        sickLeave.Status = SickLeaveStatus.Rejected;
        sickLeave.UpdatedAt = DateTime.UtcNow;
        await _sickLeaveRepository.UpdateSickLeaveAsync(sickLeave);
        return true;
    }
}
