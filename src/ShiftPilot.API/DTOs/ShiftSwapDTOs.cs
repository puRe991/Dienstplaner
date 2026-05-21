namespace ShiftPilot.API.DTOs;

public class CreateSwapRequestRequest
{
    public int InitiatorId { get; set; }
    public int TargetUserId { get; set; }
    public int InitiatorShiftId { get; set; }
    public int TargetShiftId { get; set; }
    public string? Reason { get; set; }
}

public class ShiftSwapResponse
{
    public int Id { get; set; }
    public int InitiatorId { get; set; }
    public int TargetUserId { get; set; }
    public int InitiatorShiftId { get; set; }
    public int TargetShiftId { get; set; }
    public string Status { get; set; } = string.Empty;
    public string? Reason { get; set; }
    public DateTime RequestedAt { get; set; }
    public DateTime? RespondedAt { get; set; }
}
