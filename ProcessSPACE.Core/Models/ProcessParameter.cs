using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static ProcessSPACE.Core.Models.BaseParameter;
using static ProcessSPACE.Core.Models.ProcessParameter;

namespace ProcessSPACE.Core.Models;
public struct ProcessLoadings
{
    readonly Solver Owner;
    
    readonly Solver.RouterIndex Router;

    internal ProcessLoadings(Solver solver, Solver.RouterIndex router) {
        Owner = solver;
        Router = router;
    }
}

public enum BaseParameter
{
    /// <summary>
    /// Input volatile suspended solids
    /// </summary>
    InpSSVol = 0,

    /// <summary>
    /// Input inert suspended solids
    /// </summary>
    InpSSInt,

    /// <summary>
    /// Input soluble BOD
    /// </summary>
    InpBODSol,

    /// <summary>
    /// Input particulate BOD
    /// </summary>
    InpBODPrt,

    /// <summary>
    /// Input ammonia nitrogen
    /// </summary>
    InpNAmm,

    /// <summary>
    /// Input soluble organic nitrogen
    /// </summary>
    InpNOrgSol,

    /// <summary>
    /// Input particulate organic nitrogen
    /// </summary>
    InpNOrgPrt,

    /// <summary>
    /// Input oxidized nitrogen
    /// </summary>
    InpNOxd,

    /// <summary>
    /// Input soluble organic phosphorus
    /// </summary>
    InpPOrgSol,

    /// <summary>
    /// Input particulate organic phosphorus
    /// </summary>
    InpPOrgPrt,

    /// <summary>
    /// Input chemical phosphorus
    /// </summary>
    InpPChm,

    /// <summary>
    /// Input orthophosphate
    /// </summary>
    InpPOrt,

    /// <summary>
    /// Input alkalinity
    /// </summary>
    InpAlk,

    /// <summary>
    /// Any other input parameter
    /// </summary>
    InpOther
}

public enum ProcessParameter
{
    /// <summary>
    /// Volatile suspended solids
    /// </summary>
    SSVol = 1 << InpSSVol,

    /// <summary>
    /// Inert suspended solids
    /// </summary>
    SSInt = 1 << InpSSInt,

    /// <summary>
    /// Total suspended solids
    /// </summary>
    SSTot = SSVol | SSInt,
    
    /// <summary>
    /// Soluble BOD
    /// </summary>
    BODSol = 1 << InpBODSol,

    /// <summary>
    /// Particulate BOD
    /// </summary>
    BODPrt = 1 << InpBODPrt,
    
    /// <summary>
    /// Total BOD
    /// </summary>
    BODTot = BODSol | BODPrt,

    /// <summary>
    /// Ammonia nitrogen
    /// </summary>
    NAmm = 1 << InpNAmm,

    /// <summary>
    /// Soluble organic nitrogen
    /// </summary>
    NOrgSol = 1 << InpNOrgSol,

    /// <summary>
    /// Particulate organic nitrogen
    /// </summary>
    NOrgPrt = 1 << InpNOrgPrt,

    /// <summary>
    /// Total organic nitrogen
    /// </summary>
    NOrgTot = NOrgSol | NOrgPrt,

    /// <summary>
    /// Total Kjedahl Nitrogen
    /// </summary>
    NTkn = NAmm | NOrgTot,

    /// <summary>
    /// Oxidized nitrogen (NO2 and NO3)
    /// </summary>
    NOxd = 1 << InpNOxd,

    /// <summary>
    /// Total nitrogen
    /// </summary>
    NTot = NTkn | NOxd,

    /// <summary>
    /// Organic soluble phosphorus
    /// </summary>
    POrgSol = 1 << InpPOrgSol,

    /// <summary>
    /// Organic particulate phosphorus
    /// </summary>
    POrgPrt = 1 << InpPOrgPrt,

    /// <summary>
    /// Total organic phosphorus
    /// </summary>
    POrgTot = POrgSol | POrgPrt,

    /// <summary>
    /// Chemical phosphorus
    /// </summary>
    PChm = 1 << InpPChm,

    /// <summary>
    /// Orthophosphate
    /// </summary>
    POrt = 1 << InpPOrt,

    /// <summary>
    /// Total phosphorus
    /// </summary>
    PTot = POrgTot | PChm | POrt,

    /// <summary>
    /// Alkalinity
    /// </summary>
    Alk = 1 << InpAlk,

    /// <summary>
    /// Any other constituent
    /// </summary>
    Other = 1 << InpOther,
}

public static class ParameterExtensions
{
    public const int NumBaseParameters = (int) InpOther + 1;

    public const int NumProcessParameters = 22;
}