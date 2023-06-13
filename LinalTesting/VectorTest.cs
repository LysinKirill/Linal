using Xunit.Abstractions;

namespace LinalTesting;

public class VectorTest
{
    private readonly ITestOutputHelper output;

    public VectorTest(ITestOutputHelper output)
    {
        this.output = output;
    }
    
    [Fact]
    [Trait("Category", "TransposeMethod")]
    public void TransposeTest1()
    {
        var vector = new Vector(new Fraction[] { 7, 0, 3 });

        var expectedRes = new Vector(new Fraction[] { 7, 0, 3 }, false);

        var res = vector.Transpose();

        if (res != expectedRes)
        {
            throw new Exception($"Input A:\n{vector}\nExpected:\n{expectedRes}\nWas:\n{res}");
        }
    }
    
    [Fact]
    [Trait("Category", "TransposeMethod")]
    public void TransposeTest2()
    {
        var vector = new Vector(new Fraction[] { 7, 0, 3 }, false);

        var expectedRes = new Vector(new Fraction[] { 7, 0, 3 });

        var res = vector.Transpose();

        if (res != expectedRes)
        {
            throw new Exception($"Input A:\n{vector}\nExpected:\n{expectedRes}\nWas:\n{res}");
        }
    }
    
    [Fact]
    [Trait("Category", "Operator(*)")]
    public void OperatorMultiplicationTest1()
    {
        var vector1 = new Vector(new Fraction[] { 7, 0, 3 });
        var vector2 = new Vector(new Fraction[] { 1, 5, 2 }, false);
        
        var expectedRes = new Matrix(new[]
        {
            new Fraction[] { 7, 35, 14 },
            new Fraction[] { 0, 0, 0 },
            new Fraction[] { 3, 15, 6 },
        });

        var res = vector1 * vector2;

        if (res != expectedRes)
        {
            throw new Exception($"Input A:\n{vector1}\nInput B:\n{vector2}\nExpected:\n{expectedRes}\nWas:\n{res}");
        }
    }
    
    [Fact]
    [Trait("Category", "Operator(*)")]
    public void OperatorMultiplicationTest2()
    {
        var vector1 = new Vector(new Fraction[] { 7, 0, 3 }, false);
        var vector2 = new Vector(new Fraction[] { 1, 5, 2 });
        
        var expectedRes = new Matrix(new[]
        {
            new Fraction[] { 13 },
        });

        var res = vector1 * vector2;

        if (res != expectedRes)
        {
            throw new Exception($"Input A:\n{vector1}\nInput B:\n{vector2}\nExpected:\n{expectedRes}\nWas:\n{res}");
        }
    }
    
    [Fact]
    [Trait("Category", "Operator(+)")]
    public void OperatorPlusTest1()
    {
        var vector1 = new Vector(new Fraction[] { 7, 0, 3 });
        var vector2 = new Vector(new Fraction[] { 1, 5, 2 });
        
        var expectedRes = new Vector(new Fraction[] { 8, 5, 5 });

        var res = vector1 + vector2;

        if (res != expectedRes)
        {
            throw new Exception($"Input A:\n{vector1}\nInput B:\n{vector2}\nExpected:\n{expectedRes}\nWas:\n{res}");
        }
    }
    
    [Fact]
    [Trait("Category", "Operator(+)")]
    public void OperatorPlusTest2()
    {
        var vector1 = new Vector(new Fraction[] { 7, 0, 3 });
        var vector2 = new Vector(new Fraction[] { 1, 5, 2 }, false);

        var res = new Matrix();
        try
        {
            res = vector1 + vector2;
        }
        catch (ArgumentException)
        {
            return;
        }
        throw new Exception($"Input A:\n{vector1}\nInput B:\n{vector2}\nExpected:\nArgumentException\nWas:\n{res}");
    }
    
    [Fact]
    [Trait("Category", "Operator(-)")]
    public void OperatorMinusTest1()
    {
        var vector1 = new Vector(new Fraction[] { 7, 0, 3 });
        var vector2 = new Vector(new Fraction[] { 1, 5, 2 });
        
        var expectedRes = new Vector(new Fraction[] { 6, -5, 1 });

        var res = vector1 - vector2;

        if (res != expectedRes)
        {
            throw new Exception($"Input A:\n{vector1}\nInput B:\n{vector2}\nExpected:\n{expectedRes}\nWas:\n{res}");
        }
    }
    
    [Fact]
    [Trait("Category", "Operator(-)")]
    public void OperatorMinusTest2()
    {
        var vector1 = new Vector(new Fraction[] { 7, 0, 3 });
        var vector2 = new Vector(new Fraction[] { 1, 5, 2 }, false);

        var res = new Matrix();
        try
        {
            res = vector1 - vector2;
        }
        catch (ArgumentException)
        {
            return;
        }
        throw new Exception($"Input A:\n{vector1}\nInput B:\n{vector2}\nExpected:\nArgumentException\nWas:\n{res}");
    }
    
    [Fact]
    [Trait("Category", "Indexers")]
    public void VectorIndexerTest1()
    {
        var vector = new Vector(new Fraction[] { 7, 0, 3 });
        var expectedRes = new Fraction[] { 7, 0, 3 };
        for (int i = 0; i < vector.Size; i++)
        {
            if (vector[i] != expectedRes[i])
            {
                throw new Exception($"Input A:\n{vector}\nExpected:\n{expectedRes[i]}\nWas:\n{vector[i]}");
            }
        }
    }
    
    [Fact]
    [Trait("Category", "Indexers")]
    public void VectorIndexerTest2()
    {
        var vector = new Vector(new Fraction[] { 7, 0, 3 }, false);
        var expectedRes = new Fraction[] { 7, 0, 3 };
        for (int i = 0; i < vector.Size; i++)
        {
            if (vector[i] != expectedRes[i])
            {
                throw new Exception($"Input A:\n{vector}\nExpected:\n{expectedRes[i]}\nWas:\n{vector[i]}");
            }
        }
    }
    
    [Fact]
    [Trait("Category", "ConcatVectorsMethod")]
    public void ConcatVectorsTest1()
    {
        var a = new Vector[]
        {
            new Vector(new Fraction[] { 1, 3, 0 }),
            new Vector(new Fraction[] { 3, 2, -1 }),
            new Vector(new Fraction[] { 0, -1, 1 }),
        };

        var expectedRes = new Matrix(new[]
        {
            new Fraction[] { 1, 3, 0 },
            new Fraction[] { 3, 2, -1 },
            new Fraction[] { 0, -1, 1 },
        });

        var res = Matrix.ConcatVectors(a);

        if (res != expectedRes)
        {
            throw new Exception($"Input A:\n{a}\nExpected:\n{expectedRes}\nWas:\n{res}");
        }
    }
    /*
    [Fact]
    [Trait("Category", "Constructor")]
    public void ConstructorFromFractionArrayTest1()
    {
        var m = new Vector(new Fraction[] { 5, 3, 6, 7 });

        if (m != expectedRes)
        {
            throw new Exception($"Input A:\n{m}\nExpected:\n{expectedRes}\nWas:\n{res}");
        }
    }
    */
}