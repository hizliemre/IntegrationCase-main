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
public class Single_service : IClassFixture<AppServiceFactory>
{
    private readonly AppServiceFactory _fixture;

    public Single_service(AppServiceFactory fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task should_save_the_item_on_single_service()
    {
        ISender sender = _fixture.SingleService.GetRequiredService<ISender>();

        SaveItem useCase = SaveItem.Prepare("test1");
        Result<Item> result = await sender.Send(useCase);

        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task should_save_item_and_retrieve_it_on_single_service()
    {
        // Arrange
        ISender sender = _fixture.SingleService.GetRequiredService<ISender>();
        SaveItem saveItem = SaveItem.Prepare("test2");
        GetAllItems getAllItems = GetAllItems.Prepare();

        // Act
        Result<Item> saveResult = await sender.Send(saveItem);
        Result<IEnumerable<Item>> getAllResult = await sender.Send(getAllItems);

        // Assert
        saveResult.IsSuccess.Should().BeTrue();
        getAllResult.IsSuccess.Should().BeTrue();
        getAllResult.Data.First().Content.Should().Be(new ItemContent("test2"));
    }

    [Fact]
    public async Task should_save_multiple_items_asyncronously_on_single_service()
    {
        // Arrange
        ISender sender = _fixture.SingleService.GetRequiredService<ISender>();

        SaveItem saveItem1 = SaveItem.Prepare("test3-a");
        SaveItem saveItem2 = SaveItem.Prepare("test3-b");
        SaveItem saveItem3 = SaveItem.Prepare("test3-c");

        Task<Result<Item>> saveTask1 = sender.Send(saveItem1);
        Task<Result<Item>> saveTask2 = sender.Send(saveItem2);
        Task<Result<Item>> saveTask3 = sender.Send(saveItem3);

        Task<Result<Item>> saveTask4 = sender.Send(saveItem1);
        Task<Result<Item>> saveTask5 = sender.Send(saveItem2);
        Task<Result<Item>> saveTask6 = sender.Send(saveItem3);

        await Task.WhenAll(saveTask1, saveTask2, saveTask3, saveTask4, saveTask5, saveTask6);

        saveTask1.Result.IsSuccess.Should().BeTrue();
        saveTask2.Result.IsSuccess.Should().BeTrue();
        saveTask3.Result.IsSuccess.Should().BeTrue();
        saveTask4.Result.IsSuccess.Should().BeFalse();
        saveTask5.Result.IsSuccess.Should().BeFalse();
        saveTask6.Result.IsSuccess.Should().BeFalse();
    }
}
