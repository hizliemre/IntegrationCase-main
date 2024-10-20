using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class Registrar
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR();
        return services;
    }

    public static IServiceCollection AddMediatR(this IServiceCollection services)
    {
        services.AddMediatR(configuration => { configuration.RegisterServicesFromAssembly(typeof(Registrar).Assembly); });
        return services;
    }
}
