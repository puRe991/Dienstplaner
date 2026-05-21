using ShiftPilot.Core.Models;

namespace ShiftPilot.API.Validators;

public class AvailabilityValidator
{
    public static (bool IsValid, string Error) ValidateAvailabilityTimes(TimeOnly startTime, TimeOnly endTime)
    {
        if (endTime <= startTime)
            return (false, "End time must be after start time");

        return (true, "");
    }

    public static (bool IsValid, string Error) ValidateAvailabilityDate(DateTime date)
    {
        if (date.Date < DateTime.UtcNow.Date)
            return (false, "Cannot set availability for past dates");

        if (date.Date > DateTime.UtcNow.Date.AddDays(90))
            return (false, "Availability can only be set up to 90 days in advance");

        return (true, "");
    }
}
