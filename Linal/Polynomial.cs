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