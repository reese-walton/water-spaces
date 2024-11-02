using QuikGraph;

namespace ProcessSPACE.Core.Models;

public class Connection : IEdge<Process>
{
    public int Id { get; init;}

    public Process Upstream {get; init;}
    
    public Process Downstream {get; init;}

    public Process Source => Upstream;

    public Process Target => Downstream;

    internal Connection(int id, Process upstream, Process downstream)
    {
        Id = id;
        Upstream = upstream;
        Downstream = downstream;
    }
}

