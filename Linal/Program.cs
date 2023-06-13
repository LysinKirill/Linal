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


        List<Vector> V1 = new List<Vector>()
        {
            new(new Fraction[] { -1, 3, 5, 2 }),
            new(new Fraction[] { 0, 1, -4, -5 }),
            new(new Fraction[] { 2, -4, 0, 5 }),
            new(new Fraction[] { 0, -2, 4, -5 }),
        };
        List<Vector> V2 = new List<Vector>() {
            new (new  Fraction[] {-1, 1, 1, -1}),
            new (new  Fraction[] {2, -1, -1, 2}),
            new (new  Fraction[] {-3, 2, 2, -3}),
            new (new  Fraction[] {-2, 5, 5, -2}),
        };

        LinearSpace l1 = new LinearSpace(V1);
        LinearSpace l2 = new LinearSpace(V2);
        LinearOperator linearOperator = new LinearOperator(l1, l2, matrix => matrix);
        Console.WriteLine(linearOperator.A);
        // Matrix m = Matrix.ConcatVectors(vectors.ToArray());
        // Console.WriteLine(m);
        // Console.WriteLine(m.Canonical());
    }
}