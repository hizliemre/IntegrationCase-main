using Application;
using Infrastructure;
using Infrastructure.Adapters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.Redis;

namespace Integration.Tests.Fixtures;

public class AppServiceFactory : IDisposable
{
    private readonly RedisContainer _redisContainer;
    private readonly SingletonInMemoryStoreAdapter _store;
    private readonly SingletonIdentityAdapter _identityAdapter;

    public AppServiceFactory()
    {
        _redisContainer = new RedisBuilder()
            .WithImage("redis:6.2.5")
            .WithPortBinding(6379)
            .Build();
        _redisContainer.StartAsync().GetAwaiter().GetResult();
        _store = new SingletonInMemoryStoreAdapter();
        _identityAdapter = new SingletonIdentityAdapter();
        DistributedService1 = CreateService("Distributed");
        DistributedService2 = CreateService("Distributed");
        DistributedService3 = CreateService("Distributed");
        SingleService = CreateService("Single");
    }

    public ServiceProvider DistributedService1 { get; }
    public ServiceProvider DistributedService2 { get; }
    public ServiceProvider DistributedService3 { get; }
    public ServiceProvider SingleService { get; }

    public void Dispose()
    {
        _redisContainer.DisposeAsync().GetAwaiter().GetResult();
        DistributedService1.Dispose();
        DistributedService2.Dispose();
        DistributedService3.Dispose();
    }

    private ServiceProvider CreateService(string serviceType)
    {
        IConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
        Dictionary<string, string> configs = new()
        {
            { "ServiceType", serviceType }
        };
        configurationBuilder.AddInMemoryCollection(configs);
        IConfiguration configuration = configurationBuilder.Build();
        IServiceCollection serviceCollection = new ServiceCollection();
        serviceCollection.AddApplication();
        serviceCollection.AddInfrastructure(configuration, _store, _identityAdapter);
        ServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();
        return serviceProvider;
    }
}
