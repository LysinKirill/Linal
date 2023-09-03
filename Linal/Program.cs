using System.Diagnostics;
using System.Threading.Channels;

namespace Linal;

class Program
{
    public static void Main(string[] args)
    {
        // List<Vector> vectors = new List<Vector>();
        // for (int i = 0; i < 4; ++i)
        // {
        //     vectors.Add(Vector.ReadVector());
        // }
        //
        // for (int i = 0; i < 4; ++i)
        // {
        //     vectors.Add(Vector.ReadVector());
        // }


        // LinearSpace l1 = new LinearSpace(V1);
        // LinearSpace l2 = new LinearSpace(V2);
        // LinearOperator linearOperator = new LinearOperator(l1, l2, matrix => matrix);
        // Console.WriteLine(linearOperator.A);


        Dictionary<string, Data> data = new Dictionary<string, Data>();

        data["Kirill"] = new Data
        {
            task1 = (new List<Vector>
                {
                    new(new Fraction[] { -1, 3, 5, 2 }),
                    new(new Fraction[] { 0, 1, -4, -5 }),
                    new(new Fraction[] { 2, -4, 0, 5 }),
                    new(new Fraction[] { 0, -2, 4, -5 })
                },
                new List<Vector>
                {
                    new(new Fraction[] { -1, 1, 1, -1 }),
                    new(new Fraction[] { 2, -1, -1, 2 }),
                    new(new Fraction[] { -3, 2, 2, -3 }),
                    new(new Fraction[] { -2, 5, 5, -2 })
                }),
            task4 = (9570, 39, 11, 30, 26, 24, 18, 2599, 1782),
            task5 = new Matrix(new[]
            {
                new Fraction[] { -6, -4, -2, 4 },
                new Fraction[] { -6, 14, -12, 24 },
                new Fraction[] { -6, 14, -2, 6 },
                new Fraction[] { -6, -4, -12, 6 },
            }),
            task6 = (new Vector(true, -4, -4, -3, 1), new Matrix(new[]
            {
                new Fraction[] { -2, -2, 0, 4 },
                new Fraction[] { 4, -2, 4, -2 },
                new Fraction[] { 4, 4, -2, -2 },
            })),
            task7 = (new List<Matrix>()
            {
                new Matrix(new[]
                {
                    new Fraction[] { -2, 2, },
                    new Fraction[] { 1, 1 },
                }),
                new Matrix(new[]
                {
                    new Fraction[] { -1, 0, },
                    new Fraction[] { 1, 0 },
                }),
            }, new List<Matrix>()
            {
                new Matrix(new[]
                {
                    new Fraction[] { -1, 1 },
                    new Fraction[] { 1, 2 },
                }),
            }, new List<Matrix>()
            {
                new Matrix(new[]
                {
                    new Fraction[] { -1, -1, },
                    new Fraction[] { 0, 0 },
                }),
                new Matrix(new[]
                {
                    new Fraction[] { -2, -1 },
                    new Fraction[] { 2, -1 },
                }),
            }),

            task9 = new Matrix(new[]
                {
                    new Fraction[] { 130, -352, -20 },
                    new Fraction[] { -352, 94, 92 },
                    new Fraction[] { -20, 92, 268 },
                }
            ),
            // task10 = new Matrix(new[]
            // {
            //     new Fraction[]{-1, 9, 1, -9},
            //     new Fraction[]{9, -1, -9, 1},
            // })
            
            
            task10 = new Matrix(new[]
            {
                new Fraction[]{-1, 9, 1, -9},
                new Fraction[]{9, -1, -9, 1},
            })
        };

        data["Angelika"] = new Data
        {
            task1 = (new List<Vector>()
                {
                    new(new Fraction[] { 1, 1, 1, 1 }),
                    new(new Fraction[] { 3, -4, -4, -4 }),
                    new(new Fraction[] { -1, 2, -4, -1 }),
                    new(new Fraction[] { -3, -5, -1, -2 }),
                },
                new List<Vector>()
                {
                    new(new Fraction[] { 4, 5, 5, -4 }),
                    new(new Fraction[] { -5, 0, 0, 5 }),
                    new(new Fraction[] { 5, 5, 5, -5 }),
                    new(new Fraction[] { 2, -2, -2, -2 }),
                }),
            task7 = (new List<Matrix>()
            {
                new Matrix(new[]
                {
                    new Fraction[] { 1, 2 },
                    new Fraction[] { 1, -2 },
                }),
                new Matrix(new[]
                {
                    new Fraction[] { 1, 1, },
                    new Fraction[] { 1, -1 },
                }),
            }, new List<Matrix>()
            {
                new Matrix(new[]
                {
                    new Fraction[] { -2, -1, },
                    new Fraction[] { 0, 2 },
                }),
            }, new List<Matrix>()
            {
                new Matrix(new[]
                {
                    new Fraction[] { 1, 1 },
                    new Fraction[] { 2, 1 },
                }),
                new Matrix(new[]
                {
                    new Fraction[] { 0, 0, },
                    new Fraction[] { 2, 1 },
                }),
            }),
        };

        data["pv"] = new Data
        {
            task1 = (
                new List<Vector>()
                {
                    new(new Fraction[] { -1, -3, 5, -1 }),
                    new(new Fraction[] { -2, 2, -2, -3 }),
                    new(new Fraction[] { -5, -5, -3, 4 }),
                    new(new Fraction[] { -1, 2, -1, 5 }),
                },
                new List<Vector>()
                {
                    new(new Fraction[] { -5, 3, 10, -5 }),
                    new(new Fraction[] { 1, 4, -2, 1 }),
                    new(new Fraction[] { -3, 1, 6, -3 }),
                    new(new Fraction[] { 1, -2, -2, 1 }),
                }),
            task6 = (new Vector(true, -4, -4, 0, 5), new Matrix(new[]
            {
                new Fraction[] { 0, 2, 0, -2 },
                new Fraction[] { 4, 2, 1, 3 },
                new Fraction[] { 2, -1, -1, 2 },
            }))
        };


        data["Maksim"] = new Data
        {
            task1 = (
                new List<Vector>()
                {
                    new(new Fraction[] { -1, 1, 2, -5 }),
                    new(new Fraction[] { 1, 1, 0, 1 }),
                    new(new Fraction[] { 1, 2, -1, 4 }),
                    new(new Fraction[] { 2, 0, -5, -5 }),
                },
                new List<Vector>()
                {
                    new(new Fraction[] { -5, 3, 1, -4 }),
                    new(new Fraction[] { -1, 3, 4, -5 }),
                    new(new Fraction[] { 1, -5, 0, 5 }),
                    new(new Fraction[] { 5, 5, -5, -3 }),
                }),

            task5 = new Matrix(new[]
            {
                new Fraction[] { 1, 8, 6, 0 },
                new Fraction[] { 1, 8, -4, 14 },
                new Fraction[] { 1, 10, 8, -18 },
                new Fraction[] { 1, 10, 18, 0 },
            }),

            task6 = (new Vector(true, -1, -3, 1, 0), new Matrix(new[]
            {
                new Fraction[] { -2, 0, -2, 1 },
                new Fraction[] { -2, 2, 1, 2 },
                new Fraction[] { 3, 1, 3, -2 },
            })),

            task7 = (new List<Matrix>()
            {
                new Matrix(new[]
                {
                    new Fraction[] { 2, -1, },
                    new Fraction[] { 1, 0 },
                }),
                new Matrix(new[]
                {
                    new Fraction[] { 1, -1, },
                    new Fraction[] { -2, -2 },
                }),
            }, new List<Matrix>()
            {
                new Matrix(new[]
                {
                    new Fraction[] { 2, 0, },
                    new Fraction[] { -1, 2 },
                }),
            }, new List<Matrix>()
            {
                new Matrix(new[]
                {
                    new Fraction[] { -2, 0, },
                    new Fraction[] { 2, 0 },
                }),
                new Matrix(new[]
                {
                    new Fraction[] { 0, 1, },
                    new Fraction[] { 2, 2 },
                }),
            }),

            task10 = new Matrix(new[]
            {
                new Fraction[] { 11, -11, 21, -21 },
                new Fraction[] { 21, -21, 11, -11 },
            })
        };


        data["Kate"] = new Data
        {
            task1 = (
                new List<Vector>()
                {
                    new(new Fraction[] { -3, 5, 4, 3 }),
                    new(new Fraction[] { 5, 3, 5, -5 }),
                    new(new Fraction[] { 0, -4, 0, -2 }),
                    new(new Fraction[] { -3, 1, 2, 3 }),
                },
                new List<Vector>()
                {
                    new(new Fraction[] { 0, -2, -5, 4 }),
                    new(new Fraction[] { -2, -1, -2, 2 }),
                    new(new Fraction[] { -1, 5, -3, -10 }),
                    new(new Fraction[] { 0, 0, -4, 0 })
                })
        };

        data["Denis"] = new Data
        {
            task7 = (new List<Matrix>()
            {
                new Matrix(new[]
                {
                    new Fraction[] { 0, 2 },
                    new Fraction[] { 2, 1 },
                }),
                new Matrix(new[]
                {
                    new Fraction[] { -2, -1 },
                    new Fraction[] { -2, -2 },
                }),
            }, new List<Matrix>()
            {
                new Matrix(new[]
                {
                    new Fraction[] { 1, 1 },
                    new Fraction[] { -1, 0 },
                }),
            }, new List<Matrix>()
            {
                new Matrix(new[]
                {
                    new Fraction[] { 1, -2 },
                    new Fraction[] { 1, 2 },
                }),
                new Matrix(new[]
                {
                    new Fraction[] { 0, 1 },
                    new Fraction[] { 1, 1 },
                }),
            })
        };

        //________________________________________________

        //
        // Polynomial polynomial = new Polynomial(new Fraction(4, 7), new(568, 63), new(3488, 189), new(-6376, 189),
        //     new(-4420, 63));
        // polynomial.GetFactorization();
        //
        // Console.WriteLine();
        // polynomial = new Polynomial(new Fraction(1), new(-3), new(3), new(-1));
        // polynomial.GetFactorization();
        //
        // Console.WriteLine();
        // polynomial = new Polynomial(new Fraction(1), new(137), new(-2476), new(-4106), new(27405), new(25857),
        //     new(-46818));
        // polynomial.GetFactorization();
        //
        // Console.WriteLine();
        // polynomial = new Polynomial(new Fraction(4, 7), new(10252, 1197), new(5816, 513), new(-173464, 3591),
        //     new(-52100, 1197), new(22100, 399));
        //
        // polynomial.GetFactorization();


        // Term term = new Term(new (4,5 ), new(1, 2), new(-5, 7, 3), new(-3));
        // Fraction fraction = new(4, 17);
        // Console.WriteLine($"Term = {term}\nFraction = {fraction}\nTerm/Fraction = {term / fraction}");

        // Polynomial p1 = new Polynomial(new Fraction[] { 14, 3, -2 });
        // Polynomial p2 = new Polynomial(new Fraction[] { 1, 0, 13, 2 });
        // Console.WriteLine(p1);
        // Console.WriteLine(p2);
        // Console.WriteLine(p1 * p2);

        // Task10(data["Kirill"].task10);
        
        Polynomial polynomial = new Polynomial(new Fraction(4, 7), new(10252, 1197), new(5816, 513), new(-173464, 3591),
            new(-52100, 1197), new(22100, 399));
        
        Polynomial p1 = new Polynomial(new Fraction(104), new(-1783, 3), new(244), new(148453, 48), new(-202955, 48),
            new(20137, 16), new(5915, 48));
        
        p1.GetFactorization(true);
        Console.WriteLine(p1.Evaluate(new Term(4, 5)));
    }

