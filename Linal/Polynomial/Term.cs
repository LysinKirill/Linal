using System.Text;

namespace Linal;

public class Term
{
    private readonly Dictionary<long, Fraction> _coefficients = new Dictionary<long, Fraction>();

    public bool IsRational => _coefficients.Count == 0 || (_coefficients.Count == 1 && _coefficients.ContainsKey(1));   // Remove check for coef.Count == 0???

    public Term() => _coefficients[0] = 0;

    public Term(Fraction f) => _coefficients[f.Root] = f;

    public Term(params Fraction[] fractions)
    {
        foreach (Fraction fraction in fractions)
        {
            if (!_coefficients.ContainsKey(fraction.Root))
                _coefficients[fraction.Root] = 0;
            _coefficients[fraction.Root] += fraction;
            if (_coefficients[fraction.Root] == 0)
                _coefficients.Remove(fraction.Root);
        }

    }

    public Term(List<Fraction> fractions) : this(fractions.ToArray())
    {
    }

    public double GetDouble() => _coefficients.Sum(x => x.Value.GetDouble());

    public Fraction GetRootCoefficient(long rootPart) => _coefficients[rootPart];

    public Term Copy()
    {
        List<Fraction> temp = new();
        foreach (var coefficient in _coefficients)
            temp.Add(coefficient.Value.Copy());
        return new Term(temp);
    }

    public Term Parse(String s)
    {
        // НЕ ТЕСТИРОВАЛ
        return new Term(s.Replace(" ", "").Split("+").Select(Fraction.Parse).ToArray());
    }

    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        var query = from coef in _coefficients orderby coef.Key descending select coef.Value.ToString();
        return string.Join(" + ", query);
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
        return Lcm(_coefficients.Values.Select(x => x.Denominator).ToArray());
    }


    public static bool operator ==(Term term1, Term term2)
    {
        // НЕ ТЕСТИРОВАЛ
        if (term1._coefficients.Count != term2._coefficients.Count)
            return false;

        foreach (var coef in term1._coefficients)
            if (!term2._coefficients.ContainsKey(coef.Key) || term2._coefficients[coef.Key] != coef.Value)
                return false;

        return true;
    }

    // НЕ ТЕСТИРОВАЛ
    
    public static bool operator !=(Term term1, Term term2) => !(term1 == term2);

    public static bool operator >(Term term1, Term term2) => term1.GetDouble() > term2.GetDouble();

    public static bool operator <(Term term1, Term term2) => term1.GetDouble() < term2.GetDouble();

    public static bool operator >=(Term term1, Term term2) => term1 > term2 || term1 == term2;

    public static bool operator <=(Term term1, Term term2) => term1 < term2 || term1 == term2;

    public static Term operator -(Term term) => new(term._coefficients.Select(x => -x.Value.Copy()).ToArray());

    public static Term operator +(Term term, Fraction fraction)
    {
        // НЕ ТЕСТИРОВАЛ
        Term temp = term.Copy();
        if (!temp._coefficients.ContainsKey(fraction.Root))
            temp._coefficients[fraction.Root] = 0;
        temp._coefficients[fraction.Root] += fraction;
        return temp;
    }

    // НЕ ТЕСТИРОВАЛ
    public static Term operator +(Fraction fraction, Term term) => term + fraction;

    public static Term operator -(Term term, Fraction fraction) => term + (-fraction);
    public static Term operator -(Fraction fraction, Term term) => fraction + (-term);

    public static Term operator +(Term term1, Term term2)
    {
        // НЕ ТЕСТИРОВАЛ
        Term temp = term1.Copy();
        foreach (var x in term2._coefficients)
            temp += x.Value;

        return temp;
    }

    public static Term operator -(Term term1, Term term2) => term1 + (-term2);

    public static bool operator ==(Term term, Fraction x)
    {
        // НЕ ТЕСТИРОВАЛ
        if (term._coefficients.Count != 1)
            return false;
        if (!term._coefficients.ContainsKey(1))
            return false;
        
        return term._coefficients[1] == x;
    }

    public static bool operator !=(Term term, Fraction x)
    {
        // НЕ ТЕСТИРОВАЛ
        return !(term == x);
    }

    public static Term operator *(Term term, long x)
    {
        Term copy = term.Copy();
        foreach (var key in term._coefficients.Keys)
        {
            copy._coefficients[key] *= x;
        }

        return copy;
    }

    public static Term operator *(long x, Term term) => term * x;
}