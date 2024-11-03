using QuikGraph;

namespace ProcessSPACE.Core.Models;

public readonly record struct ConnectionId(int Value)
{
    public static implicit operator int(ConnectionId p) => p.Value;
    public static implicit operator ConnectionId(int id) => new(id);
}

public class Connection
{
    public ConnectionId Id { get; init; }

    public ProcessId Upstream { get; init; }

    public ProcessId Downstream { get; init; }

    internal Connection(ConnectionId id, ProcessId upstream, ProcessId downstream)
    {
        Id = id;
        Upstream = upstream;
        Downstream = downstream;
    }
}