    static void Task1((List<Vector> v1, List<Vector> v2) v)
    {
        Matrix a = Matrix.ConcatVectors(v.v1.ToArray()).Transpose();
        Matrix b = Matrix.ConcatVectors(v.v2.ToArray()).Transpose();

        Matrix c = Matrix.ConcatColumns(a, b);
        Console.WriteLine(c.Canonical());

        Matrix res = c.Canonical().TakeColumns(x => x >= 4).Transpose();
        Console.WriteLine($"\nA (матрица линейного оператора) = \n{res}\n");

        LinearSpace l1 = new LinearSpace(4);
        LinearSpace l2 = new LinearSpace(4);

        Console.WriteLine($"Канонический вид A: \n {res.Canonical()}");
        LinearOperator A = new LinearOperator(l1, l2, res);
        Console.WriteLine($"Dim(Ker) = {A.Ker().Dim}");
        Console.WriteLine("Ker = ");
        foreach (var x in A.Ker().Basis)
            Console.WriteLine(x);

        Console.WriteLine($"Dim(Im) = {A.Im().Dim}");
        Console.WriteLine("Im = ");
        foreach (var x in A.Im().Basis)
            Console.WriteLine(x);
    }

    static void Task4((
        int population,
        int light_infect,
        int severe_infect,
        int light_heal,
        int severe_heal,
        int severe_to_light,
        int light_to_severe,
        int current_light,
        int current_severe) data)
    {
        int x0 = data.population - data.current_light - data.current_severe;
        int y0 = data.current_light;
        int z0 = data.current_severe;
        Console.WriteLine(
            $"x - количество здоровых людей; x0 = {x0}\n" +
            $"y - количество больных в легкой форме; y0 = {y0}\n" +
            $"z - количество больных в тяжелой форме; z0 = {z0}\n");
        Console.WriteLine(
            $"x -> x + {data.light_heal / 100d:f2}y + {data.severe_heal / 100d:f2}z - {data.light_infect / 100d:f2}x - {data.severe_infect / 100d:f2}x = " +
            $"{1 - (data.light_infect + data.severe_infect) / 100d:f2}x + {data.light_heal / 100d:f2}y + {data.severe_heal / 100d:f2}z");
        Console.WriteLine(
            $"y -> {data.light_infect / 100d:f2}x + y + {data.severe_to_light / 100d:f2}z - {data.light_heal / 100d:f2}y - {data.light_to_severe / 100d:f2}y = " +
            $"{data.light_infect / 100d:f2}x + {1 - (data.light_heal + data.light_to_severe) / 100d:f2}y + {data.severe_to_light / 100d:f2}z");
        Console.WriteLine(
            $"y -> {data.severe_infect / 100d:f2}x + {data.light_to_severe/100d:f2}y + z - {data.severe_heal / 100d:f2}z - {data.severe_to_light / 100d:f2}z = " +
            $"{data.severe_infect / 100d:f2}x + {data.light_to_severe/100d:f2}y + {1 - (data.severe_heal + data.severe_to_light) / 100d:f2}z");
        
        Matrix m = new Matrix(3, 3);
        m[0, 0] = new Fraction(100 - (data.light_infect + data.severe_infect), 100);
        m[0, 1] = new Fraction(data.light_heal, 100);
        m[0, 2] = new Fraction(data.severe_heal, 100);

        m[1, 0] = new Fraction(data.light_infect, 100);
        m[1, 1] = new Fraction(100 - (data.light_heal + data.light_to_severe), 100);
        m[1, 2] = new Fraction(data.severe_to_light, 100);
        
        m[2, 0] = new Fraction(data.severe_infect, 100);
        m[2, 1] = new Fraction(data.light_to_severe, 100);
        m[2, 2] = new Fraction(100 - (data.severe_heal + data.severe_to_light), 100);
        
        Console.WriteLine($"A = \n{m}");
        Console.WriteLine("Найдём с.з. для A (можно находить с.з. для 100 * A, но тогда потом найденные значения нужно будет поделить на 100):");
        Console.WriteLine("Характеристический многочлен A:\n" +
                          $"f(t) = {m.CharacteristicPolynomial()}".Replace('x', 't'));
        var eigen = m.GetEigenvaluesAndEigenvectors();
        if (eigen.Count != 3)
            throw new UnluckyException("Не были найдены 3 собственных значения. Проблема...");
        for(int i = 0; i < 3; ++i)
        {
            Console.WriteLine($"t{i + 1} = {eigen[i].eigenvalue}");
            Console.WriteLine(m - Matrix.E(3) * eigen[i].eigenvalue);
            Console.WriteLine((m - Matrix.E(3) * eigen[i].eigenvalue).Canonical());
            Console.WriteLine($"с.в. №{i + 1} = \n{eigen[i].eigenvector}\n");
        }

        Matrix C = Matrix.ConcatVectors(eigen[0].eigenvector, eigen[1].eigenvector, eigen[2].eigenvector);
        Console.WriteLine($"C = \n{C}");
        Console.WriteLine($"C^-1 = \n{C.Inverse()}");
        Matrix sus = new Matrix(3, 3);
        sus.Fill(0);
        sus[0, 0] = eigen[0].eigenvalue;
        sus[1, 1] = eigen[1].eigenvalue;
        sus[2, 2] = eigen[2].eigenvalue;
        Console.WriteLine($"V = \n{sus}");
        Console.WriteLine($"C * V * C^-1 = \n{C * sus * C.Inverse()}");
        if (C * sus * C.Inverse() != m)
            Console.WriteLine("Проблема: произведение данных трех матриц должно было совпасть с матрицей A");
        Console.WriteLine("Возводим A в виде произведения трех матриц в степень m, при этом появляются степени m только у чисел, стоящих на диагонали матрицы V,\n" +
                          "остальные матрицы не меняются. После этого устремляем m к бесконечности, при этом в матрице V обнуляются все элементы кроме 1^m");
        sus.Apply(_ => 0, sus.Filter(x => x != 1));
        Console.WriteLine($"V\' = \n{sus}");
        Console.WriteLine($"(x0, y0, z0)^T = ({x0}, {y0}, {z0})^T");
        Matrix B = C * sus * C.Inverse();
        Console.WriteLine($"При m -> inf: B = A^m = C * V\' * C-1 = \n{B}");
        Matrix ans = B * new Vector(true, x0, y0, z0);
        Console.WriteLine($"B * (x0, y0, z0)^T = \n{ans}");
        Console.WriteLine($"С течением времени:\n" +
                          $"\tЗдоровые: {ans[0, 0]}\n" +
                          $"\tБольные в лёгкой форме: {ans[1, 0]}\n" +
                          $"\tБольные в тяжёлой форме: {ans[2, 0]}");

    }

