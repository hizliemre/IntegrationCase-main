using System.Collections.Concurrent;
using Application.Providers;
using Kernel;

namespace Infrastructure.Providers;

internal sealed class SingleServiceLockProvider : ILockProvider
{
    private readonly ConcurrentDictionary<string, SemaphoreSlim> _lock = new();

    public Result<bool> Acquire(string lockName)
    {
        SemaphoreSlim semaphore = new(1, 1);
        if (_lock.TryAdd(lockName, semaphore))
        {
            semaphore.Wait();
            return Result<bool>.Success(true);
        }
        Error error = new("LOCK", "Failed to acquire lock");
        return Result<bool>.Failure(error);
    }

    public Result<bool> Release(string lockName)
    {
        if (_lock.TryRemove(lockName, out SemaphoreSlim? semaphore))
        {
            semaphore.Release();
            return Result<bool>.Success(true);
        }
        Error error = new("LOCK", "Failed to release lock");
        return Result<bool>.Failure(error);
    }
}
