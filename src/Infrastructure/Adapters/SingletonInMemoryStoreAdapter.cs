using System.Collections.Concurrent;
using Domain;
using Domain.Ports;
using Kernel;

namespace Infrastructure.Adapters;

public sealed class SingletonInMemoryStoreAdapter : IPersistencyPort, IRetrievePort
{
    private readonly ConcurrentBag<Item> _items = new();

    public Task<Result<Item>> Save(Item item)
    {
        _items.Add(item);
        return Task.FromResult(Result<Item>.Success(item));
    }

    public Task<Result<IEnumerable<Item>>> GetAllItems()
    {
        Result<IEnumerable<Item>> result = Result<IEnumerable<Item>>.Success(_items.ToList());
        return Task.FromResult(result);
    }

    public Task<Result<IEnumerable<Item>>> FindItemsWithContent(ItemContent itemContent)
    {
        List<Item> result = _items.Where(item => item.Content == itemContent).ToList();
        return Task.FromResult(Result<IEnumerable<Item>>.Success(result));
    }

    public Task<Result<bool>> HasItem(ItemContent itemContent)
    {
        bool hasItem = _items.Any(item => item.Content == itemContent);
        return Task.FromResult(Result<bool>.Success(hasItem));
    }
}
