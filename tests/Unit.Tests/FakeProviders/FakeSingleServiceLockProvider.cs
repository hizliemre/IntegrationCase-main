using System.Collections.Concurrent;
using Application.Providers;
using Kernel;

internal class FakeSingleServiceLockProvider : ILockProvider
{
    private readonly ConcurrentDictionary<string, SemaphoreSlim> _lock = new();
    public int Acquire_Called;
    public int Release_Called;

    public Result<bool> Acquire(string lockName)
    {
        SemaphoreSlim semaphore = new(1, 1);
        if (_lock.TryAdd(lockName, semaphore)) return Result<bool>.Success(true);
        Error error = new("LOCK", "Failed to acquire lock");
        Acquire_Called++;
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
        Release_Called++;
        return Result<bool>.Failure(error);
    }
}
