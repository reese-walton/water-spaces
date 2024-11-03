using System.Collections.Generic;
using QuikGraph;

namespace ProcessSPACE.Core.Models;

public class ModelManager
{
    readonly record struct ModelConnection(ProcessId Source, ProcessId Target) : IEdge<ProcessId>
    {
        public ModelConnection(Connection conn) : this(conn.Upstream, conn.Downstream) { }
    }

    private readonly AdjacencyGraph<ProcessId, ModelConnection> _graph = new();
    private readonly Dictionary<ProcessId, Process> _processes = new();
    private readonly Dictionary<ConnectionId, Connection> _connections = new();

    public bool AddProcess(Process process)
    {
        bool added = _graph.AddVertex(process.Id);
        if (added)
        {
            _processes.Add(process.Id, process);
        }
        return added;
    }

    public Process? GetProcess(ProcessId id)
    {
        bool _ = _processes.TryGetValue(id, out Process? result);
        return result;
    }

    public Process? RemoveProcess(ProcessId id)
    {
        bool _ = _processes.Remove(id, out Process? result);
        return result;
    }

    public bool AddConnection(Connection conn)
    {
        bool added = _graph.AddEdge(new ModelConnection(conn));
        if (added)
        {
            _connections.Add(conn.Id, conn);
        }
        return added;
    }

    public Connection? GetConnection(ConnectionId id)
    {
        bool _ = _connections.TryGetValue(id, out Connection? result);
        return result;
    }

    public Connection? RemoveConnection(ConnectionId id)
    {
        if (_connections.Remove(id, out Connection? result))
        {
            bool _ = _graph.RemoveEdge(new ModelConnection(result));
        }
        return result;
    }
}
