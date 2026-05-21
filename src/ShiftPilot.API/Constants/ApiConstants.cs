namespace ShiftPilot.API.Constants;

public static class ApiConstants
{
    // Shift Constants
    public const int MinShiftDurationHours = 1;
    public const int MaxShiftDurationHours = 24;
    public const int MaxScheduleAdvanceDays = 90;

    // Availability Constants
    public const int MaxAvailabilityAdvanceDays = 90;

    // Sick Leave Constants
    public const int MaxSickLeaveDays = 30;
    public const int MedicalCertificateDaysThreshold = 3;

    // Pagination
    public const int DefaultPageSize = 20;
    public const int MaxPageSize = 100;

    // JWT
    public const string AuthorizationHeader = "Authorization";
    public const string BearerScheme = "Bearer";

    // Error Messages
    public const string InvalidCredentials = "Invalid email or password";
    public const string UserNotFound = "User not found";
    public const string ShiftNotFound = "Shift not found";
    public const string SickLeaveNotFound = "Sick leave not found";
    public const string SwapRequestNotFound = "Swap request not found";
    public const string AvailabilityNotFound = "Availability not found";
}
