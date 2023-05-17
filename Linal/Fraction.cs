namespace Linal;

public class Fraction
{
    private static double _eps = 10e-8;

    public long Numerator { get; private set; }
    public long Denominator { get; private set; }

    public long Root { get; private set; }

    private int _lenS;


    public Fraction(Fraction f)
    {
        Numerator = f.Numerator;
        Denominator = f.Denominator;
        Root = f.Root;
    }
    public Fraction(long a, long b, long root = 1)
    {
        Numerator = a;
        Denominator = b;
        Root = root;
        if (root < 0)
            throw new ArgumentException("Square root of a negative number is not defined in ℝ");
        if (root == 0)
            Numerator = 0;
        if (b == 0)
            throw new DivideByZeroException("Attempted to divide by zero.");
        Simplify();
    }

    public Fraction(long a) : this(a, 1)
    {
    }

    private void Simplify()
    {
        // haven't checked this part of code yet.
        long i = 2;
        while (i * i <= Root)
        {
            if (Root % (i * i) == 0)
            {
                Numerator *= i;
                Root /= i * i;
                continue;
            }

            ++i;
        }


        long n = Gcd(Numerator, Denominator);
        Numerator /= n;
        Denominator /= n;
        if ((Numerator < 0 && Denominator < 0) || (Numerator > 0 && Denominator < 0))
        {
            Numerator *= -1;
            Denominator *= -1;
        }

        _lenS = ToString().Length;
    }

    public int GetLenS() => _lenS;

    public static double GetDouble(Fraction f) => (double)f.Numerator / f.Denominator;

    

    public override string ToString()
    {
        if (Denominator == 1)
        {
            if (Numerator == 1 && Root != 1)
                return $"√({Root})";
            if (Numerator == -1 && Root != 1)
                return $"-√({Root})";
            return Numerator + (Root == 1 ? "" : $"√({Root})");
        }

        return
            $"{(Root == 1 ? Numerator : Numerator == 1 ? "" : Numerator == -1 ? "-" : Numerator) + (Root == 1 ? "" : $"√({Root})")}/{Denominator}";
    }

    private static long Gcd(long a, long b)
    {
        (a, b) = (Math.Abs(a), Math.Abs(b));
        while (b > 0)
        {
            (a, b) = (b, a % b);
        }

        return a;
    }

    public static Fraction Abs(Fraction fraction) =>
        new(Math.Abs(fraction.Numerator), fraction.Denominator, fraction.Root);

    public static Fraction Parse(String s)
    {
        s = s.Replace(" ", "").Replace("*", "").ToLower();
        long num = 1;
        if (s[0] == '-')
        {
            num = -1;
            s = s[1..];
        }

        long root = 1;

        if (s.Contains('/'))
        {
            string[] a = s.Split("/");

            long den;
            if (a[0].Contains("sqrt("))
            {
                int aux = a[0].IndexOf("sqrt(");
                int end = a[0].IndexOf(")");
                root = long.Parse(a[0][(aux + 5)..end]);
                num *= aux == 0 ? 1 : long.Parse(a[0][..aux]);
            }
            else
            {
                num *= long.Parse(a[0]);
            }

            if (a[1].Contains("sqrt("))
            {
                int aux = a[1].IndexOf("sqrt(");
                int end = a[1].IndexOf(")");
                long tempRoot = long.Parse(a[1][(aux + 5)..end]);
                root *= tempRoot;
                den = aux == 0 ? 1 : long.Parse(a[1][..aux]);
                den *= tempRoot;
            }
            else
            {
                den = long.Parse(a[1]);
            }

            if (num == 0 || root == 0)
                return den == 0
                    ? throw new DivideByZeroException("Attempted to divide by zero.")
                    : new Fraction(0, 1);
            return new Fraction(num, den, root);
        }

        if (s.Contains("sqrt("))
        {
            int aux = s.IndexOf("sqrt(");
            int end = s.IndexOf(")");
            root = long.Parse(s[(aux + 5)..end]);
            num *= aux == 0 ? 1 : long.Parse(s[..aux]);

            if (num == 0 || root == 0)
                return new Fraction(0, 1);

            return new Fraction(num, 1, root);
        }

        num = long.Parse(s);
        return new Fraction(num);
    }

    public static Fraction Inverse(Fraction fraction) => fraction.Numerator == 0 ? throw new ArgumentException("Zero has no inverse") : new Fraction(fraction.Denominator, fraction.Numerator * fraction.Root, fraction.Root);

