namespace Linal;

class Program
{
    public static void Main(string[] args)
    {
        Matrix m = Matrix.E(5);

        while (m.Rank() != 3)
        {
            m = Matrix.GetRandIntMatrix(5, 5, -3, 3);
        }
        
        Console.WriteLine(m);
        Console.WriteLine($"Rank(A) = {m.Rank()}");
        Console.WriteLine(m * m.Transpose());
        Console.WriteLine($"Rank(A * A^T) = {(m * m.Transpose()).Rank()}");
        
        Console.WriteLine((m*m.Transpose()).Canonical());

    }
}