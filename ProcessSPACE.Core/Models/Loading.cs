
using System.Diagnostics;

namespace ProcessSPACE.Core.Models;

/// <summary>
/// Mass loadings of consituents
/// </summary>
/// <remarks>
/// There are base constituents (Soluble BOD, Particulate BOD, etc.) and
/// aggregate loadings (Total BOD, Total Nitrogen, etc.).
/// Each base constituent must be provided during construction, so is marked as `required`
/// so that constructors don't miss new parameters when/if they are added.
/// If you don't need to provide all constituents, use the template constructors
/// and modify only the parameters you're interested in.
/// 
/// <example>
/// <code>
/// var onlyBOD = ProcessLoad.Empty() with { SolBOD = 200.0, PartBOD = 100.0 }
/// Console.Writeline($"TotBOD = {onlyBOD.TotBOD}") // "TotBOD = 300.0"
/// </code>
/// </example>
/// </remarks>
public record ProcessLoad
{
    #region Loadings
    /// <summary>
    /// Soluble BOD
    /// </summary>
    public
#if NET7_0_OR_GREATER
    required
#endif
    double SolBOD
    { get; init; }

    /// <summary>
    /// Particulate BOD Mass Loading
    /// </summary>
    public
#if NET7_0_OR_GREATER
    required
#endif
    double PartBOD
    { get; init; }

    /// <summary>
    /// Total BOD Mass Loading
    /// </summary>
    /// <remarks>
    /// Sum of <see cref="PartBOD"/> and <see cref="SolBOD"/>
    /// </remarks>
    public double TotBOD => SolBOD + PartBOD;

    /// <summary>
    /// Volatile Suspended Solids Mass Loading
    /// </summary>
    public
#if NET7_0_OR_GREATER
    required
#endif
    double VolSS
    { get; init; }

    /// <summary>
    /// Fixed Suspended Solids Mass Loading
    /// </summary>
    public
#if NET7_0_OR_GREATER
    required
#endif
    double InertSS
    { get; init; }

    /// <summary>
    /// Total Suspended Solids Mass Loading
    /// </summary>
    /// <remarks>
    /// Sum of <see cref="VolSS"/> and <see cref="InertSS"/>.
    /// </remarks>
    public double TotSS => VolSS + InertSS;

    public double VolSSFrac
    {
        get => VolSS / TotSS;
        init
        {
            SanitizeFraction(value);
            double tempTot = TotSS;
            VolSS = value * tempTot;
            InertSS = tempTot - VolSS;
        }
    }

    /// <summary>
    /// Ammonia and Ammonium Nitrogen
    /// </summary>
    public
#if NET7_0_OR_GREATER
    required
#endif
    double AmmN
    { get; init; }

    /// <summary>
    /// Total Kiejdhal Nitrogen
    /// </summary>
    /// <remarks>
    /// Sum of <see cref="AmmN"/> and <see cref="TotOrgN"/>
    /// </remarks>
    public double TotKN => AmmN + TotOrgN;

    /// <summary>
    /// Soluble Organic Nitrogen
    /// </summary>
    public
#if NET7_0_OR_GREATER
    required
#endif
    double SolOrgN
    { get; init; }

    /// <summary>
    /// Particulate Organic Nitrogen
    /// </summary>
    public
#if NET7_0_OR_GREATER
    required
#endif
    double PartOrgN
    { get; init; }

    /// <summary>
    /// Total Organic Nitrogen
    /// </summary>
    /// <remarks>
    /// Sum of <see cref="SolOrgN"/> and <see cref="PartOrgN"/>.
    /// </remarks>
    public double TotOrgN => SolOrgN + PartOrgN;

    /// <summary>
    /// Oxidized Nitrogen
    /// </summary>
    public
#if NET7_0_OR_GREATER
    required
#endif
    double NOx
    { get; init; }

    /// <summary>
    /// Total Nitrogen
    /// </summary>
    public double TotN => NOx + TotKN;

    /// <summary>
    /// Orthophosphate
    /// </summary>
    public
#if NET7_0_OR_GREATER
    required
#endif
    double OrtP
    { get; init; }

    /// <summary>
    /// Chemical Phosphorus
    /// </summary>
    public
#if NET7_0_OR_GREATER
    required
#endif
    double ChemP
    { get; init; }

    /// <summary>
    /// Soluble Organic Phosphorus
    /// </summary>
    public
#if NET7_0_OR_GREATER
    required
#endif
    double SolOrgP
    { get; init; }

    /// <summary>
    /// Particulate Organic Phosphorus
    /// </summary>
    public
#if NET7_0_OR_GREATER
    required
#endif
    double PartOrgP
    { get; init; }

    /// <summary>
    /// Total Organic Phosphorus
    /// </summary>
    /// <remarks>
    /// Sum of <see cref="PartOrgP" /> and <see cref="SolOrgP" />.
    /// </remarks>
    public double TotOrgP => SolOrgP + PartOrgP;

