namespace ProcessSPACE.Core.Models;

public interface IProcessImpl
{
    IProcessRouter CreateRouter(Solver solver);
}

public abstract class SingleEffluentProcess : IProcessImpl {
    IProcessRouter IProcessImpl.CreateRouter(Solver solver) {
        throw new NotImplementedException();
    }
}
