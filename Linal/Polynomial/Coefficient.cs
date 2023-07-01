using System.Text;
using Linal.Interfaces;

namespace Linal;

public class Coefficient
{
    private List<INum> _coeffs = new();

    public Coefficient(List<INum> coeffs)
    {
        _coeffs = coeffs;
    }

    public void Simplify()
    {
    }

    public override string ToString()
    {
        if (_coeffs.Count == 1)
        {
            return $"{_coeffs[0]}";
        }

        var sb = new StringBuilder();

        for (var i = 0; i < _coeffs.Count; i++)
        {
            if (_coeffs[i].IsNegative)
            {
                sb.Append($"{_coeffs[i]}");
            }
            else
            {
                var str = i == 0 ? $"{_coeffs[i]}" : $"+{_coeffs[i]}";
                sb.Append(str);
            }
        }

        return sb.ToString();
    }
}