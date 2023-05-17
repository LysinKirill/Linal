namespace Linal;

public class LinearSpace
{
    private readonly List<Matrix> _basis;
    public int Dim => _basis.Count;

    public LinearSpace(List<Matrix> vectors)
    {
        _basis = GetBasis(vectors);
    }

    public LinearSpace(params Matrix[] vectors)
    {
        _basis = GetBasis(vectors.ToList());
    }

    private List<Matrix> GetBasis(List<Matrix> vectors)
    {
        var canonical = Matrix.Canonical(Matrix.ConcatColumns(vectors.ToArray()));
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
}