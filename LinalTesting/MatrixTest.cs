using Xunit.Abstractions;

namespace LinalTesting;

public class MatrixTest
{
    private readonly ITestOutputHelper output;

    public MatrixTest(ITestOutputHelper output)
    {
        this.output = output;
    }
    
    [Fact]
    [Trait("Category", "Operator(+)")]
    public void OperatorPlusTest1()
    {
        var m1 = new Matrix(new[]
        {
            new Fraction[] { 7, 0, 3 },
            new Fraction[] { 13, 2, 7 },
            new Fraction[] { -14, 0, -6 },
        });
        
        var m2 = new Matrix(new[]
        {
            new Fraction[] { 5, -5, -3 },
            new Fraction[] { 3, -3, -2 },
            new Fraction[] { 2, -2, -1 },
        });
        
        var expectedRes = new Matrix(new[]
        {
            new Fraction[] { 12, -5, 0 },
            new Fraction[] { 16, -1, 5 },
            new Fraction[] { -12, -2, -7 },
        });

        var res = m1 + m2;

        if (res != expectedRes)
        {
            throw new Exception($"Input A:\n{m1}\nInput B:\n{m2}\nExpected:\n{expectedRes}\nWas:\n{res}");
        }
    }
    
    [Fact]
    [Trait("Category", "RankMethod")]
    public void RankTest1()
    {
        var m = new Matrix(new[]
        {
            new Fraction[] { 7, 0, 3 },
            new Fraction[] { 13, 2, 7 },
            new Fraction[] { -14, 0, -6 },
        });

        const int expectedRes = 2;

        var res = m.Rank();

        if (res != expectedRes)
        {
            throw new Exception($"Input A:\n{m}\nExpected:\n{expectedRes}\nWas:\n{res}");
        }
    }
    
    [Fact]
    [Trait("Category", "RankMethod")]
    public void RankTest2()
    {
        var m = new Matrix(new[]
        {
            new Fraction[] { 5, 3, 6 },
            new Fraction[] { 10, -2, 7 },
            new Fraction[] { -6, 3, 1 },
        });

        const int expectedRes = 3;

        var res = m.Rank();

        if (res != expectedRes)
        {
            throw new Exception($"Input A:\n{m}\nExpected:\n{expectedRes}\nWas:\n{res}");
        }
    }

    [Fact]
    [Trait("Category", "IsDiagonalMethod")]
    public void IsDiagonalTest1()
    {
        var m = new Matrix(new[]
        {
            new Fraction[] { 7, 0, 3 },
            new Fraction[] { 13, 2, 7 },
            new Fraction[] { -14, 0, -6 },
        });

        var res = m.IsDiagonal();

        if (res)
        {
            throw new Exception($"Input:\n{m}\nExpected:\n{false}\nWas:\n{res}");
        }
    }

    [Fact]
    [Trait("Category", "IsDiagonalMethod")]
    public void IsDiagonalTest2()
    {
        var m = new Matrix(new[]
        {
            new Fraction[] { 1, 0, 0 },
            new Fraction[] { 0, 2, 0 },
            new Fraction[] { 0, 0, 1 },
        });

        var res = m.IsDiagonal();

        if (!res)
        {
            throw new Exception($"Input:\n{m}\nExpected:\n{true}\nWas:\n{res}");
        }
    }

    [Fact]
    [Trait("Category", "CopyMethod")]
    public void CopyTest1()
    {
        var m = new Matrix(new[]
        {
            new Fraction[] { 5, -5, -3 },
            new Fraction[] { 3, -3, -2 },
            new Fraction[] { 2, -2, -1 },
        });

        var res = m.Copy();

        if (m != res)
        {
            throw new Exception($"Input:\n{m}\nExpected:\n{m}\nWas:\n{res}");
        }
    }

