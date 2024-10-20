using Domain;
using Domain.Ports;
using Domain.UseCases;
using Kernel;
using MediatR;

namespace Application.UseCaseHandlers;

internal sealed class GetAllItemsHandler : IRequestHandler<GetAllItems, Result<IEnumerable<Item>>>
{
    private readonly IRetrievePort _retrievePort;

    public GetAllItemsHandler(IRetrievePort retrievePort)
    {
        _retrievePort = retrievePort;
    }

    public async Task<Result<IEnumerable<Item>>> Handle(GetAllItems request, CancellationToken cancellationToken)
    {
        Result<IEnumerable<Item>> result = await _retrievePort.GetAllItems();
        return result;
    }
}
