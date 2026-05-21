using ShiftPilot.Core.Models;

namespace ShiftPilot.API.Repositories;

public interface IAvailabilityRepository
{
    Task<Availability?> GetAvailabilityByIdAsync(int availabilityId);
    Task<IEnumerable<Availability>> GetAllAvailabilitiesAsync();
    Task<IEnumerable<Availability>> GetAvailabilitiesByUserAsync(int userId);
    Task<IEnumerable<Availability>> GetAvailabilitiesForDateRangeAsync(DateTime startDate, DateTime endDate);
    Task<Availability> AddAvailabilityAsync(Availability availability);
    Task<Availability> UpdateAvailabilityAsync(Availability availability);
    Task<bool> DeleteAvailabilityAsync(int availabilityId);
}
