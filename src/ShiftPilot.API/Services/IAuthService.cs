using ShiftPilot.Core.Models;

namespace ShiftPilot.API.Services;

public interface IAuthService
{
    Task<(bool Success, string Token, string Message)> RegisterAsync(string email, string firstName, string lastName, string password);
    Task<(bool Success, string Token, string Message)> LoginAsync(string email, string password);
    Task<User?> ValidateUserAsync(int userId);
}
