using ShiftPilot.Core.Models;

namespace ShiftPilot.API.Services;

public interface IAvailabilityService
{
    Task<IEnumerable<Availability>> GetAllAvailabilitiesAsync();
    Task<Availability?> GetAvailabilityByIdAsync(int availabilityId);
    Task<Availability> CreateAvailabilityAsync(Availability availability);
    Task<Availability> UpdateAvailabilityAsync(Availability availability);
    Task<bool> DeleteAvailabilityAsync(int availabilityId);
    Task<IEnumerable<Availability>> GetAvailabilitiesByUserAsync(int userId);
    Task<IEnumerable<Availability>> GetAvailabilitiesForDateRangeAsync(DateTime startDate, DateTime endDate);
}
