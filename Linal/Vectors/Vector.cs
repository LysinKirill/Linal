namespace Linal;

public class Vector : Matrix
{
    /*private readonly Fraction[] _data;
    public int Size => _data.Length;
    private bool _transposed;

    public Vector(int size, Fraction value, bool transposed = false)
    {
        _transposed = transposed;
        _data = new Fraction[size];
        Array.Fill(_data, value);
    }

    public Vector(Fraction[] data, bool transposed = false)
    {
        _data = data;
        _transposed = transposed;
    }
    
    public Vector(List<Fraction> data, bool transposed = false)
    {
        _transposed = transposed;
        _data = data.ToArray();
    }

    public static Matrix operator *(Vector vector1, Vector vector2)
    {
        if (vector1._transposed && vector1.Size != vector2.Size)
        {
            throw new ArgumentException("Incorrect dimensions");
        }
        
    }*/

    public int Size => _isVertical ? Rows : Columns;
    private bool _isVertical;
    public Vector(Fraction[] f, bool isVertical = true)
    {
        _isVertical = isVertical;
        if (isVertical)
        {
            _data = new Fraction[f.Length][];
            for (var i = 0; i < _data.Length; i++)
            {
                _data[i] = new Fraction[1];
                _data[i][0] = f[i];
            }
        }
        else
        {
            _data = new Fraction[1][];
            _data[0] = new Fraction[f.Length];
            f.CopyTo(_data[0], 0);
        }
    }

    public Vector(List<Fraction> list, bool isVertical = true) : this(list.ToArray(), isVertical) { }

    public Vector(int size, Fraction value, bool isVertical = true)
    {
        _isVertical = isVertical;
        if (isVertical)
        {
            _data = new Fraction[size][];
            for (var i = 0; i < _data.Length; i++)
            {
                _data[i] = new Fraction[1];
                _data[i][0] = value;
            }
        }
        else
        {
            _data = new Fraction[1][];
            _data[0] = new Fraction[size];
            Array.Fill(_data[0], value);
        }
    }

    public override Vector Transpose()
    {
        var temp = new Fraction[Size];
        for (int i = 0; i < Size; i++)
        {
            temp[i] = this[i];
        }
        return new Vector(temp, !_isVertical);
    }

    public Fraction this[int id]
    {
        get => _isVertical ? _data[id][0] : _data[0][id];
        set
        {
            if (_isVertical)
            {
                _data[id][0] = value;
            }
            else
            {
                _data[0][id] = value;
            }
        }
    }
}