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

        Matrix m = Matrix.ConcatColumns(vectors.ToArray());
        Console.WriteLine(m);
        
        LinearSpace linSpace = new LinearSpace(vectors);
        
        Console.WriteLine("Amogus");

    }
}