using Kernel;
using MediatR;

namespace Domain.UseCases;

public sealed class GetAllItems : IRequest<Result<IEnumerable<Item>>>
{
    private GetAllItems() { }
    public static GetAllItems Prepare() => new();
}
