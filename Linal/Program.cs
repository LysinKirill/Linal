namespace Linal;

class Program
{
    public static void Main(string[] args)
    {
        Matrix m = Matrix.GetRandRationalMatrix(3, 3, -4, 4, 5);
        Console.WriteLine(m);
        
        Console.WriteLine(Matrix.Inverse(m));
        
    }
}