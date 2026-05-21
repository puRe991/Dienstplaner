namespace ShiftPilot.Core.Models;

public class User
{
    public int Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public UserRole Role { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    
    // Navigation properties
    public ICollection<Shift> AssignedShifts { get; set; } = new List<Shift>();
    public ICollection<SickLeave> SickLeaves { get; set; } = new List<SickLeave>();
    public ICollection<ShiftSwapRequest> InitiatedSwaps { get; set; } = new List<ShiftSwapRequest>();
    public ICollection<Availability> Availabilities { get; set; } = new List<Availability>();
}

public enum UserRole
{
    Employee = 0,
    Manager = 1,
    Admin = 2
}