    public static Fraction operator *(Fraction a, Fraction b)
    {
        Fraction f = new Fraction(a.Numerator * b.Numerator, a.Denominator * b.Denominator, a.Root * b.Root);
        f.Simplify();
        return f;
    }

    public static Fraction operator +(Fraction a, Fraction b)
    {
        if (a == 0)
            return b;
        if (b == 0)
            return a;
        if (a.Root != b.Root && a.Root != 0 && b.Root != 0)
            throw new RazrabDaunException("Cannot add two fractions with different radicals (not implemented).");
        // Конректная проблема... Как в такой реализации складывать две дроби, у которых в числителе два разных корня?
        Fraction f = new Fraction(a.Numerator * b.Denominator + b.Numerator * a.Denominator,
            a.Denominator * b.Denominator, Math.Max(a.Root, b.Root));
        f.Simplify();
        return f;
    }

    public static Fraction operator *(int a, Fraction b)
    {
        Fraction f = new Fraction(a * b.Numerator, b.Denominator, b.Root);
        f.Simplify();
        return f;
    }

    public static Fraction operator +(int a, Fraction b)
    {
        // Как в такой реализации складывать число с дробью, у которой есть корень в числителе?

        if (b.Root != 1 || b.Root != 0)
            throw new RazrabDaunException("Cannot add a number to a fraction containing radicals (not implemented).");

        return new Fraction(a * b.Denominator + b.Numerator, b.Denominator);
    }

    public static Fraction operator -(int a, Fraction b) => a + (b * (-1));
    public static Fraction operator -(Fraction b, int a) => b + (-a);
    public static Fraction operator -(Fraction a, Fraction b) => a + (b * (-1));
    public static Fraction operator +(Fraction b, int a) => (a + b);
    public static Fraction operator *(Fraction b, int a) => (a * b);
    public static Fraction operator -(Fraction f) => f * (-1);


    public static Fraction operator /(Fraction a, int b)
    {
        Fraction f = new Fraction(a.Numerator, a.Numerator * b);
        f.Simplify();
        return f;
    }

    public static Fraction operator /(int b, Fraction a)
    {
        Fraction f = new Fraction(b * a.Denominator, a.Numerator);
        f.Simplify();
        return f;
    }

    public static Fraction operator /(Fraction a, Fraction b)
    {
        Fraction f = new Fraction(a.Numerator * b.Denominator, a.Denominator * b.Numerator);
        f.Simplify();
        return f;
    }

    public static void SetEqualityPrecision(double eps)
    {
        _eps = eps > 0 ? eps : throw new ArgumentException("Epsilon value should be represented by a positive double");
    }

    public static bool operator ==(Fraction f1, Fraction f2) =>
        ((f1.Numerator == f2.Numerator) && (f1.Denominator == f2.Denominator));

    public static bool operator !=(Fraction f1, Fraction f2) => !(f1 == f2);
    public static bool operator >(Fraction f1, Fraction f2) => GetDouble(f1) > GetDouble(f2);
    public static bool operator <(Fraction f1, Fraction f2) => GetDouble(f1) < GetDouble(f2);
    public static bool operator >=(Fraction f1, Fraction f2) => GetDouble(f1) >= GetDouble(f2);
    public static bool operator <=(Fraction f1, Fraction f2) => GetDouble(f1) <= GetDouble(f2);

    public static bool operator ==(Fraction f, double d) => Math.Abs(GetDouble(f) - d) < _eps;
    public static bool operator !=(Fraction f, double d) => !(f == d);
    public static bool operator >(Fraction f, double d) => GetDouble(f) > d;
    public static bool operator <(Fraction f, double d) => GetDouble(f) < d;
    public static bool operator >=(Fraction f, double d) => GetDouble(f) >= d;
    public static bool operator <=(Fraction f, double d) => GetDouble(f) <= d;

    public static bool operator ==(double d, Fraction f) => Math.Abs(GetDouble(f) - d) < _eps;
    public static bool operator !=(double d, Fraction f) => !(d == f);
    public static bool operator >(double d, Fraction f) => d > GetDouble(f);
    public static bool operator <(double d, Fraction f) => d < GetDouble(f);
    public static bool operator >=(double d, Fraction f) => d >= GetDouble(f);
    public static bool operator <=(double d, Fraction f) => d <= GetDouble(f);


    public static explicit operator Fraction(long x) => new(x);
}