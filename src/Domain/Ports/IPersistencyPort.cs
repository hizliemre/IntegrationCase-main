using Kernel;

namespace Domain.Ports;

public interface IPersistencyPort
{
    public Task<Result<Item>> Save(Item item);
}
