using Domain.Ports;
using Kernel;

namespace Infrastructure.Adapters;

public sealed class SingletonIdentityAdapter : IIdentityPort
{
    private int _currentIdentity;

    public Task<Result<int>> GetNextIdentity()
    {
        int nextIdentity = Interlocked.Increment(ref _currentIdentity);
        return Task.FromResult(Result<int>.Success(nextIdentity));
    }
}
