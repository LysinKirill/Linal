namespace Linal;

public class EuclideanSpace : LinearSpace
{
    private readonly Func<Vector, Vector, Fraction> _dotProduct;
    
    public EuclideanSpace(List<Vector> vectors, Func<Vector, Vector, Fraction>? dotProduct = null) : base(vectors)
    {
        _dotProduct = dotProduct ?? DefaultDotProduct;
    }
    
    public EuclideanSpace(Vector[] vectors, Func<Vector, Vector, Fraction>? dotProduct = null) : base(vectors)
    {
        _dotProduct = dotProduct ?? DefaultDotProduct;
    }
    
    public EuclideanSpace(Matrix basisMatrix, Func<Vector, Vector, Fraction>? dotProduct = null) : base(basisMatrix)
    {
        _dotProduct = dotProduct ?? DefaultDotProduct;
    }

    public EuclideanSpace(LinearSpace linearSpace, Func<Vector, Vector, Fraction>? dotProduct = null) : base(linearSpace.Basis)
    {
        _dotProduct = dotProduct ?? DefaultDotProduct;
    }

    public double GetLength(Vector vector)
    {
        if (!ContainsVector(vector))
            throw new ArgumentException($"The given vector {vector} does not belong to this euclidean space.");
        return Math.Sqrt(_dotProduct(vector, vector).GetDouble());
    }

    public static Fraction GetCosSqr(Vector x, Matrix systemMatrix, Vector b)
    {
        var gram = Gram(systemMatrix.Transpose());
        var xShtrix = systemMatrix * x - b;
        var temp = Matrix.ConcatColumns(gram, xShtrix);
        var tmp = temp.Canonical().GetColumn(temp.Columns - 1).Transpose();
        var xOrtogonal = new Vector((tmp * systemMatrix).Transpose());
        return DefaultDotProduct(x, xOrtogonal) * DefaultDotProduct(x, xOrtogonal) /
               (DefaultDotProduct(x, x) * DefaultDotProduct(xOrtogonal, xOrtogonal));
    }

    public double GetCos(Vector v1, Vector v2) => _dotProduct(v1, v2).GetDouble() / (GetLength(v1) * GetLength(v2));
    public double GetAngle(Vector v1, Vector v2) => Math.Acos(GetCos(v1, v2));

    public Matrix Gram(List<Vector> vectors)
    {
        var n = vectors.Count;
        Matrix gram = new Matrix(n, n);
        for (int i = 0; i < n; ++i)
        for (int j = 0; j < n; ++j)
            gram[i, j] = _dotProduct(vectors[i], vectors[j]);
        return gram;
    }
    
    public static Matrix Gram(List<Vector> vectors, Func<Vector, Vector, Fraction>? dotProduct = null)
    {
        dotProduct ??= DefaultDotProduct;
        var n = vectors.Count;
        var gram = new Matrix(n, n);
        for (int i = 0; i < n; ++i)
        for (int j = 0; j < n; ++j)
            gram[i, j] = dotProduct(vectors[i], vectors[j]);
        return gram;
    }

    public static Matrix Gram(Matrix matrix, Func<Vector, Vector, Fraction>? dotProduct = null)
    {
        return Gram(matrix.GetVerticalVectors(), dotProduct);
    }

    public Matrix Gram() => Gram(Basis);

    public Fraction Gramian() => Gram().Det();
    public Fraction Gramian(List<Vector> vectors) => Gram(vectors).Det();

    public double GetVolume(List<Vector> vectors) => Math.Sqrt(Gramian(vectors).GetDouble());

    public Fraction DistSqr(Vector vector)
    {
        var vectors = Basis.Select(x => x).ToList();
        vectors.Add(vector);
        return Gramian(vectors) / Gramian();
    }

    public static Fraction DistSqr(Vector x, Matrix systemMatrix, Vector b)
    {
        var gram = Gram(systemMatrix.Transpose());
        var xShtrix = systemMatrix * x - b;
        var v = Matrix.ConcatColumns(gram, xShtrix);
        var g = xShtrix.Transpose().AddColumns(false, new Vector(false, 0));
        var M = v.AddRows(false, g);
        return Fraction.Abs(M.Det() / gram.Det());
    }

    public double Dist(Vector vector) => Math.Sqrt(DistSqr(vector).GetDouble());
    
    public EuclideanSpace Orthogonalize(bool print = false)
    {
        var newBasis = new List<Vector>(Basis.Count);
        for (var i = 0; i < Basis.Count; i++)
        {
            var a = Basis[i];
            var b = a.Copy();
            if (print)
            {
                Console.Write($"b{i} = {a} ");
            }
            foreach (var prevB in newBasis)
            {
                var temp = prevB * (_dotProduct(prevB, a) / _dotProduct(prevB, prevB));
                b -= temp;
                if (print)
                {
                    Console.Write($"- {temp} ");
                }
            }

            if (print)
            {
                Console.WriteLine($"= {b}");
            }
            newBasis.Add(b);
        }

        return new EuclideanSpace(newBasis, _dotProduct);
    }

    public Fraction DotProduct(Vector v1, Vector v2)
    {
        return _dotProduct(v1, v2);
    }

    private static readonly Func<Vector, Vector, Fraction> DefaultDotProduct = (v1, v2) =>
    {
        if (v1.Rows != v2.Rows)
            throw new ArgumentException("Unable to compute dot product of vectors with different dimensions");
        var res = new Fraction(0);
        for (int i = 0; i < v1.Rows; i++)
        {
            res += v1[i, 0] * v2[i, 0];
        }

        return res;
    };
}