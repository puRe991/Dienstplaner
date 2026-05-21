using ShiftPilot.API.Middleware;
using ShiftPilot.API.Services;

namespace ShiftPilot.API.Extensions;

public static class ServiceExtensions
{
    public static IServiceCollection AddCustomServices(this IServiceCollection services)
    {
        services.AddScoped<ISchedulingService, SchedulingService>();
        services.AddScoped<INotificationService, NotificationService>();
        return services;
    }

    public static IApplicationBuilder UseCustomMiddleware(this IApplicationBuilder app)
    {
        app.UseMiddleware<ExceptionHandlingMiddleware>();
        return app;
    }
}
