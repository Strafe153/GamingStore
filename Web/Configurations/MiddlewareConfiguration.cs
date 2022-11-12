using Web.Middleware;

namespace WebApi.Configurations;

public static class MiddlewareConfiguration
{
    public static void AddCustomMiddleware(this IApplicationBuilder builder)
    {
        builder.UseMiddleware<ExceptionsMiddleware>();
    }
}
