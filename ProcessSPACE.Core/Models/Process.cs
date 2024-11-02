namespace ProcessSPACE.Core.Models;

public class Process
{
    public int Id { get; internal init; }

    public string Name { get; set; } = string.Empty;

    public ProcessImpl Def { get; internal init; }

    internal Process(int id, ProcessImpl def) {
        Id = id;
        Def = def;
    }
}