    static void Task5(Matrix matrix)
    {
        var res = matrix.QR(true);
        Console.WriteLine("Check:");
        Console.WriteLine(res.Q * res.R);
    }

    static void Task6((Vector a, Matrix A) pair)
    {
        // fsr не робит, почини
        //var e = new EuclideanSpace(pair.A.FSR());
        var basis = new List<Vector>() { new(true, -1, 3, 3, 1) };
        var e = new EuclideanSpace(basis);
        Console.WriteLine(e.Gram(basis));
        basis.Add(pair.a);
        Console.WriteLine(e.Gram(basis));
        Console.WriteLine(e.Gramian(basis));
        Console.WriteLine(e.DistSqr(pair.a));
        var xProj = new Fraction(-4, 5) * basis[0];
        Console.WriteLine("Dist^2:");
        Console.WriteLine(EuclideanSpace.DistSqr(pair.a, pair.A, new Vector(3, 0)));
        Console.WriteLine("Xproj:");
        Console.WriteLine(xProj);
        Console.WriteLine("(a, xpoj)");
        Console.WriteLine(e.DotProduct(pair.a, xProj));
        Console.WriteLine("(a, a)");
        Console.WriteLine(e.DotProduct(pair.a, pair.a));
        Console.WriteLine("(xproj, xproj)");
        Console.WriteLine(e.DotProduct(xProj, xProj));
        Console.WriteLine("(CosA)^2 = ");
        Console.WriteLine(EuclideanSpace.GetCosSqr(pair.a, pair.A, new Vector(3, 0)));
    }

