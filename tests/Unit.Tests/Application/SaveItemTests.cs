using Application;
using Application.Providers;
using Application.Services;
using Domain;
using Domain.UseCases;
using FluentAssertions;
using Kernel;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xunit;

namespace Unit.Tests.Application;

public class SaveItem_use_case
{
    [Fact]
    public async Task should_be_run_the_its_own_handler()
    {
        // Arrange
        IServiceCollection serviceCollection = new ServiceCollection();
        Mock<IItemOperationBackendService> mockService = new();
        mockService.Setup(m => m.Save(It.IsAny<string>())).Returns(Task.FromResult(Result<Item>.Success(null)));
        Mock<ILockProvider> mockLockProvider = new();
        mockLockProvider.Setup(m => m.Acquire(It.IsAny<string>())).Returns(Result<bool>.Success(true));
        mockLockProvider.Setup(m => m.Release(It.IsAny<string>())).Returns(Result<bool>.Success(true));
        serviceCollection.AddSingleton(mockService.Object);
        serviceCollection.AddSingleton(mockLockProvider.Object);
        serviceCollection.AddMediatR();
        ServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();
        ISender sender = serviceProvider.GetRequiredService<ISender>();

        // Act
        SaveItem useCase = SaveItem.Prepare("a");
        Result<Item> result = await sender.Send(useCase);

        // Assert
        result.IsSuccess.Should().BeTrue();
    }
}
