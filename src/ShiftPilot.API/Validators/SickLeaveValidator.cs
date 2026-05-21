using ShiftPilot.Core.Models;

namespace ShiftPilot.API.Validators;

public class SickLeaveValidator
{
    public static (bool IsValid, string Error) ValidateSickLeaveDateRange(DateTime startDate, DateTime endDate)
    {
        if (endDate < startDate)
            return (false, "End date must be after or equal to start date");

        if ((endDate - startDate).TotalDays > 30)
            return (false, "Sick leave period cannot exceed 30 days");

        return (true, "");
    }

    public static (bool IsValid, string Error) ValidateSickLeaveDocumentation(SickLeave sickLeave)
    {
        if ((sickLeave.EndDate - sickLeave.StartDate).TotalDays >= 3)
        {
            if (string.IsNullOrWhiteSpace(sickLeave.Certificate))
                return (false, "Medical certificate required for sick leave longer than 3 days");
        }

        return (true, "");
    }
}
