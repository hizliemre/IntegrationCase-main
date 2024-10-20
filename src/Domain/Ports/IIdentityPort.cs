using Kernel;

namespace Domain.Ports;

public interface IIdentityPort
{
    public Task<Result<int>> GetNextIdentity();
}
