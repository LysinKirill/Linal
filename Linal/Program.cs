namespace Linal;

class Program
{
    public static void Main(string[] args)
    {
        Matrix m = new Matrix();
        m.Read();
        
        (Matrix Q, Matrix R) = m.QR();
        
        Console.WriteLine(Q);
        Console.WriteLine(R);
        
        Console.WriteLine(Q.Transpose() * Q);
        Console.WriteLine(Q * R);

        // Matrix m = new Matrix();
        // m.Read();
        // Console.WriteLine(m.Transpose() * m);
    }
}