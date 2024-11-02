using QuikGraph;
using QuikGraph.Algorithms;

namespace ProcessSPACE.Core.Models;

public class ModelManager
{
    private AdjacencyGraph<Process, Connection> _graph = new();

    public bool AddProcess(Process process) {
        return _graph.AddVertex(process);
    }

    public bool AddConnection(Connection conn)
    {
        return _graph.AddEdge(conn);
    }
}