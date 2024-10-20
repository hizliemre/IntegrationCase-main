using Application.Providers;
using Kernel;
using StackExchange.Redis;

namespace Infrastructure.Providers;

internal sealed class RedisDistributedLockProvider : ILockProvider
{
    private readonly string _lockValue;
    private readonly IDatabase _redisDb;

    public RedisDistributedLockProvider(IConnectionMultiplexer redis)
    {
        _redisDb = redis.GetDatabase();
        _lockValue = Guid.NewGuid().ToString();
    }

    public Result<bool> Acquire(string lockName)
    {
        bool result = _redisDb.LockTake(lockName, _lockValue, TimeSpan.FromSeconds(10));
        return result ? Result<bool>.Success(true) : Result<bool>.Failure(new Error("LOCK", "Failed to acquire lock"));
    }

    public Result<bool> Release(string lockName)
    {
        bool result = _redisDb.LockRelease(lockName, _lockValue);
        return result ? Result<bool>.Success(true) : Result<bool>.Failure(new Error("LOCK", "Failed to release lock"));
    }
}
