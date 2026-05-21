namespace ShiftPilot.API.DTOs;

public class CreateShiftRequest
{
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public int ShiftType { get; set; }
    public string? Notes { get; set; }
}

public class UpdateShiftRequest
{
    public int Id { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public int ShiftType { get; set; }
    public int? AssignedUserId { get; set; }
    public string? Notes { get; set; }
}

public class ShiftResponse
{
    public int Id { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public string Type { get; set; } = string.Empty;
    public int? AssignedUserId { get; set; }
    public string Status { get; set; } = string.Empty;
    public string? Notes { get; set; }
}