    [Fact]
    [Trait("Category", "CopyMethod")]
    public void CopyTest2()
    {
        var m = new Matrix(new[]
        {
            new Fraction[] { 7, 0, 3, 25 },
            new Fraction[] { 13, 2, 7, 5 },
            new Fraction[] { -14, 0, -6, 9 },
        });

        var res = m.Copy();

        if (m != res)
        {
            throw new Exception($"Input:\n{m}\nExpected:\n{m}\nWas:\n{res}");
        }
    }

    [Fact]
    [Trait("Category", "IsSymmetricalMethod")]
    public void IsSymmetricalTest1()
    {
        var m = new Matrix(new[]
        {
            new Fraction[] { 7, 0, 3 },
            new Fraction[] { 13, 2, 7 },
            new Fraction[] { -14, 0, -6 },
        });

        var res = m.IsSymmetrical();

        if (res)
        {
            throw new Exception($"Input:\n{m}\nExpected:\n{false}\nWas:\n{res}");
        }
    }

    [Fact]
    [Trait("Category", "IsSymmetricalMethod")]
    public void IsSymmetricalTest2()
    {
        var m = new Matrix(new[]
        {
            new Fraction[] { 1, 3, 0 },
            new Fraction[] { 3, 2, -1 },
            new Fraction[] { 0, -1, 1 },
        });

        var res = m.IsSymmetrical();

        if (!res)
        {
            throw new Exception($"Input:\n{m}\nExpected:\n{true}\nWas:\n{res}");
        }
    }

    [Fact]
    [Trait("Category", "ConcatColumnsMethod")]
    public void ConcatColumnsTest1()
    {
        var a = new Matrix(new[]
        {
            new Fraction[] { 1, 3, 0 },
            new Fraction[] { 3, 2, -1 },
            new Fraction[] { 0, -1, 1 },
        });

        var b = new Matrix(new[]
        {
            new Fraction[] { 5, 3, 6 },
            new Fraction[] { 10, -2, 7 },
            new Fraction[] { -6, 3, 1 },
        });

        var expectedRes = new Matrix(new[]
        {
            new Fraction[] { 1, 3, 0, 5, 3, 6 },
            new Fraction[] { 3, 2, -1, 10, -2, 7 },
            new Fraction[] { 0, -1, 1, -6, 3, 1 },
        });

        var res = Matrix.ConcatColumns(a, b);

        if (res != expectedRes)
        {
            throw new Exception($"Input A:\n{a}\nInput B:\n{b}\nExpected:\n{expectedRes}\nWas:\n{res}");
        }
    }

    [Fact]
    [Trait("Category", "ConcatColumnsMethod")]
    public void ConcatColumnsTest2()
    {
        var a = new Matrix(new[]
        {
            new Fraction[] { 1, 3, },
            new Fraction[] { 3, 2, },
            new Fraction[] { 0, -1, },
        });

        var b = new Matrix(new[]
        {
            new Fraction[] { 5, 3, 6 },
            new Fraction[] { 10, -2, 7 },
            new Fraction[] { -6, 3, 1 },
        });

        var expectedRes = new Matrix(new[]
        {
            new Fraction[] { 1, 3, 5, 3, 6 },
            new Fraction[] { 3, 2, 10, -2, 7 },
            new Fraction[] { 0, -1, -6, 3, 1 },
        });

        var res = Matrix.ConcatColumns(a, b);

        if (res != expectedRes)
        {
            throw new Exception($"Input A:\n{a}\nInput B:\n{b}\nExpected:\n{expectedRes}\nWas:\n{res}");
        }
    }

    [Fact]
    [Trait("Category", "ConcatColumnsMethod")]
    public void ConcatColumnsTest3()
    {
        var a = new Matrix(new[]
        {
            new Fraction[] { 1, 3, 0 },
            new Fraction[] { 3, 2, -1 },
            new Fraction[] { 0, -1, 1 },
        });

        var b = new Matrix(new[]
        {
            new Fraction[] { 5, 3, 6 },
            new Fraction[] { 10, -2, 7 },
            new Fraction[] { -6, 3, 1 },
        });

        var expectedRes = new Matrix(new[]
        {
            new Fraction[] { 1, 3, 0, 5, 3, 6 },
            new Fraction[] { 3, 2, -1, 10, -2, 7 },
            new Fraction[] { 0, -1, 1, -6, 3, 1 },
        });

        var res = Matrix.ConcatColumns(new[] { a, b });

        if (res != expectedRes)
        {
            throw new Exception($"Input A:\n{a}\nInput B:\n{b}\nExpected:\n{expectedRes}\nWas:\n{res}");
        }
    }

