namespace Linal;

public class EuclideanSpace : LinearSpace
{
    private readonly Func<Matrix, Matrix, Fraction> _dotProduct;
    
    public EuclideanSpace(List<Matrix> vectors, Func<Matrix, Matrix, Fraction>? dotProduct = null) : base(vectors)
    {
        _dotProduct = dotProduct ?? DefaultDotProduct;
    }
    
    public EuclideanSpace(Matrix[] vectors, Func<Matrix, Matrix, Fraction>? dotProduct = null) : base(vectors)
    {
        _dotProduct = dotProduct ?? DefaultDotProduct;
    }
    
    public EuclideanSpace(Matrix basisMatrix, Func<Matrix, Matrix, Fraction>? dotProduct = null) : base(basisMatrix)
    {
        _dotProduct = dotProduct ?? DefaultDotProduct;
    }

    public EuclideanSpace(LinearSpace linearSpace, Func<Matrix, Matrix, Fraction>? dotProduct = null) : base(linearSpace.Basis)
    {
        _dotProduct = dotProduct ?? DefaultDotProduct;
    }

    public double GetLength(Matrix vector)
    {
        if (!ContainsVector(vector))
            throw new ArgumentException("The given vector does not belong to this euclidean space.");
        return Math.Sqrt(_dotProduct(vector, vector).GetDouble());
    }

    public double GetCos(Matrix v1, Matrix v2) => _dotProduct(v1, v2).GetDouble() / (GetLength(v1) * GetLength(v2));
    public double GetAngle(Matrix v1, Matrix v2) => Math.Acos(GetCos(v1, v2));

    public Matrix Gram(List<Matrix> vectors)
    {
        int n = vectors.Count;
        Matrix gram = new Matrix(n, n);
        for (int i = 0; i < n; ++i)
        for (int j = 0; j < n; ++j)
            gram[i, j] = _dotProduct(vectors[i], vectors[j]);
        return gram;
    }

    public Matrix Gram() => Gram(Basis);

    public Fraction Gramian() => Gram().Det();
    public Fraction Gramian(List<Matrix> vectors) => Gram(vectors).Det();

    public double GetVolume(List<Matrix> vectors) => Math.Sqrt(Gramian(vectors).GetDouble());

    public Fraction DistSqr(Matrix vector)
    {
        List<Matrix> vectors = Basis.Select(x => x).ToList();
        vectors.Add(vector);
        return Gramian(vectors) / Gramian();
    }

    public double Dist(Matrix vector) => Math.Sqrt(DistSqr(vector).GetDouble());
    
    public EuclideanSpace Orthogonalize()
    {
        var newBasis = new List<Matrix>(Basis.Count);
        foreach (var a in Basis)
        {
            var b = a.Copy();
            foreach (var prevB in newBasis)
            {
                b -= prevB * (_dotProduct(prevB, a) / _dotProduct(prevB, prevB));
            }

            //if (!b.All(0))
            //{
                newBasis.Add(b);
            //}
        }

        return new EuclideanSpace(newBasis, _dotProduct);
    }

    private static readonly Func<Matrix, Matrix, Fraction> DefaultDotProduct = (v1, v2) =>
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