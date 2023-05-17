namespace Linal;

class Program
{
    public static void Main(string[] args)
    {
        // List<Matrix> vectors = new List<Matrix>();
        //
        // for (int i = 0; i < 4; ++i)
        // {
        //     vectors.Add(Matrix.ReadVector());
        // }

        // Matrix m = Matrix.ConcatColumns(vectors.ToArray());
        // Console.WriteLine(m);
        // m.Data[1][2] = new Fraction(9);
        //
        // LinearSpace linSpace = new LinearSpace(vectors);
        //
        // Console.WriteLine("Amogus");

        Matrix m = new Matrix();
        m.Read();

        foreach (Matrix vector in m.GetVectors())
            Console.WriteLine(vector + "\n");

    }
}