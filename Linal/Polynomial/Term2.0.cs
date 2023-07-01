namespace Linal;

public class Term2_0
{
    public string Name { get; init; }
    public int Exponent { get; set; }
    public Coefficient Coefficient { get; set; }

    public Term2_0(Coefficient coefficient, int exponent = 1 , string name = "x")
    {
        Name = name;
        Coefficient = coefficient;
        Exponent = exponent;
    }
    
    public static Term2_0 operator+(Term2_0 term1, Term2_0 term2)
    {
        throw new NotImplementedException();
    }

    public override string ToString() => 
        Exponent == 0 ? $"{Coefficient}" : $"{Coefficient}{Name}^{Exponent}";
}