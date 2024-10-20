using Kernel;
using MediatR;

namespace Domain.UseCases;

public sealed class SaveItem : IRequest<Result<Item>>
{
    private SaveItem(string content)
    {
        Content = content;
    }

    public string Content { get; }
    public static SaveItem Prepare(string content) => new(content);
}
