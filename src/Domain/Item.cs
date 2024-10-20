using System.Runtime.CompilerServices;
using Domain.Ports;
using Kernel;

namespace Domain;

public record ItemId(int Value);

public record ItemContent(string Value);

public sealed class Item
{
    private Item() { }
    public ItemId Id { get; private init; }
    public ItemContent Content { get; private init; }
    public override string ToString() => $"{Id.Value}:{Content.Value}";

    public static async Task<Item> Create(ItemContent content, IIdentityPort identityPort)
    {
        Result<int> idResult = await identityPort.GetNextIdentity();
        if (idResult.IsFailure) throw new InvalidOperationException(idResult.Error.Description);
        return new Item { Id = new ItemId(idResult.Data), Content = content };
    }
}
