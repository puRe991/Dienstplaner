using Microsoft.EntityFrameworkCore;
using ShiftPilot.Core.Models;
using ShiftPilot.Data;

namespace ShiftPilot.API.Repositories;

public class AvailabilityRepository : IAvailabilityRepository
{
    private readonly ApplicationDbContext _context;

    public AvailabilityRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Availability?> GetAvailabilityByIdAsync(int availabilityId)
    {
        return await _context.Availabilities.Include(a => a.User).FirstOrDefaultAsync(a => a.Id == availabilityId);
    }

    public async Task<IEnumerable<Availability>> GetAllAvailabilitiesAsync()
    {
        return await _context.Availabilities.Include(a => a.User).ToListAsync();
    }

    public async Task<IEnumerable<Availability>> GetAvailabilitiesByUserAsync(int userId)
    {
        return await _context.Availabilities
            .Where(a => a.UserId == userId)
            .Include(a => a.User)
            .ToListAsync();
    }

    public async Task<IEnumerable<Availability>> GetAvailabilitiesForDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        return await _context.Availabilities
            .Where(a => a.Date >= startDate && a.Date <= endDate)
            .Include(a => a.User)
            .ToListAsync();
    }

    public async Task<Availability> AddAvailabilityAsync(Availability availability)
    {
        _context.Availabilities.Add(availability);
        await _context.SaveChangesAsync();
        return availability;
    }

    public async Task<Availability> UpdateAvailabilityAsync(Availability availability)
    {
        _context.Availabilities.Update(availability);
        await _context.SaveChangesAsync();
        return availability;
    }

    public async Task<bool> DeleteAvailabilityAsync(int availabilityId)
    {
        var availability = await _context.Availabilities.FirstOrDefaultAsync(a => a.Id == availabilityId);
        if (availability == null)
            return false;

        _context.Availabilities.Remove(availability);
        await _context.SaveChangesAsync();
        return true;
    }
}
