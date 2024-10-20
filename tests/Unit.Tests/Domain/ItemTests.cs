using Domain;
using Domain.Ports;
using FluentAssertions;
using Kernel;
using Microsoft.Extensions.DependencyInjection;
using Unit.Tests.FakeAdapters;
using Xunit;

namespace Unit.Tests.Domain;

public class Item_should
{
    [Fact]
    public async Task be_created()
    {
        // Arrange
        ServiceCollection serviceCollection = new();
        serviceCollection.AddSingleton<IIdentityPort, FakeIdentityAdapter>();
        ServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();
        FakeIdentityAdapter identityPort = (serviceProvider.GetRequiredService<IIdentityPort>() as FakeIdentityAdapter)!;

        // Act
        ItemContent itemContent = new("a");
        Item item = await Item.Create(itemContent, identityPort);
        Result<int> nextId = await identityPort.GetNextIdentity();

        // Assert
        item.Id.Should().Be(new ItemId(1));
        item.Content.Should().Be(itemContent);
        nextId.IsSuccess.Should().BeTrue();
        nextId.Data.Should().Be(2);
        identityPort.GetNextIdentityCall_Called.Should().Be(2);
    }
}