    [Fact]
    [Trait("Category", "ConcatColumnsMethod")]
    public void ConcatColumnsTest4()
    {
        var a = new Matrix(new[]
        {
            new Fraction[] { 1, 3, },
            new Fraction[] { 3, 2, },
            new Fraction[] { 0, -1, },
        });

        var b = new Matrix(new[]
        {
            new Fraction[] { 5, 3, 6 },
            new Fraction[] { 10, -2, 7 },
            new Fraction[] { -6, 3, 1 },
        });

        var expectedRes = new Matrix(new[]
        {
            new Fraction[] { 1, 3, 5, 3, 6 },
            new Fraction[] { 3, 2, 10, -2, 7 },
            new Fraction[] { 0, -1, -6, 3, 1 },
        });

        var res = Matrix.ConcatColumns(new[] { a, b });

        if (res != expectedRes)
        {
            throw new Exception($"Input A:\n{a}\nInput B:\n{b}\nExpected:\n{expectedRes}\nWas:\n{res}");
        }
    }
    
    [Fact]
    [Trait("Category", "ConcatColumnsMethod")]
    public void ConcatColumnsTest5()
    {
        var a = new Matrix(new[]
        {
            new Fraction[] { 1, 3, },
            new Fraction[] { 3, 2, },
            new Fraction[] { 0, -1, },
        });

        var b = new Matrix(new[]
        {
            new Fraction[] { 5, 3, 6 },
            new Fraction[] { 10, -2, 7 },
            new Fraction[] { -6, 3, 1 },
        });
        
        var c = new Matrix(new[]
        {
            new Fraction[] { 5, },
            new Fraction[] { 7, },
            new Fraction[] { 9, },
        });

        var expectedRes = new Matrix(new[]
        {
            new Fraction[] { 1, 3, 5, 3, 6, 5 },
            new Fraction[] { 3, 2, 10, -2, 7, 7 },
            new Fraction[] { 0, -1, -6, 3, 1, 9 },
        });

        var res = Matrix.ConcatColumns(a, b, c);

        if (res != expectedRes)
        {
            throw new Exception($"Input A:\n{a}\nInput B:\n{b}\nInput C:\n{c}\nExpected:\n{expectedRes}\nWas:\n{res}");
        }
    }

    [Fact]
    [Trait("Category", "GetVectorsMethod")]
    public void GetVectorsTest1()
    {
        var m = new Matrix(new[]
        {
            new Fraction[] { 1, 3, 5, 3, 6 },
            new Fraction[] { 3, 2, 10, -2, 7 },
            new Fraction[] { 0, -1, -6, 3, 1 },
        });

        var expectedRes = new Vector[]
        {
            new(new Fraction[] { 1, 3, 0 }),
            new(new Fraction[] { 3, 2, -1 }),
            new(new Fraction[] { 5, 10, -6 }),
            new(new Fraction[] { 3, -2, 3 }),
            new(new Fraction[] { 6, 7, 1 }),
        };

        var res = m.GetVerticalVectors();

        if (res.Count != expectedRes.Length)
            throw new Exception($"expectedRes.Count != res.Count");

        for (int i = 0; i < res.Count; i++)
        {
            if (res[i] != expectedRes[i]) 
            {
                throw new Exception($"Input A:\n{m}\nExpected:\n{expectedRes[i]}\nWas:\n{res[i]}");
            }
        }
    }
    
