using System.Security.Claims;

namespace ShiftPilot.API.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static int GetUserId(this ClaimsPrincipal user)
    {
        var userIdClaim = user.FindFirst("id")?.Value;
        return int.TryParse(userIdClaim, out var userId) ? userId : 0;
    }

    public static string GetUserEmail(this ClaimsPrincipal user)
    {
        return user.FindFirst("email")?.Value ?? string.Empty;
    }

    public static string GetUserRole(this ClaimsPrincipal user)
    {
        return user.FindFirst("role")?.Value ?? string.Empty;
    }
}
