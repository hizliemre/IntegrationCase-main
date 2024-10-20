using Application.Providers;
using Application.Services;
using Domain.Ports;
using Infrastructure.Adapters;
using Infrastructure.Providers;
using Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace Infrastructure;

public static class Registrar
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration, SingletonInMemoryStoreAdapter store)
    {
        services.AddScoped<IRetrievePort>(_ => store);
        services.AddScoped<IPersistencyPort>(_ => store);
        services.AddSingleton<IIdentityPort, SingletonIdentityAdapter>();
        services.AddScoped<IItemOperationBackendService, ItemOperationBackendService>();
        string serviceType = configuration["ServiceType"]!;
        switch (serviceType)
        {
            case "Single":
                services.AddSingleton<ILockProvider, SingleServiceLockProvider>();
                break;
            case "Distributed":
                // Redis host can be configured in appsettings.json
                services.AddSingleton<IConnectionMultiplexer>(_ => ConnectionMultiplexer.Connect("localhost:6379"));
                services.AddScoped<ILockProvider, RedisDistributedLockProvider>();
                break;
        }
        return services;
    }
}
