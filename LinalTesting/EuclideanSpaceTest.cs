using Xunit.Abstractions;

namespace LinalTesting;

public class EuclideanSpaceTest
{
    private readonly ITestOutputHelper output;

    public EuclideanSpaceTest(ITestOutputHelper output)
    {
        this.output = output;
    }

    [Fact]
    [Trait("Category", "OrthogonalizeMethod")]
    public void OrthogonalizeTest1()
    {
        var eSpace = new EuclideanSpace(
            new Matrix(new[]
            {
                new Fraction[] { 1, 5, 3 },
                new Fraction[] { 1, 8, 9 },
                new Fraction[] { -1, -2, 3 },
                new Fraction[] { -2, -3, 8 },
            })
        );

        var expectedRes = new Matrix(new[]
        {
            new Fraction[] { 1, 2 },
            new Fraction[] { 1, 5 },
            new Fraction[] { -1, 1 },
            new Fraction[] { -2, 3 },
        });

        var res = eSpace.Orthogonalize().MatrixBasis;

        if (res != expectedRes)
        {
            throw new Exception($"Input A:\n{eSpace.MatrixBasis}\nExpected:\n{expectedRes}\nWas:\n{res}");
        }

        output.WriteLine($"Input:\n{eSpace.MatrixBasis}\nOutput:\n{res}");
    }
    
    [Fact]
    [Trait("Category", "OrthogonalizeMethod")]
    public void OrthogonalizeTest2()
    {
        var eSpace = new EuclideanSpace(
            new Matrix(new[]
            {
                new Fraction[] { 1, 2, 0 },
                new Fraction[] { 0, 1, 1 },
                new Fraction[] { 2, 2, -2 },
                new Fraction[] { 1, 3, 1 },
            })
        );

        var expectedRes = new Matrix(new[]
        {
            new Fraction[] { 1, new(1, 2), },
            new Fraction[] { 0, 1 },
            new Fraction[] { 2, -1 },
            new Fraction[] { 1, new(3, 2) },
        });

        var res = eSpace.Orthogonalize().MatrixBasis;

        if (res != expectedRes)
        {
            throw new Exception($"Input A:\n{eSpace.MatrixBasis}\nExpected:\n{expectedRes}\nWas:\n{res}");
        }

        output.WriteLine($"Input:\n{eSpace.MatrixBasis}\nOutput:\n{res}");
    }
    
    [Fact]
    [Trait("Category", "OrthogonalizeMethod")]
    public void OrthogonalizeTest3()
    {
        var eSpace = new EuclideanSpace(
            new Matrix(new[]
            {
                new Fraction[] { 2, 7, 1, 5 },
                new Fraction[] { 1, 4, 1, 7 },
                new Fraction[] { 3, 3, -6, 7 },
                new Fraction[] { -1, -3, 0, 8 },
            })
        );

        var expectedRes = new Matrix(new[]
        {
            new Fraction[] { 2, 3, 1 },
            new Fraction[] { 1, 2, 5 },
            new Fraction[] { 3, -3, 1 },
            new Fraction[] { -1, -1, 10 },
        });

        var res = eSpace.Orthogonalize().MatrixBasis;

        if (res != expectedRes)
        {
            throw new Exception($"Input A:\n{eSpace.MatrixBasis}\nExpected:\n{expectedRes}\nWas:\n{res}");
        }

        output.WriteLine($"Input:\n{eSpace.MatrixBasis}\nOutput:\n{res}");
    }
}