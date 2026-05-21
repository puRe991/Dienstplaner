using Microsoft.AspNetCore.RateLimiting;

namespace ShiftPilot.API.Extensions;

public static class RateLimitingExtensions
{
    public static IServiceCollection AddCustomRateLimiting(this IServiceCollection services)
    {
        services.AddRateLimiter(options =>
        {
            options.AddFixedWindowLimiter("fixed", limiterOptions =>
            {
                limiterOptions.PermitLimit = 100;
                limiterOptions.Window = TimeSpan.FromMinutes(1);
                limiterOptions.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                limiterOptions.QueueLimit = 2;
            });

            options.AddSlidingWindowLimiter("sliding", limiterOptions =>
            {
                limiterOptions.PermitLimit = 50;
                limiterOptions.Window = TimeSpan.FromSeconds(10);
                limiterOptions.SegmentsPerWindow = 2;
                limiterOptions.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                limiterOptions.QueueLimit = 2;
            });
        });

        return services;
    }
}
