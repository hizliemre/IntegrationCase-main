using Application.Services;
using Domain;
using Domain.Ports;
using Kernel;

namespace Infrastructure.Services;

internal sealed class ItemOperationBackendService : IItemOperationBackendService
{
    private readonly IIdentityPort _identityPort;
    private readonly IPersistencyPort _persistencyPort;
    private readonly IRetrievePort _retrievePort;

    public ItemOperationBackendService(IIdentityPort identityPort, IPersistencyPort persistencyPort, IRetrievePort retrievePort)
    {
        _identityPort = identityPort;
        _persistencyPort = persistencyPort;
        _retrievePort = retrievePort;
    }

    public async Task<Result<IEnumerable<Item>>> FindItemsWithContent(string itemContent)
    {
        ItemContent content = new(itemContent);

        Result<IEnumerable<Item>> result = await _retrievePort.FindItemsWithContent(content);

        return result;
    }

    public async Task<Result<IEnumerable<Item>>> GetAllItems()
    {
        Result<IEnumerable<Item>> result = await _retrievePort.GetAllItems();

        return result;
    }

    public async Task<Result<Item>> Save(string itemContent)
    {
        // This simulates how long it takes to save
        // the item content. Forty seconds, give or take.
        Thread.Sleep(2_000);

        ItemContent content = new(itemContent);

        Result<bool> hasItem = await _retrievePort.HasItem(content);

        if (hasItem.IsFailure) return Result<Item>.Failure(hasItem.Error);

        if (hasItem is { IsSuccess: true, Data: true })
        {
            Error error = new("DUPLICATE", "Item already exists.");
            return Result<Item>.Failure(error);
        }

        Item item = await Item.Create(content, _identityPort);
        Result<Item> result = await _persistencyPort.Save(item);

        return result;
    }
}
