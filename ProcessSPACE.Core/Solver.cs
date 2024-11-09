using System;
using System.Collections.Generic;
using MathNet.Numerics.LinearAlgebra;
using ProcessSPACE.Core.Models;

namespace ProcessSPACE.Core;

public class Solver
{
    private Matrix<double> matrix;

    public Solver() : this(CreateMatrix.Sparse<double>(12, 12)) { }

    public Solver(Matrix<double> matrix) => this.matrix = matrix;

    public void AddExpression(ProcessParameter item, ProcessParameter def) { }
}
