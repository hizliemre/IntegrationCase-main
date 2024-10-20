using Kernel;

namespace Domain.Ports;

public interface IRetrievePort
{
    public Task<Result<IEnumerable<Item>>> GetAllItems();
    public Task<Result<IEnumerable<Item>>> FindItemsWithContent(ItemContent itemContent);
    public Task<Result<bool>> HasItem(ItemContent itemContent);
}
