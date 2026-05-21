using ShiftPilot.API.Repositories;
using ShiftPilot.Core.Models;

namespace ShiftPilot.API.Services;

public class AvailabilityService : IAvailabilityService
{
    private readonly IAvailabilityRepository _availabilityRepository;

    public AvailabilityService(IAvailabilityRepository availabilityRepository)
    {
        _availabilityRepository = availabilityRepository;
    }

    public async Task<IEnumerable<Availability>> GetAllAvailabilitiesAsync()
    {
        return await _availabilityRepository.GetAllAvailabilitiesAsync();
    }

    public async Task<Availability?> GetAvailabilityByIdAsync(int availabilityId)
    {
        return await _availabilityRepository.GetAvailabilityByIdAsync(availabilityId);
    }

    public async Task<Availability> CreateAvailabilityAsync(Availability availability)
    {
        availability.CreatedAt = DateTime.UtcNow;
        availability.UpdatedAt = DateTime.UtcNow;
        return await _availabilityRepository.AddAvailabilityAsync(availability);
    }

    public async Task<Availability> UpdateAvailabilityAsync(Availability availability)
    {
        availability.UpdatedAt = DateTime.UtcNow;
        return await _availabilityRepository.UpdateAvailabilityAsync(availability);
    }

    public async Task<bool> DeleteAvailabilityAsync(int availabilityId)
    {
        return await _availabilityRepository.DeleteAvailabilityAsync(availabilityId);
    }

    public async Task<IEnumerable<Availability>> GetAvailabilitiesByUserAsync(int userId)
    {
        return await _availabilityRepository.GetAvailabilitiesByUserAsync(userId);
    }

    public async Task<IEnumerable<Availability>> GetAvailabilitiesForDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        return await _availabilityRepository.GetAvailabilitiesForDateRangeAsync(startDate, endDate);
    }
}
