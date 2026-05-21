namespace ShiftPilot.Core.Models;

public class Shift
{
    public int Id { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public ShiftType Type { get; set; }
    public int? AssignedUserId { get; set; }
    public ShiftStatus Status { get; set; }
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    
    // Navigation properties
    public User? AssignedUser { get; set; }
}

public enum ShiftType
{
    Morning = 0,
    Afternoon = 1,
    Evening = 2,
    Night = 3,
    FullDay = 4
}

public enum ShiftStatus
{
    Unassigned = 0,
    Assigned = 1,
    Completed = 2,
    Cancelled = 3
}