    static void Task7((List<Matrix>, List<Matrix>, List<Matrix>) tuple)
    {
        Func<Matrix, Matrix, Fraction> dotProduct = (m1, m2) =>
        {
            var res = (m2.Transpose() * m1).Trace();
            Console.WriteLine("DotProduct:");
            Console.WriteLine("M1:");
            Console.WriteLine(m1);
            Console.WriteLine("M2:");
            Console.WriteLine(m2);
            Console.WriteLine("RES:");
            Console.WriteLine(res);
            return res;
        };
        var m = tuple.Item3[0] - tuple.Item3[1];
        Console.WriteLine("X1 - X2");
        Console.WriteLine(m);
        var L1L2 = new List<Matrix>();
        L1L2.AddRange(tuple.Item1);
        L1L2.AddRange(tuple.Item2);
        var gram = EuclideanSpace.Gram(L1L2, dotProduct);
        L1L2.Add(m);
        var gramShtrix = EuclideanSpace.Gram(L1L2, dotProduct);
        Console.WriteLine(gram);
        Console.WriteLine(gramShtrix);
        Console.WriteLine("DistSqr");
        Console.WriteLine(gramShtrix.Det() / gram.Det());
    }

    static void Task9(Matrix A)
    {
        Console.WriteLine($"A = \n{A}");
        if (!A.IsSymmetrical())
        {
            Console.WriteLine("Матрица A несимметрична => не существует спектрального разложения => проблема");
            return;
        }

        Console.WriteLine("Матрица A симметрична => существует спектральное разложение матрицы:");
        var spectral = A.SpectralDecomposition();
        Console.WriteLine($"U = \n{spectral.U}");
        Console.WriteLine($"V = \n{spectral.V}");
        Console.WriteLine($"U_t= \n{spectral.U_T}");
        Console.WriteLine($"U * V * U_t= \n{spectral.U * spectral.V * spectral.U_T}");

        Console.WriteLine(
            $"\n\nОтвет:\nМатрица ортогонального преобразования:\n{spectral.U}\nДиагональный вид:\n{spectral.V}");
    }


