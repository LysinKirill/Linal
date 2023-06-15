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
                })
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
                })
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
            }))
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

        
        Task6(data["Maksim"].task6);

        /*Console.WriteLine(EuclideanSpace.DistSqr(new Vector(true, 4, 2, -5, 1), new Matrix(new[]
        {
            new Fraction[] { 2, -2, 1, 2 },
            new Fraction[] { 2, -4, 2, 3 },
        }), new Vector(true, 9, 12)));*/
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

    static void Task5(Matrix matrix)
    {
        var res = matrix.QR(true);
        Console.WriteLine("Check:");
        Console.WriteLine(res.Q * res.R);
    }

    static void Task6((Vector a, Matrix A) pair)
    {
        //var e = new EuclideanSpace(pair.A.FSR());
        var basis = new List<Vector>() { new(true, 7, 3, -4, 6) };
        var e = new EuclideanSpace(basis);
        //Console.WriteLine(e.Gram(basis));
        //basis.Add(pair.a);
        //Console.WriteLine(e.Gram(basis));
        //Console.WriteLine(e.Gramian(basis));
        //Console.WriteLine(e.DistSqr(pair.a));
        var xProj = new Fraction(-20, 110) * basis[0];
        Console.WriteLine(xProj);
        Console.WriteLine(e.DotProduct(pair.a, xProj));
        Console.WriteLine(e.DotProduct(pair.a, pair.a));
        Console.WriteLine(e.DotProduct(xProj, xProj));
        Console.WriteLine(EuclideanSpace.GetCosSqr(pair.a, pair.A, new Vector(3, 0)));
        Console.WriteLine(EuclideanSpace.DistSqr(pair.a, pair.A, new Vector(3, 0)));
    }
}

class Data
{
    public (List<Vector>, List<Vector>) task1;
    public Matrix task5;
    public (Vector, Matrix) task6;
}