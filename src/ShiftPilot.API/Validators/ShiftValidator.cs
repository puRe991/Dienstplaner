using ShiftPilot.Core.Models;

namespace ShiftPilot.API.Validators;

public class ShiftValidator
{
    public static (bool IsValid, string Error) ValidateShiftTimes(DateTime startTime, DateTime endTime)
    {
        if (endTime <= startTime)
            return (false, "End time must be after start time");

        if ((endTime - startTime).TotalHours < 1)
            return (false, "Shift must be at least 1 hour long");

        if ((endTime - startTime).TotalHours > 24)
            return (false, "Shift cannot be longer than 24 hours");

        return (true, "");
    }

    public static (bool IsValid, string Error) ValidateShiftDateRange(DateTime startTime, DateTime maxDate)
    {
        if (startTime < DateTime.UtcNow)
            return (false, "Cannot create shifts in the past");

        if (startTime > maxDate)
            return (false, $"Shift must be scheduled before {maxDate:yyyy-MM-dd}");

        return (true, "");
    }
}
