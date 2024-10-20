using Application.Providers;
using Application.Services;
using Domain;
using Domain.UseCases;
using Kernel;
using MediatR;

namespace Application.UseCaseHandlers;

internal sealed class SaveItemHandler : IRequestHandler<SaveItem, Result<Item>>
{
    private readonly ILockProvider _lockProvider;
    private readonly IItemOperationBackendService _service;

    public SaveItemHandler(IItemOperationBackendService service, ILockProvider lockProvider)
    {
        _service = service;
        _lockProvider = lockProvider;
    }

    public async Task<Result<Item>> Handle(SaveItem request, CancellationToken cancellationToken)
    {
        string lockName = $"lock:{request.Content}";
        try
        {
            Result<bool> lockResult = _lockProvider.Acquire(lockName);
            if (lockResult.IsFailure) return Result<Item>.Failure(lockResult.Error);

            Result<Item> result = await _service.Save(request.Content);
            return result.IsFailure ? Result<Item>.Failure(result.Error) : result;
        }
        finally
        {
            Result<bool> releseResult = _lockProvider.Release(lockName);
            if (releseResult.IsFailure)
            {
                /* Log error */
            }
        }
    }
}
