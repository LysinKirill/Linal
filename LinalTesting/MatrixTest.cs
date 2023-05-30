namespace LinalTesting;

public class MatrixTest
{
    [Fact]
    public void CanonicalTest1()
    {
        var m = new Matrix(new[]
        {
            new Fraction[] { 5, -5, -3 },
            new Fraction[] { 3, -3, -2 },
            new Fraction[] { 2, -2, -1 },
        });
        var expectedRes = new Matrix(new[]
        {
            new Fraction[] { 1, -1, 0 },
            new Fraction[] { 0, 0, 1 },
            new Fraction[] { 0, 0, 0 },
        });
        var res = Matrix.Canonical(m);
        if (res != expectedRes)
        {
            throw new Exception();
        }
    }

    [Fact]
    public void CanonicalTest2()
    {
        var m = new Matrix(new[]
        {
            new Fraction[] { 7, 0, 3 },
            new Fraction[] { 13, 2, 7 },
            new Fraction[] { -14, 0, -6 },
        });
        var expectedRes = new Matrix(new[]
        {
            new Fraction[] { 1, 0, new(3, 7) },
            new Fraction[] { 0, 1, new(5, 7) },
            new Fraction[] { 0, 0, 0 },
        });
        var res = Matrix.Canonical(m);
        if (res != expectedRes)
        {
            throw new Exception();
        }
    }

    [Fact]
    public void CanonicalTest3()
    {
        var m = new Matrix(new[]
        {
            new Fraction[] { 7, 0, 3, 25 },
            new Fraction[] { 13, 2, 7, 5 },
            new Fraction[] { -14, 0, -6, 9 },
        });
        var expectedRes = new Matrix(new[]
        {
            new Fraction[] { 1, 0, new(3, 7), 0 },
            new Fraction[] { 0, 1, new(5, 7), 0 },
            new Fraction[] { 0, 0, 0, 1 },
        });
        var res = Matrix.Canonical(m);
        if (res != expectedRes)
        {
            throw new Exception();
        }
    }
    
    [Fact]
    public void CanonicalTest4()
    {
        var m = new Matrix(new[]
        {
            new Fraction[] { 5, -3, 10, 25, 2 },
            new Fraction[] { 20, 2, 5, 5, 3 },
            new Fraction[] { -15, 1, -6, 9, 4 },
        });
        var expectedRes = new Matrix(new[]
        {
            new Fraction[] { 1, 0, 0, new(-39, 14), new(-99, 140) },
            new Fraction[] { 0, 1, 0, new(165, 14), new(115, 28) },
            new Fraction[] { 0, 0, 1, new(52, 7), new(25, 14) },
        });
        var res = Matrix.Canonical(m);
        if (res != expectedRes)
        {
            throw new Exception();
        }
    }
}