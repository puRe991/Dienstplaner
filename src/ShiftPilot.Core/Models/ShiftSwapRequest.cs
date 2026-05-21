namespace ShiftPilot.Core.Models;

public class ShiftSwapRequest
{
    public int Id { get; set; }
    public int InitiatorId { get; set; }
    public int TargetUserId { get; set; }
    public int InitiatorShiftId { get; set; }
    public int TargetShiftId { get; set; }
    public ShiftSwapStatus Status { get; set; }
    public string? Reason { get; set; }
    public DateTime RequestedAt { get; set; }
    public DateTime? RespondedAt { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    
    // Navigation properties
    public User Initiator { get; set; } = null!;
    public User TargetUser { get; set; } = null!;
    public Shift InitiatorShift { get; set; } = null!;
    public Shift TargetShift { get; set; } = null!;
}

public enum ShiftSwapStatus
{
    Pending = 0,
    Approved = 1,
    Rejected = 2,
    Cancelled = 3
}
