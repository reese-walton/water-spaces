using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using MathNet.Numerics.LinearAlgebra;
using ProcessSPACE.Core.Models;

using static ProcessSPACE.Core.Models.ParameterExtensions;

namespace ProcessSPACE.Core;

/// <summary>
/// A 'pointer' to a column in the solver matrix where the first parameter
/// of the router can be found.
/// </summary>
internal record struct RouterIndex(int Value);

internal record struct ParameterIndex(RouterIndex Index, BaseParameter Parameter);

public class Solver
{
    private readonly Matrix<double> HydraulicsMatrix;
    private readonly Matrix<double> ProcessMatrix;

    private readonly HashSet<RouterIndex> Routers = new();

    public Solver(int numConnections) : this(
        CreateMatrix.Sparse<double>(numConnections, numConnections),
        CreateMatrix.Sparse<double>(numConnections * NumBaseParameters, numConnections * NumBaseParameters)
    )
    { }

    public Solver(Matrix<double> hydraulicsMatrix, Matrix<double> processMatrix)
    {
        HydraulicsMatrix = hydraulicsMatrix;
        ProcessMatrix = processMatrix;
    }

    public ProcessRouterHandle RegisterEffluent()
    {
        // record that the index exists, but defer increasing size of matrix
        // until we are ready to calculate (do it all at once)
        RouterIndex index = new(Routers.Count);
        Routers.Add(index);
        return new ProcessRouterHandle(this, index);
    }

    internal void DefineParameter(ProcessRouterHandle handle, BaseParameter parameter, ParameterDefinition definition)
    {
        throw new NotImplementedException();
    }
}

public class ParameterDefinition
{
    readonly Dictionary<ParameterIndex, double> Parameters;

    private ParameterDefinition(Dictionary<ParameterIndex, double> parameters) => this.Parameters = parameters;

    internal ParameterDefinition(ParameterIndex index, double factor = 0) => Parameters = new() {
            { index, factor }
        };

    public static ParameterDefinition operator +(ParameterDefinition left, ParameterDefinition right)
    {
        IEnumerable<ParameterIndex> keys = left.Parameters.Keys.Union(right.Parameters.Keys);
        Dictionary<ParameterIndex, double> newParams = new(keys.Count());
        foreach (ParameterIndex key in keys)
        {
            left.Parameters.TryGetValue(key, out double leftVal);
            right.Parameters.TryGetValue(key, out double rightVal);
            newParams.Add(key, leftVal + rightVal);
        }

        return new ParameterDefinition(newParams);
    }

    public static ParameterDefinition operator *(double factor, ParameterDefinition parameter)
    {
        Dictionary<ParameterIndex, double> newDict = new(parameter.Parameters);

        foreach (ParameterIndex key in newDict.Keys)
        {
            newDict[key] *= factor;
        }
        return new ParameterDefinition(newDict);
    }
    public static ParameterDefinition operator *(ParameterDefinition parameter, double factor) => factor * parameter;
}
