namespace Linal;

public class EuclideanSpace : LinearSpace
{
    private readonly Func<Matrix, Matrix, Fraction> _dotProduct;
    
    public EuclideanSpace(List<Matrix> vectors, Func<Matrix, Matrix, Fraction>? dotProduct = null) : base(vectors)
    {
        _dotProduct = dotProduct ?? defaultDotProduct;
    }

    public EuclideanSpace(LinearSpace linearSpace, Func<Matrix, Matrix, Fraction>? dotProduct = null) : base(linearSpace.Basis)
    {
        _dotProduct = dotProduct ?? defaultDotProduct;
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

    public Fraction Gramian() => Matrix.Det(Gram());
    public Fraction Gramian(List<Matrix> vectors) => Matrix.Det(Gram(vectors));

    public double GetVolume(List<Matrix> vectors) => Math.Sqrt(Gramian(vectors).GetDouble());
    
    public EuclideanSpace Orthogonalize()
    {
        var newBasis = new List<Matrix>(Basis.Count);
        for (int i = 0; i < Basis.Count; i++)
        {
            var b = Matrix.Copy(Basis[i]);
            for (int j = 0; j < newBasis.Count; j++)
            {
                b -= newBasis[j] * (_dotProduct(newBasis[j], Basis[i]) / _dotProduct(newBasis[j], newBasis[j]));
            }

            var flag0 = false;
            for (int j = 0; j < b.Rows; j++)
            {
                if (b[j, 0] != 0)
                {
                    break;
                }

                if (j == b.Rows - 1)
                {
                    flag0 = true;
                }
            }

            if (!flag0)
            {
                newBasis.Add(b);
            }
        }

        return new EuclideanSpace(newBasis, _dotProduct);
    }

    private static readonly Func<Matrix, Matrix, Fraction> defaultDotProduct = (v1, v2) =>
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