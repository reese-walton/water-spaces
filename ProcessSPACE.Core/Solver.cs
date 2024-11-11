using System;
using System.Collections.Generic;
using System.Reflection;
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
    private Matrix<double> HydraulicsMatrix;
    private Matrix<double> ProcessMatrix;

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
        EnsureSize(handle.Router.Value);
        int index = handle.Router.Value * NumBaseParameters + parameter.AsOffset();
        ProcessMatrix.SetRow(index, definition.Parameters);
    }

    private void EnsureSize(int linkCount)
    {
        if (HydraulicsMatrix.RowCount < linkCount || ProcessMatrix.RowCount < linkCount * NumBaseParameters)
        {
            linkCount = Math.Max(linkCount, Routers.Count);
            HydraulicsMatrix = HydraulicsMatrix.Resize(linkCount, linkCount);
            int paramCount = linkCount * NumBaseParameters;
            ProcessMatrix = ProcessMatrix.Resize(paramCount, paramCount);
        }
    }
}

public class ParameterDefinition
{
    readonly internal Vector<double> Parameters;

    private ParameterDefinition(Vector<double> parameters) => this.Parameters = parameters;

    internal ParameterDefinition(ParameterIndex index, double factor = 0)
    {
        Parameters = Vector<double>.Build.Sparse();
        Parameters[index.Index.Value * NumBaseParameters + index.Parameter.AsOffset()] = factor;
    }

    public static ParameterDefinition operator +(ParameterDefinition left, ParameterDefinition right)
    {
        return new ParameterDefinition(left.Parameters + right.Parameters);
    }

    public static ParameterDefinition operator *(double factor, ParameterDefinition parameter)
    {
        return new ParameterDefinition(factor * parameter.Parameters);
    }
    public static ParameterDefinition operator *(ParameterDefinition parameter, double factor) => factor * parameter;
}
