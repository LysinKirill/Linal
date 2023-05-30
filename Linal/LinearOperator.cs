namespace Linal;

public class LinearOperator
{
    private LinearSpace _firstLinearSpace;
    private LinearSpace _secondLinearSpace;

    private Func<Matrix, Matrix> _func;
    public Matrix A { get; init; }

    public LinearOperator(LinearSpace V, LinearSpace W, Func<Matrix, Matrix> func)
    {
        _firstLinearSpace = V;
        _secondLinearSpace = W;
        _func = func;

        if (!CheckLinearOperator())
            throw new ArgumentException("Invalid operator");

        A = GetOperatorMatrix();
    }


    private bool CheckLinearOperator()
    {
        Fraction[] magicConsts = { new(13, 7), new(5, 3), new(-19, 6), new(1, 2), new(-11, 5) };
        foreach (Matrix e1 in _firstLinearSpace.Basis)
        {
            if (!_secondLinearSpace.ContainsVector(_func(e1)))
                return false;

            foreach (Matrix e2 in _firstLinearSpace.Basis)
            {
                if (_func(e1 + e2) != _func(e1) + _func(e2))
                    return false;
            }

            foreach (var magicConst in magicConsts)
                if (_func(magicConst * e1) != _func(e1) * magicConst)
                    return false;
        }

        return true;
    }

    private Matrix GetOperatorMatrix()
    {
        Matrix temp = Matrix.ConcatColumns(Matrix.Transpose(_firstLinearSpace.MatrixBasis),
            Matrix.Transpose(_secondLinearSpace.MatrixBasis));
        temp = Matrix.Canonical(temp);
        return Matrix.Transpose(temp.TakeColumns(x => x > _firstLinearSpace.Basis[0].Rows));
    }
}