using ProcessSPACE.Core;
using ProcessSPACE.Core.Models;
using MathNet.Numerics.LinearAlgebra;

Console.WriteLine("ProcessSPACE v2");

var vec1 = Vector<double>.Build.Sparse(5);
vec1[0] = 1.0;
vec1[4] = 5.0;

var vec2 = Vector<double>.Build.Sparse(5);
vec2[1] = 1.0;
vec2[3] = 5.0;

Console.WriteLine(vec1 + vec2);