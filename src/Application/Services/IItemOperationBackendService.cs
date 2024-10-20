using Domain;
using Kernel;

namespace Application.Services;

public interface IItemOperationBackendService
{
    public Task<Result<Item>> Save(string itemContent);
    public Task<Result<IEnumerable<Item>>> GetAllItems();
    public Task<Result<IEnumerable<Item>>> FindItemsWithContent(string itemContent);
}
