using Application.Behaviors;
using MediatR;

namespace Web.Configurations
{
    public static class MediatRConfiguration
    {
        public static void ConfigureMediatR(this IServiceCollection services)
        {
            services.AddMediatR(typeof(Application.AssemblyReference).Assembly);
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationPipelineBehavior<,>));
        }
    }
}
