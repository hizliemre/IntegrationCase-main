using Domain;
using Domain.UseCases;
using FluentAssertions;
using Integration.Tests.Fixtures;
using Kernel;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Integration.Tests;

[Collection("test_scope")]
public class Distributed_Service : IClassFixture<AppServiceFactory>
{
    private readonly AppServiceFactory _fixture;

    public Distributed_Service(AppServiceFactory fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task should_save_multiple_items_asyncronously_on_distributed_service()
    {
        // Arrange
        ISender service1 = _fixture.DistributedService1.GetRequiredService<ISender>();
        ISender service2 = _fixture.DistributedService2.GetRequiredService<ISender>();
        ISender service3 = _fixture.DistributedService3.GetRequiredService<ISender>();

        SaveItem saveItem1 = SaveItem.Prepare("test4-a");
        SaveItem saveItem2 = SaveItem.Prepare("test4-b");
        SaveItem saveItem3 = SaveItem.Prepare("test4-c");
        GetAllItems getAllItems = GetAllItems.Prepare();

        // Act
        List<Task<Result<Item>>> tasks = new()
        {
            service1.Send(saveItem1),
            service1.Send(saveItem2),
            service1.Send(saveItem3),

            service2.Send(saveItem1),
            service2.Send(saveItem2),
            service2.Send(saveItem3),

            service3.Send(saveItem1),
            service3.Send(saveItem2),
            service3.Send(saveItem3)
        };

        Result<Item>[] results = await Task.WhenAll(tasks);
        Result<IEnumerable<Item>> getAllItemsResultFromService1 = await service1.Send(getAllItems);
        Result<IEnumerable<Item>> getAllItemsResultFromService2 = await service2.Send(getAllItems);
        Result<IEnumerable<Item>> getAllItemsResultFromService3 = await service3.Send(getAllItems);

        // Assert
        results.Should().HaveCount(9);
        results.Where(m => m.IsSuccess).Should().HaveCount(3);
        getAllItemsResultFromService1.Data.Should().HaveCount(3);
        getAllItemsResultFromService2.Data.Should().HaveCount(3);
        getAllItemsResultFromService3.Data.Should().HaveCount(3);
        getAllItemsResultFromService1.Data.Select(m => m.Content).Should().BeEquivalentTo(new[] { new ItemContent("test4-a"), new ItemContent("test4-b"), new ItemContent("test4-c") });
        getAllItemsResultFromService1.Data.Should().BeEquivalentTo(getAllItemsResultFromService2.Data);
        getAllItemsResultFromService2.Data.Should().BeEquivalentTo(getAllItemsResultFromService3.Data);
    }
}
