namespace ProcessSPACE.Core.Models;

public readonly record struct ProcessId(int Value)
{
    public static implicit operator int(ProcessId p) => p.Value;
    public static implicit operator ProcessId(int id) => new(id);
}

public class Process
{
    public ProcessId Id { get; internal init; }

    public string Name { get; set; } = string.Empty;

    public IProcessImpl ProcessSolver { get; set; }

    internal Process(int id, IProcessImpl process)
    {
        Id = id;
        ProcessSolver = process;
    }
}
