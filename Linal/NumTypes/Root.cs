using Linal.Interfaces;

namespace Linal;

public class Root : INum
{
    public int Exponent { get; set; }
    public Fraction Value { get; set; }
    public bool IsNegative { get; set; }
    
    public Root(int exponent, Fraction value, bool isNegative)
    {
        Exponent = exponent;
        Value = value;
        IsNegative = isNegative;
    }
}