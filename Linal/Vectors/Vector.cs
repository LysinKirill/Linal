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

    public int Size => IsVertical ? Rows : Columns;
    public bool IsVertical { get; }

    public Vector(Fraction[] f, bool isVertical = true)
    {
        IsVertical = isVertical;
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
        IsVertical = isVertical;
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

    public Vector(Vector vector)
    {
        IsVertical = vector.IsVertical;
        if (IsVertical)
        {
            _data = new Fraction[vector.Size][];
            for (var i = 0; i < _data.Length; i++)
            {
                _data[i] = new Fraction[1];
                _data[i][0] = vector[i];
            }
        }
        else
        {
            _data = new Fraction[1][];
            _data[0] = new Fraction[vector.Size];
            Array.Copy(vector._data[0], _data[0], vector.Size);
        }
    }

    public Vector(Matrix matrix)
    {
        if (matrix.Rows != 1 && matrix.Columns != 1)
        {
            throw new ArgumentException("Not vectorizable matrix");
        }

        if (matrix.Columns == 1)
        {
            IsVertical = true;
            _data = new Fraction[matrix.Rows][];
            for (var i = 0; i < _data.Length; i++)
            {
                _data[i] = new Fraction[1];
                _data[i][0] = matrix[i,0];
            }
        }
        else
        {
            IsVertical = false;
            _data = new Fraction[1][];
            _data[0] = new Fraction[matrix.Columns];
            Array.Copy(matrix.GetRow(0)._data[0], _data[0], matrix.Columns);
        }
    }

    public override Vector Transpose()
    {
        var temp = new Fraction[Size];
        for (int i = 0; i < Size; i++)
        {
            temp[i] = this[i];
        }
        return new Vector(temp, !IsVertical);
    }

    public override Vector Copy()
    {
        var temp = base.Copy();
        return new Vector(temp.GetColumn(0));
    }

    public static Vector ReadVector()
    {
        return new Vector(Array.ConvertAll(Console.ReadLine()!.Split(), Fraction.Parse).ToList());
    }
    
    public Fraction this[int id]
    {
        get => IsVertical ? _data[id][0] : _data[0][id];
        set
        {
            if (IsVertical)
            {
                _data[id][0] = value;
            }
            else
            {
                _data[0][id] = value;
            }
        }
    }

    public static Vector operator *(Vector vector, Fraction num)
    {
        Matrix m1 = vector;
        var res = m1 * num;
        return vector.IsVertical ? new Vector(res.GetColumn(0)) : new Vector(res.GetRow(0));
    }
    
    public static Vector operator *(Fraction num, Vector vector)
    {
        return vector * num;
    }
    
    public static Vector operator-(Vector vector1, Vector vector2)
    {
        Matrix m1 = vector1;
        Matrix m2 = vector2;
        var res = m1 - m2;
        return vector1.IsVertical ? new Vector(res.GetColumn(0)) : new Vector(res.GetRow(0));
    }
}