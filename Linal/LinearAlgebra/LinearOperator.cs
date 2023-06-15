namespace Linal;

public class LinearOperator
{
    private LinearSpace _firstLinearSpace;
    private LinearSpace _secondLinearSpace;

    private Func<Vector, Vector> _func;
    public Matrix A { get; init; }

    public LinearOperator(LinearSpace V, LinearSpace W, Func<Vector, Vector> func)
    {
        _firstLinearSpace = V;
        _secondLinearSpace = W;
        _func = func;

        if (!CheckLinearOperator())
            throw new ArgumentException("Invalid operator");

        A = GetOperatorMatrix();
    }
    
    public LinearOperator(LinearSpace V, LinearSpace W, Matrix A)
    {
        _firstLinearSpace = V;
        _secondLinearSpace = W;
        this.A = A;
        _func = x => new Vector(this.A * x);

        //if (!CheckLinearOperator())
        //    throw new ArgumentException("Invalid operator");
        
    }

    public LinearSpace Ker()
    {
        return new LinearSpace(A.FSR());
    }

    public LinearSpace Im() => new(A);


    private bool CheckLinearOperator()
    {
        Fraction[] magicConsts = { new(13, 7), new(5, 3), new(-19, 6), new(1, 2), new(-11, 5) };
        foreach (var e1 in _firstLinearSpace.Basis)
        {
            if (!_secondLinearSpace.ContainsVector(_func(e1)))
                return false;

            foreach (var e2 in _firstLinearSpace.Basis)
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
        Matrix temp = Matrix.ConcatColumns(_firstLinearSpace.MatrixBasis.Transpose(),
            _secondLinearSpace.MatrixBasis.Transpose());
        temp = temp.Canonical();
        return temp.TakeColumns(x => x > _firstLinearSpace.Basis[0].Rows).Transpose();
    }
}