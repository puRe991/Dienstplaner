using ShiftPilot.API.Services;
using ShiftPilot.API.Repositories;

namespace ShiftPilot.API.Extensions;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // Services
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IShiftService, ShiftService>();
        services.AddScoped<ISickLeaveService, SickLeaveService>();
        services.AddScoped<IShiftSwapService, ShiftSwapService>();
        services.AddScoped<IAvailabilityService, AvailabilityService>();
        services.AddScoped<ISchedulingService, SchedulingService>();
        services.AddScoped<INotificationService, NotificationService>();

        // Repositories
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IShiftRepository, ShiftRepository>();
        services.AddScoped<ISickLeaveRepository, SickLeaveRepository>();
        services.AddScoped<IShiftSwapRepository, ShiftSwapRepository>();
        services.AddScoped<IAvailabilityRepository, AvailabilityRepository>();

        // Logging
        services.AddScoped<ICustomLogger, CustomLogger>();

        return services;
    }
}