    /// <summary>
    /// Total Phosphorus
    /// </summary>
    /// <remarks>
    /// Sum of <see cref="TotOrgP" /> and <see cref="ChemP" />.
    /// </remarks>
    public double TotP => TotOrgP + ChemP;

    /// <summary>
    /// Alkalinity
    /// </summary>
    /// <remarks>
    /// Sum of HCO3-, CO32-, and OH- less pH
    /// </remarks>
    public
#if NET7_0_OR_GREATER
    required
#endif
    double Alk
    { get; init; }
    #endregion

    #region Constructors
    /// <summary>
    /// Smallest possible loading
    /// </summary>
    /// <remarks>
    /// Rather than using zero, use a small number so that fractionations, or
    /// ratios between loadings, can be represented even when the actual values
    /// haven't been set yet.
    /// <para>
    /// Use the `float` epsilon so that even small ratios can be expressed using doubles.
    /// </para>
    /// </remarks>
    protected const double MIN_VALUE = Single.Epsilon;

    /// <summary>
    /// Largest possible loading
    /// </summary>
    /// <remarks>
    /// Cap the largest loading to protect arithmetic from overflowing.
    /// </remarks>
    protected const double MAX_VALUE = Single.MaxValue;

    /// <summary>
    /// A new <see cref="ProcessRouterHandle"/> with all constituents set to a <see cref="MIN_VALUE">MIN_VALUE</see>.
    /// </summary>
    /// <returns></returns>
    public static ProcessLoad Empty() => new()
    {
        SolBOD = MIN_VALUE,
        PartBOD = MIN_VALUE,
        VolSS = MIN_VALUE,
        InertSS = MIN_VALUE,
        SolOrgN = MIN_VALUE,
        PartOrgN = MIN_VALUE,
        AmmN = MIN_VALUE,
        NOx = MIN_VALUE,
        ChemP = MIN_VALUE,
        OrtP = MIN_VALUE,
        SolOrgP = MIN_VALUE,
        PartOrgP = MIN_VALUE,
        Alk = MIN_VALUE,
    };
    #endregion

    #region Operators
    public static ProcessLoad operator +(ProcessLoad left, ProcessLoad right)
    {
        return new()
        {
            VolSS = left.VolSS + right.VolSS,
            InertSS = left.InertSS + right.InertSS,
            SolBOD = left.SolBOD + right.SolBOD,
            PartBOD = left.PartOrgP + right.PartOrgP,
            AmmN = left.AmmN + right.AmmN,
            NOx = left.NOx + right.NOx,
            SolOrgN = left.SolOrgN + right.SolOrgN,
            PartOrgN = left.PartOrgN + right.PartOrgN,
            ChemP = left.ChemP + right.ChemP,
            OrtP = left.OrtP + right.OrtP,
            SolOrgP = left.SolOrgP + right.SolOrgP,
            PartOrgP = left.PartOrgP + right.PartOrgP,
            Alk = left.Alk + right.Alk,
        };
    }

    public static ProcessLoad operator -(ProcessLoad left, ProcessLoad right)
    {
        return new()
        {
            VolSS = left.VolSS - right.VolSS,
            InertSS = left.InertSS - right.InertSS,
            SolBOD = left.SolBOD - right.SolBOD,
            PartBOD = left.PartOrgP - right.PartOrgP,
            AmmN = left.AmmN - right.AmmN,
            NOx = left.NOx - right.NOx,
            SolOrgN = left.SolOrgN - right.SolOrgN,
            PartOrgN = left.PartOrgN - right.PartOrgN,
            ChemP = left.ChemP - right.ChemP,
            OrtP = left.OrtP - right.OrtP,
            SolOrgP = left.SolOrgP - right.SolOrgP,
            PartOrgP = left.PartOrgP - right.PartOrgP,
            Alk = left.Alk - right.Alk,
        };
    }

    public static ProcessLoad operator *(ProcessLoad load, double factor)
    {
        if (factor < MIN_VALUE)
        {
            return Empty();
        }

        return new()
        {
            VolSS = load.VolSS * factor,
            InertSS = load.InertSS * factor,
            SolBOD = load.SolBOD * factor,
            PartBOD = load.PartOrgP * factor,
            AmmN = load.AmmN * factor,
            NOx = load.NOx * factor,
            SolOrgN = load.SolOrgN * factor,
            PartOrgN = load.PartOrgN * factor,
            ChemP = load.ChemP * factor,
            OrtP = load.OrtP * factor,
            SolOrgP = load.SolOrgP * factor,
            PartOrgP = load.PartOrgP * factor,
            Alk = load.Alk * factor,
        };
    }

    public static ProcessLoad operator *(double factor, ProcessLoad load) => load * factor;
    #endregion

    #region Utilities
    [Conditional("DEBUG")]
    static void SanitizeFraction(double fraction)
    {
        if (fraction >= 1.0)
        {
            throw new ArgumentOutOfRangeException();
        }

        if (fraction <= 0.0)
        {
            throw new ArgumentOutOfRangeException();
        }
    }
    #endregion
}
