using static ProcessSPACE.Core.Models.BaseParameter;

namespace ProcessSPACE.Core.Models;

public interface IProcessRouter
{
    void DefineParameters(IEnumerable<ProcessRouterHandle> flows);
}

public class ProcessRouterHandle
{
    internal readonly Solver Owner;

    internal readonly RouterIndex Router;

    internal ProcessRouterHandle(Solver solver, RouterIndex router)
    {
        Owner = solver;
        Router = router;
    }

    public ParameterDefinition this[BaseParameter bp] {
        get => new(new ParameterIndex(Router, bp), 1.0);
        set
        {
            Owner.DefineParameter(this, bp, value);
        }
    }
    public double Flow = 0;
}

public abstract class SingleEffluentRouter : IProcessRouter
{
    protected ProcessRouterHandle Effluent {get;}
    
    public SingleEffluentRouter(Solver solver) {
        Effluent = solver.RegisterEffluent();
    }

    protected abstract void RouteSS(IEnumerable<ProcessRouterHandle> flows);

    public void DefineParameters(IEnumerable<ProcessRouterHandle> flows)
    {
        
    }
}

public class CompleteMixRouter : SingleEffluentRouter
{
    public CompleteMixRouter(Solver solver): base(solver) {}
    
    protected override void RouteSS(IEnumerable<ProcessRouterHandle> flows) {
        var inflow1 = flows.First();
        Effluent[InpSSVol] = inflow1[InpSSVol] * (Effluent.Flow / inflow1.Flow);
    }
}
