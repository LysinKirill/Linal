namespace Linal;

public class EuclideanSpace : LinearSpace
{
    public EuclideanSpace(List<Matrix> vectors, Func<Matrix, Matrix, Fraction> dotProduct) : base(vectors)
    {
        this._dotProduct = dotProduct;
    }

    public EuclideanSpace(LinearSpace linearSpace, Func<Matrix, Matrix, Fraction> dotProduct) : base(linearSpace.Basis)
    {
        this._dotProduct = dotProduct;
    }

    private readonly Func<Matrix, Matrix, Fraction> _dotProduct;
    
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
}