    static void Task10(Matrix A)
    {
        Console.WriteLine("Сингулярное разложение матрицы A:");
        var decomposition = A.SingularDecomposition(true);
        
        Console.WriteLine("Сингулярное разложение матрицы A^T:");
        Console.WriteLine("Транспонируем обе части равенства A = V * D * U^T, получим A^T = U * D^T * V^T");
        Console.WriteLine($"U = \n{decomposition.U_T.Transpose()}");
        Console.WriteLine($"D^T = \n{decomposition.D.Transpose()}");
        Console.WriteLine($"V^T = \n{decomposition.V.Transpose()}");
        Console.WriteLine($"\n_____________________________________\n" +
                          $"U * D^T * V^T = \n{decomposition.U_T.Transpose() * decomposition.D.Transpose() * decomposition.V.Transpose()}");

        Fraction sigma1 = decomposition.D[0, 0].Copy();
        decomposition.D.Fill(0);
        decomposition.D[0, 0] = sigma1;
        Console.WriteLine($"Заменим в матрице D все сингуляры кроме первого нулями:\nD' = \n{decomposition.D}");
        Console.WriteLine($"A' = V * D' * U^T = \n{decomposition.V * decomposition.D * decomposition.U_T}");
        Console.WriteLine($"(A')^T = \n{(decomposition.V * decomposition.D * decomposition.U_T).Transpose()}");
    }
}

class Data
{
    public (List<Vector>, List<Vector>) task1;

    public (
        int population,
        int light_infect,
        int severe_infect,
        int light_heal,
        int severe_heal,
        int severe_to_light,
        int light_to_severe,
        int current_light,
        int current_severe) task4; // писать нужно именно проценты
    
    public Matrix task5;
    public (Vector, Matrix) task6;
    public (List<Matrix>, List<Matrix>, List<Matrix>) task7;
    public Matrix task8;
    public Matrix task9;
    public Matrix task10;
}