    [Fact]
    [Trait("Category", "TransposeMethod")]
    public void TransposeTest1()
    {
        var m = new Matrix(new[]
        {
            new Fraction[] { 7, 0, 3 },
            new Fraction[] { 13, 2, 7 },
            new Fraction[] { -14, 0, -6 },
        });
        var expectedRes = new Matrix(new[]
        {
            new Fraction[] { 7, 13, -14 },
            new Fraction[] { 0, 2, 0 },
            new Fraction[] { 3, 7, -6 },
        });
        var res = m.Transpose();
        if (res != expectedRes)
        {
            throw new Exception($"Input:\n{m}\nExpected:\n{expectedRes}\nWas:\n{res}");
        }

        output.WriteLine($"Input:\n{m}\nOutput:\n{res}");
    }
    
    [Fact]
    [Trait("Category", "TransposeMethod")]
    public void TransposeTest2()
    {
        var m = new Matrix(new[]
        {
            new Fraction[] { 7, 0, 3, 25 },
            new Fraction[] { 13, 2, 7, 5 },
            new Fraction[] { -14, 0, -6, 9 },
        });
        var expectedRes = new Matrix(new[]
        {
            new Fraction[] { 7, 13, -14 },
            new Fraction[] { 0, 2, 0 },
            new Fraction[] { 3, 7, -6 },
            new Fraction[] { 25, 5, 9 },
        });
        var res = m.Transpose();
        if (res != expectedRes)
        {
            throw new Exception($"Input:\n{m}\nExpected:\n{expectedRes}\nWas:\n{res}");
        }

        output.WriteLine($"Input:\n{m}\nOutput:\n{res}");
    }
    
    [Fact]
    [Trait("Category", "ReduceMethod")]
    public void ReduceTest1()
    {
        var m = new Matrix(new[]
        {
            new Fraction[] { 5, -5, -3 },
            new Fraction[] { 3, -3, -2 },
            new Fraction[] { 2, -2, -1 },
        });
        var res = m.Reduce();

        if (res[1, 0] != 0 || res[1, 1] != 0 || res[2, 0] != 0 || res[2, 1] != 0 || res[2, 2] != 0)
        {
            throw new Exception($"Input:\n{m}\nWas:\n{res}");
        }

        output.WriteLine($"Input:\n{m}\nOutput:\n{res}");
    }
    
    [Fact]
    [Trait("Category", "ReduceMethod")]
    public void ReduceTest2()
    {
        var m = new Matrix(new[]
        {
            new Fraction[] { 25, -20, -3, 5 },
            new Fraction[] { 15, -3, -7, 18 },
            new Fraction[] { 4, -7, -1, 25 },
        });
        var res = m.Reduce();

        if (res[1, 0] != 0 || res[2, 0] != 0 || res[2, 1] != 0)
        {
            throw new Exception($"Input:\n{m}\nWas:\n{res}");
        }

        output.WriteLine($"Input:\n{m}\nOutput:\n{res}");
    }
    
    [Fact]
    [Trait("Category", "ReduceMethod")]
    public void ReduceTest3()
    {
        var m = new Matrix(new[]
        {
            new Fraction[] { 25, -20, },
            new Fraction[] { 18, -2, },
            new Fraction[] { 3, -7, },
            new Fraction[] { 8, -9, },
        });
        var res = m.Reduce();

        if (res[1, 0] != 0 || res[2, 0] != 0 || res[2, 1] != 0 || res[3, 0] != 0 || res[3, 1] != 0)
        {
            throw new Exception($"Input:\n{m}\nWas:\n{res}");
        }

        output.WriteLine($"Input:\n{m}\nOutput:\n{res}");
    }

    [Fact]
    [Trait("Category", "CanonicalMethod")]
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
        var res = m.Canonical();
        if (res != expectedRes)
        {
            throw new Exception($"Input:\n{m}\nExpected:\n{expectedRes}\nWas:\n{res}");
        }

