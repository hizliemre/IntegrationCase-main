using Kernel;

namespace Application.Providers;

public interface ILockProvider
{
    public Result<bool> Acquire(string lockName);
    public Result<bool> Release(string lockName);
}
