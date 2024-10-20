using Domain.Ports;
using Kernel;

namespace Unit.Tests.FakeAdapters;

internal class FakeIdentityAdapter : IIdentityPort
{
    private int _currentIdentity;
    public int GetNextIdentityCall_Called;

    public Task<Result<int>> GetNextIdentity()
    {
        int nextIdentity = Interlocked.Increment(ref _currentIdentity);
        GetNextIdentityCall_Called++;
        return Task.FromResult(Result<int>.Success(nextIdentity));
    }
}