        output.WriteLine($"Input:\n{m}\nOutput:\n{res}");
    }

    [Fact]
    [Trait("Category", "CanonicalMethod")]
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
        var res = m.Canonical();
        if (res != expectedRes)
        {
            throw new Exception($"Input:\n{m}\nExpected:\n{expectedRes}\nWas:\n{res}");
        }

        output.WriteLine($"Input:\n{m}\nOutput:\n{res}");
    }

    [Fact]
    [Trait("Category", "CanonicalMethod")]
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
        var res = m.Canonical();
        if (res != expectedRes)
        {
            throw new Exception($"Input:\n{m}\nExpected:\n{expectedRes}\nWas:\n{res}");
        }

        output.WriteLine($"Input:\n{m}\nOutput:\n{res}");
    }

    [Fact]
    [Trait("Category", "CanonicalMethod")]
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
        var res = m.Canonical();
        if (res != expectedRes)
        {
            throw new Exception($"Input:\n{m}\nExpected:\n{expectedRes}\nWas:\n{res}");
        }

        output.WriteLine($"Input:\n{m}\nOutput:\n{res}");
    }
    
    
    
    [Fact]
    [Trait("Category", "CanonicalMethod")]
    public void CanonicalTest5()
    {
        var m = new Matrix(new[]
        {
            new Fraction[] { 5, -3, 10, },
            new Fraction[] { 20, 2, 5, },
            new Fraction[] { -15, 1, -6, },
            new Fraction[] { -31, 7, -15, },
            new Fraction[] { -22, 5, -4, },
        });
        var expectedRes = new Matrix(new[]
        {
            new Fraction[] { 1, 0, 0, },
            new Fraction[] { 0, 1, 0, },
            new Fraction[] { 0, 0, 1, },
            new Fraction[] { 0, 0, 0, },
            new Fraction[] { 0, 0, 0 },
        });
        var res = m.Canonical();
        if (res != expectedRes)
        {
            throw new Exception($"Input:\n{m}\nExpected:\n{expectedRes}\nWas:\n{res}");
        }

        output.WriteLine($"Input:\n{m}\nOutput:\n{res}");
    }
    
    
    [Fact]
    [Trait("Category", "FsrMethod")]
    public void FsrTest1()
    {
        var m = new Matrix(new[]
        {
            new Fraction[] { 2, 1, 1, 3 },
            new Fraction[] { 3, 2, 2, 1 },
            new Fraction[] { 1, 2, 2, -9 },
        });
        
        var expectedRes = new Vector[]
        {
            new Vector(new Fraction[] { 0, -1, 1, 0}),
            new Vector(new Fraction[] { -5, 7, 0, 1}),
        };

        var res = m.FSR();
        for (int i = 0; i < res.Count; i++)
        {
            if (res.Count != expectedRes.Length || res[i] != expectedRes[i])
            {
                throw new Exception($"Input:\n{m}\nExpected:\n{expectedRes[i]}\nWas:\n{res[i]}");
            }
        }
        
        output.WriteLine($"Input:\n{m}\nOutput:\n{res}");
    }
    
    [Fact]
    [Trait("Category", "FsrMethod")]
    public void FsrTest2()
    {
        var m = new Matrix(new[]
        {
            new Fraction[] { 2, 1, 3, -1 },
            new Fraction[] { 3, 2, 0, -2 },
            new Fraction[] { 3, 1, 9, -1 },
        });
        
        var expectedRes = new Vector[]
        {
            new Vector(new Fraction[] { -6, 9, 1, 0}),
            new Vector(new Fraction[] { 0, 1, 0, 1}),
        };

        var res = m.FSR();
        for (int i = 0; i < res.Count; i++)
        {
            if (res.Count != expectedRes.Length || res[i] != expectedRes[i])
            {
                throw new Exception($"Input:\n{m}\nExpected:\n{expectedRes[i]}\nWas:\n{res[i]}");
            }
        }
        
        output.WriteLine($"Input:\n{m}\nOutput:\n{res}");
    }
}
