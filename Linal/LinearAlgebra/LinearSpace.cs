namespace Linal;

public class LinearSpace
{
    public List<Vector> Basis { get; }
    public Matrix MatrixBasis { get; }
    public int Dim => Basis.Count;

    public LinearSpace(List<Vector> vectors)
    {
        Basis = GetBasis(vectors);
        MatrixBasis = Matrix.ConcatVectors(Basis.ToArray());
    }

    public LinearSpace(int n)
    {
        Basis = new();
        for (int i = 0; i < n; ++i)
        {
            Basis.Add(new Vector(n, 0));
            Basis[^1][i] = 1;
        }
        MatrixBasis = Matrix.ConcatVectors(Basis.ToArray());
    }
    
    public LinearSpace(Matrix basisMatrix)
    {
        Basis = GetBasis(basisMatrix.GetVerticalVectors());
        MatrixBasis = Matrix.ConcatVectors(Basis.ToArray());
    }

    public LinearSpace(params Vector[] vectors)
    {
        Basis = GetBasis(vectors.ToList());
        MatrixBasis = Matrix.ConcatVectors(Basis.ToArray());
    }

    private List<Matrix> GetBasis(List<Matrix> vectors)
    {
        var canonical = Matrix.ConcatColumns(vectors.ToArray()).Canonical();
        var res = new List<Matrix>(vectors.Count);
        for (int i = 0; i < canonical.Rows; i++)
        {
            for (int j = 0; j < canonical.Columns; j++)
            {
                if (canonical[i, j] == 0) continue;
                res.Add(vectors[j]);
                break;
            }
        }

        return res;
    }
    
    private List<Vector> GetBasis(List<Vector> vectors)
    {
        var canonical = Matrix.ConcatVectors(vectors.ToArray()).Canonical();
        var res = new List<Vector>(vectors.Count);
        for (int i = 0; i < canonical.Rows; i++)
        {
            for (int j = 0; j < canonical.Columns; j++)
            {
                if (canonical[i, j] == 0) continue;
                res.Add(vectors[j]);
                break;
            }
        }

        return res;
    }

    public bool ContainsVector(Vector vector)
        => Matrix.ConcatColumns(MatrixBasis, vector).Rank() == MatrixBasis.Rank();
}