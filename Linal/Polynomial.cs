using System.Text;

namespace Linal;

public class Polynomial
{
    private Dictionary<int, Term> _terms = new();


    public int Degree => _terms.Keys.Max();

    public Polynomial(Dictionary<int, Term> terms) => _terms = terms;
    public Polynomial(params Fraction[] fractions)
    {
        int i = fractions.Length - 1;
        while (i >= 0)
        {
            if (fractions[fractions.Length - i - 1] == 0)
            {
                --i;
                continue;
            }
            _terms[i] = new(fractions[fractions.Length - i - 1]);
            --i;
        }
    }

    public Polynomial(params Term[] terms)
    {
        int i = terms.Length - 1;
        while (i >= 0)
        {
            if (terms[terms.Length - i - 1] == 0)
            {
                --i;
                continue;
            }
            _terms[i] = terms[terms.Length - i - 1];
            --i;
        }
    }

    public Polynomial Derivative(int order = 1)
    {
        Polynomial p = Copy();
        for (int i = 0; i < order; ++i)
        {
            Dictionary<int, Term> dict = new();
            foreach (var term in p._terms)
                if (term.Key > 0)
                    dict[term.Key - 1] = term.Value.Copy() * term.Key;
            p = new Polynomial(dict);
        }

        return p;
    }

    private static long Gcd(params long[] nums)
    {
        if (nums.Length == 1)
            return nums[0];
        if (nums.Length == 2)
            return Gcd(nums[0], nums[1]);
        return Gcd(nums[0], Gcd(nums[1..]));
    }
    
    private static long Lcm(params long[] nums)
    {
        if (nums.Length == 1)
            return nums[0];
        if (nums.Length == 2)
            return Lcm(nums[0], nums[1]);
        return Lcm(nums[0], Lcm(nums[1..]));
    }

    private static long Gcd(long a, long b)
    {
        while (b != 0)
        {
            long temp = b;
            b = a % b;
            a = temp;
        }
        return a;
    }

    private static long Lcm(long a, long b)
    {
        return (a / Gcd(a, b)) * b;
    }
    public long CommonDenominator()
    {
        return Lcm(_terms.Values.Select(x => x.CommonDenominator()).ToArray());
    }

    public List<Fraction> GetFactorization()
    {
        StringBuilder sb = new();
        List<Fraction> factors = new();
        Polynomial poly = Copy();
        Fraction root = null;
        do
        {
            poly.Horner(out poly, out root);
            if (root is not null) 
                factors.Add(root);
            
        } while (root is not null);
        
        sb.Append($"{this} = {poly}");
        if (factors.Count != 0)
        {
            foreach (var factor in factors.OrderByDescending(x => x.GetDouble()))
            {
                if (factor == 0)
                    sb.Append(" * x");
                else
                    sb.Append($" * (x {(factor > new Fraction() ? "-" : "+")} {Fraction.Abs(factor)})");
            }
            Console.WriteLine(sb);
        }
        return factors;
    } 
    public void Horner(out Polynomial poly, out Fraction? root)
    {
        if (_terms.Any(x => !x.Value.IsRational))
            throw new ArgumentException("Cannot perform Horner's method on a polynomial with irrational coefficients");
        //List<Fraction> list = _terms.OrderByDescending(x => x.Key).Select(term => term.Value.GetRootCoefficient(1)).ToList();

        int degree = Degree;
        HashSet<Fraction> divFirst = _terms[degree].GetRootCoefficient(1).GetDivisors();
        HashSet<Fraction> divLast = _terms.ContainsKey(0)
            ? _terms[0].GetRootCoefficient(1).GetDivisors()
            : new HashSet<Fraction>() { 0 };

        HashSet<Fraction> divDenom = new Fraction(CommonDenominator()).GetDivisors();
        foreach (var c in divLast)
        {
            foreach (var a in divFirst)
            {
                foreach (var denom in divDenom)
                {
                    // Ах эти вложенные циклы...
                    // Не уверен, что самый вложенный цикл должен так работать кста, но проверено на целом ОДНОМ тесте
                    Fraction temp = c / (a * denom);
                    // Fraction temp = c / (a / denom);  // Чет бред написал, надо разобраться
                    Fraction sum = _terms[degree].GetRootCoefficient(1);
                    for (int i = degree - 1; i >= 0; --i)
                    {
                        sum *= temp;
                        if (_terms.ContainsKey(i))
                            sum += _terms[i].GetRootCoefficient(1);
                    }

                    if (sum == 0)   // Root found
                    {
                        Dictionary<int, Term> newTerms = new();
                        sum = _terms[degree].GetRootCoefficient(1);
                        newTerms[degree - 1] = _terms[degree].Copy();
                        for (int i = degree - 1; i >= 1; --i)
                        {
                            sum *= temp;
                            if (_terms.ContainsKey(i))
                                sum += _terms[i].GetRootCoefficient(1);
                            if (sum != 0)
                                newTerms[i - 1] = new Term(sum);
                        }

                        poly = new Polynomial(newTerms);
                        root = temp;
                        return;
                    }
                }
            }
        }

        poly = this;
        root = null;
    }
    public Polynomial Copy()
    {
        Dictionary<int, Term> temp = new();
        foreach (var term in _terms)
            temp[term.Key] = term.Value.Copy();
        return new Polynomial(temp);
    }
    
    public override string ToString()
    {
        var query = from term in _terms
            where term.Value != 0
            orderby term.Key descending
            select $"({term.Value}){(term.Key == 0 ? "" : term.Key == 1 ? "*x" : $"*x^{term.Key}")}";
        return string.Join(" + ", query);
    }

    
    // НЕ ТЕСТИРОВАЛ
    
    public static bool operator ==(Polynomial p1, Polynomial p2)
    {
        if (p1._terms.Count != p2._terms.Count)
            return false;
        
        foreach (var term in p1._terms)
            if (!p2._terms.ContainsKey(term.Key) || p2._terms[term.Key] != term.Value)
                return false;

        return true;
    }

    public static bool operator !=(Polynomial p1, Polynomial p2)
    {
        return !(p1 == p2);
    }

    public static bool operator ==(Polynomial p, Fraction x)
    {
        // Надо проверить, могут ли быть члены с нулевыми коэффициентами в полиноме. Т.е. члены вида 0 * x^n
        if (p._terms.Count != 1)    // Сравнивать с дробью можно только полиномы-константы
            return false;
        if (!p._terms.ContainsKey(0))
            return false;
        return p._terms[0] == x;
    }

    public static bool operator !=(Polynomial p, Fraction x)
    {
        return !(p == x);
    }
}