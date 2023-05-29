namespace Linal;

class Program
{
    public static void Main(string[] args)
    {
        List<Matrix> vectors = new List<Matrix>();
        
        for (int i = 0; i < 4; ++i)
        {
            vectors.Add(Matrix.ReadVector());
        }

        var eSpace = new EuclideanSpace(vectors, (v1, v2) =>
        {
            var res = new Fraction(0);
            for (int i = 0; i < v1.Rows; i++)
            {
                res += v1[i, 0] * v2[i, 0];
            }

            return res;
        });

        var nSpace = eSpace.Orthogonalize();
        Console.WriteLine("Amogus");
        //Matrix m = new Matrix();
        //m.Read();

        //Console.WriteLine(Matrix.Diagonalize(m));

    }
}