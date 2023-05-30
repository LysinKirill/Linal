namespace Linal;

public class LinearSpace
{
    public List<Matrix> Basis { get; }
    public Matrix MatrixBasis { get; }
    public int Dim => Basis.Count;

    public LinearSpace(List<Matrix> vectors)
    {
        Basis = GetBasis(vectors);
        MatrixBasis = Matrix.ConcatColumns(Basis.ToArray());
    }

    public LinearSpace(params Matrix[] vectors)
    {
        Basis = GetBasis(vectors.ToList());
        MatrixBasis = Matrix.ConcatColumns(Basis.ToArray());
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

    public bool ContainsVector(Matrix vector)
        => Matrix.ConcatColumns(MatrixBasis, vector).Rank() == MatrixBasis.Rank();
}