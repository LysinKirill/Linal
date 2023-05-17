﻿using System.Diagnostics;
using System.Text;

namespace Linal;

public class Matrix
{
    Fraction[,] _data;
    public int Rows { get; private set; }
    public int Columns { get; private set; }

    private int _maxS;

    public Matrix(Fraction[,] f)
    {
        Rows = f.GetLength(0);
        Columns = f.GetLength(1);
        _data = f;
        _maxS = GetMaxLength();
    }
    
    public Matrix(List<List<Fraction>> fractionList)
    {
        Rows = fractionList.Count;
        Columns = fractionList[0].Count;
        _data = new Fraction[Rows, Columns];
        
        for(int i = 0; i < Rows; ++i)
        {
            for(int j = 0; j < Columns; ++j)
            {
                _data[i, j] = fractionList[i][j];
            }
        }
    }

    public Matrix()
    {
        Rows = 0;
        Columns = 0;
        _data = null;
    }
    public Matrix(int rows, int columns)
    {
        if (rows <= 0 || columns <= 0)
            throw new ArgumentException("Rows and columns of matrix have to be positive integers");
        Rows = rows;
        Columns = columns;
        _data = new Fraction[rows, columns];
    }

    public static Matrix Copy(Matrix matrix)
    {
        Matrix copy = new Matrix();
        copy.Rows = matrix.Rows;
        copy.Columns = matrix.Columns;
        copy._data = new Fraction[copy.Rows, copy.Columns];

        for (int i = 0; i < copy.Rows; ++i)
        for (int j = 0; j < copy.Columns; ++j)
            copy._data[i, j] = new Fraction(matrix._data[i, j]);

        return copy;
    }

    public static Matrix Parse(string s)
    {
        string[] lines = s.Split("\n");
        int m = lines.Length;
        int n = lines[0].Split(" ").Length;
        Matrix a = new Matrix();
        a._data = new Fraction[m, n];
        a.Rows = m;
        a.Columns = n;
        for (int i = 0; i < m; i++)
        {
            string[] arr = lines[i].Split(" ");
            for (int j = 0; j < n; j++)
            {
                a._data[i, j] = Fraction.Parse(arr[j]);
            }
        }

        a._maxS = a.GetMaxLength();

        return a;
    }

    public void Read(int m, int n)
    {
        (int, int) size = (m, n);
        _data = new Fraction[size.Item1, size.Item2];
        (m, n) = size;
        string[] s;
        for (int i = 0; i < size.Item1; i++)
        {
            s = Console.ReadLine()!.Split(" ");
            for (int j = 0; j < size.Item2; j++)
                _data[i, j] = Fraction.Parse(s[j]);
        }

        _maxS = GetMaxLength();
        Console.WriteLine();
    }
    
    public static Matrix ConcatColumns(Matrix a, Matrix b)
    {
        Matrix newMatrix = new Matrix();
        newMatrix.Columns = a.Columns + b.Columns;
        newMatrix.Rows = a.Rows + b.Rows;
        newMatrix._data = new Fraction[newMatrix.Rows, newMatrix.Columns];
        for(int i = 0; i < newMatrix.Rows; ++i)
        {
            for(int j = 0; j < newMatrix.Columns; ++j)
            {
                newMatrix._data[i, j] = j < a.Columns ? a._data[i, j] : b._data[i, j - a.Columns];
            }
        }
        return newMatrix;
    }
    
    public static Matrix ConcatColumns(params Matrix[] matrixes)
    {
        int commonRowCount = matrixes[0].Rows;

        List<List<Fraction>> rows = new ();
        
        for(int i = 0; i < commonRowCount; ++i)
            rows.Add(new List<Fraction>());
        
        for(int i = 0; i < commonRowCount; ++i)
        {
            foreach(var matrix in matrixes)
            {
                for (int j = 0; j < matrix.Columns; ++j)
                {
                    rows[i].Add(matrix[i, j]);
                }
            }
        }
        return new Matrix(rows);
    }

    public int Rang()
    {
        Matrix r = Reduce(this);
        for (int i = Rows - 1; i >= 0; ++i)
        {
            for (int j = 0; j < Columns; ++i)
            {
                if (r._data[i, j] != 0)
                    return i + 1;
            }
        }

        return 0;
    }
    
    public void Read()
    {
        String[] s = Console.ReadLine()!.Split(" ");
        (int, int) size = (int.Parse(s[0]), int.Parse(s[1]));
        _data = new Fraction[size.Item1, size.Item2];
        (Rows, Columns) = size;
        for (int i = 0; i < size.Item1; i++)
        {
            s = Console.ReadLine()!.Split(" ");
            for (int j = 0; j < size.Item2; j++)
                _data[i, j] = Fraction.Parse(s[j]);
        }

        _maxS = GetMaxLength();
        Console.WriteLine();
    }

    public void Write()
    {
        Console.WriteLine(this);
    }

    private int GetMaxLength()
    {
        int max = -1;
        for (int i = 0; i < Rows; i++)
        for (int j = 0; j < Columns; j++)
            if (_data[i, j].GetLenS() > max)
                max = _data[i, j].GetLenS();
        return max;
    }

    public static Matrix Transpose(Matrix p)
    {
        Matrix tr = new Matrix();
        (tr.Rows, tr.Columns) = (p.Columns, p.Rows);
        tr._data = new Fraction[tr.Rows, tr.Columns];
        for (int i = 0; i < tr.Rows; i++)
        for (int j = 0; j < tr.Columns; j++)
            tr._data[i, j] = p._data[j, i];
        return tr;
    }

    public Matrix Select(Func<Fraction, Fraction> func)
    {
        Matrix copy = Copy(this);
        copy.Apply(func);
        return copy;
    }
    public Matrix SelectRows(Func<Fraction, Fraction> func, params int[] rows)
    {
        Matrix copy = Copy(this);
        copy.ApplyRows(func, rows);
        return copy;
    }
    public Matrix SelectRow(Func<Fraction, Fraction> func, int row) => SelectRows(func, row);
    
    public Matrix SelectColumns(Func<Fraction, Fraction> func, params int[] columns)
    {
        Matrix copy = Copy(this);
        copy.ApplyColumns(func, columns);
        return copy;
    }
    public Matrix SelectColumn(Func<Fraction, Fraction> func, int column) => SelectColumns(func, column);

    public void Apply(Func<Fraction, Fraction> func)
    {
        for (int i = 0; i < Rows; ++i)
        for (int j = 0; j < Columns; ++j)
            _data[i, j] = func(_data[i, j]);
    }

    public void ApplyRows(Func<Fraction, Fraction> func, params int[] rows)
    {
        foreach(int i in rows)
            for (int j = 0; j < Columns; ++j)
                _data[i, j] = func(_data[i, j]);
    }

    public void ApplyRow(Func<Fraction, Fraction> func, int row) => ApplyRows(func, row);
    
    public void ApplyColumns(Func<Fraction, Fraction> func, params int[] columns)
    {
        for(int i = 0; i < Rows; ++i)
            foreach(int j in columns)
                _data[i, j] = func(_data[i, j]);
    }

    public void ApplyColumn(Func<Fraction, Fraction> func, int column) => ApplyRows(func, column);
    
    public static Matrix Reduce(Matrix matrix)
    {
        Matrix m = Copy(matrix);
        
        
        int shift = 0;
        for (int i = 0; i < m.Rows; ++i)
        {
            int aux = i - shift;
            while (aux < m.Rows && m[aux, i] == 0)
                ++aux;
            if (aux == m.Rows)
            {
                ++shift;
                continue;
            }
            
            m.SwapRows(i - shift, aux);
            Fraction inv = Fraction.Inverse(m[i - shift, i]);
            m.ApplyRow(x => x * inv, i - shift);
            
            for (int k = i - shift + 1; k < m.Rows; ++k)
            {
                if (m[k, i] == 0)
                    continue;
                inv = Fraction.Inverse(m[k, i]);
                m.ApplyRow(x => x * inv, k);
                m.AddToRow(k, i - shift, new Fraction(-1));
            }
        }

        return m;
    }

    public static Matrix Canonical(Matrix matrix)
    {
        Matrix m = Reduce(matrix);

        for (int row = m.Rows - 1; row >= 0; --row)
        {
            int aux = 0;
            while (aux < m.Columns && m[row, aux] == 0)
                ++aux;
            if(aux == m.Columns)
                continue;

            for (int i = 0; i < row; i++)
            {
                if (m[i, aux] == 0)
                    continue;
                m.AddToRow(i, row, -m[i, aux]);
            }
        }

        return m;
    }



    /// <summary>
    /// Calculates and returns adjugate matrix for matrix a
    /// </summary>
    /// <param name="a">Square matrix a</param>
    /// <returns>adjugate matrix for a</returns>
    /// <exception cref="Exception"></exception>
    public static Matrix Adj(Matrix a)
    {
        if (!IsSquare(a))
            throw new Exception("Cannot get an adjugate of a non-square matrix");
        Fraction[,] arr = new Fraction[a.Rows, a.Rows];
        for (int i = 0; i < a.Rows; i++)
        {
            for (int j = 0; j < a.Rows; j++)
            {
                arr[i, j] = A(a, i, j);
            }
        }


        return Transpose(new Matrix(arr));
    }

    public static bool TryInverse(Matrix m, out Matrix invMatrix)
    {
        Fraction det = Det(m);
        if (det == 0)
        {
            invMatrix = null;
            return false;
        }

        invMatrix = (1 / det) * Adj(m);
        return true;
    }

    public static Matrix Inverse(Matrix m)
    {
        Fraction det = Det(m);
        if (det == 0)
            throw new Exception("The matrix is degenerate, thus it has no inverse");
        return (1 / det) * Adj(m);
    }

    /// <summary>
    /// Calculates Co-factor matrix
    /// </summary>
    /// <param name="a">square matrix A</param>
    /// <param name="i">omitted row</param>
    /// <param name="j">omitted column</param>
    /// <returns>Co-factor matrix for A</returns>
    /// <exception cref="Exception"></exception>
    public static Fraction A(Matrix a, int i, int j)
    {
        if (i >= a.Rows || j >= a.Columns)
            throw new Exception("Cannot get such algebraic complement");
        if ((i + j) % 2 == 0)
        {
            return Det(Minor(a, i, j));
        }

        return (-1) * Det(Minor(a, i, j));
    }

    public static Matrix Minor(Matrix a, int i, int j)
    {
        if (i >= a.Rows || j >= a.Columns)
            throw new Exception("Cannot get such Minor");
        Fraction[,] f = new Fraction[a.Rows - 1, a.Columns - 1];


        for (int p = 0; p < a.Rows; p++)
        {
            if (p == i)
                continue;
            int y = p;
            if (p >= i)
                y -= 1;
            for (int l = 0; l < a.Columns; l++)
            {
                if (l == j)
                    continue;
                int x = l;
                if (l >= j)
                    x -= 1;
                f[y, x] = a._data[p, l];
            }
        }

        return new Matrix(f);
    }

    public static Fraction Det(Matrix a)
    {
        if (!IsSquare(a))
            throw new Exception();

        if (a.Columns == 1)
            return a._data[0, 0];

        if (a.Columns == 2)
            return a._data[0, 0] * a._data[1, 1] - a._data[0, 1] * a._data[1, 0];
        Fraction sum = new Fraction(0);
        //  разложение по 1-ой строке
        for (int j = 0; j < a.Columns; j++)
        {
            sum += a._data[0, j] * A(a, 0, j); // need to check this
        }

        return sum;
    }

    public static Fraction Trace(Matrix matrix)
    {
        if (!IsSquare(matrix))
            throw new ArgumentException("Trace is only defined for square matrices");

        Fraction sum = new Fraction(0);

        for (int i = 0; i < matrix.Columns; ++i)
            sum += matrix._data[i, i];
        return sum;
    }

    public static bool IsSquare(Matrix matrix) => matrix.Rows == matrix.Columns;

    public void SwapRows(int row1, int row2)
    {
        if (row1 == row2)
            return;
        for (int i = 0; i < Columns; i++)
            (_data[row1, i], _data[row2, i]) = (_data[row2, i], _data[row1, i]);
    }

    public void SwapColumns(int col1, int col2)
    {
        if (col1 == col2)
            return;
        for (int i = 0; i < Rows; i++)
            (_data[i, col1], _data[i, col2]) = (_data[i, col2], _data[i, col1]);
    }

    public Fraction this[int i, int j]
    {
        get => _data[i, j];
        set => _data[i, j] = value;
    }

    public static Matrix operator *(Matrix a, Matrix b)
    {
        if (!(a.Columns == b.Rows))
            throw new Exception();
        Matrix c = new Matrix();
        c.Rows = a.Rows;
        c.Columns = b.Columns;
        c._data = new Fraction[c.Rows, c.Columns];
        for (int i = 0; i < a.Rows; i++)
        for (int j = 0; j < b.Columns; j++)
        {
            Fraction s = new Fraction(0);
            for (int k = 0; k < a.Columns; k++)
                s += a._data[i, k] * b._data[k, j];
            c._data[i, j] = s;
        }

        c._maxS = c.GetMaxLength();
        return c;
    }

    public static Matrix operator +(Matrix a, Matrix b)
    {
        if (!((a.Rows == b.Rows) & (a.Columns == b.Columns)))
            throw new Exception();
        Matrix c = new Matrix();
        (c.Rows, c.Columns) = (a.Rows, a.Columns);
        c._data = new Fraction[c.Rows, c.Columns];
        for (int i = 0; i < c.Rows; i++)
        for (int j = 0; j < c.Columns; j++)
            c._data[i, j] = a._data[i, j] + b._data[i, j];
        c._maxS = c.GetMaxLength();
        return c;
    }

    public static Matrix operator -(Matrix a, Matrix b)
    {
        if (!((a.Rows == b.Rows) & (a.Columns == b.Columns)))
            throw new Exception();
        Matrix c = new Matrix();
        (c.Rows, c.Columns) = (a.Rows, a.Columns);
        c._data = new Fraction[c.Rows, c.Columns];
        for (int i = 0; i < c.Rows; i++)
        for (int j = 0; j < c.Columns; j++)
            c._data[i, j] = a._data[i, j] - b._data[i, j];
        c._maxS = c.GetMaxLength();
        return c;
    }


    public static Matrix operator *(Matrix a, int num)
    {
        Matrix c = new Matrix();
        c.Rows = a.Rows;
        c.Columns = a.Columns;
        c._data = new Fraction[a.Rows, a.Columns];
        for (int i = 0; i < c.Rows; i++)
        for (int j = 0; j < c.Columns; j++)
            c._data[i, j] = a._data[i, j] * num;

        c._maxS = c.GetMaxLength();
        return c;
    }

    public static Matrix operator ^(Matrix matrix, int power)
    {
        if (matrix.Rows != matrix.Columns)
            throw new ArgumentException("Cannot raise non-square matrix to a power");
        if (power == 0)
            return E(matrix.Columns);

        if (power < 0)
            return Inverse(matrix) * matrix ^ Math.Abs(power);

        if (power % 2 == 1)
            return matrix * (matrix ^ (power - 1));

        return (matrix * matrix) ^ (power / 2);
    }

    public static Matrix operator *(int num, Matrix a) => a * num;

    public void AddToRow(int i, int j, Fraction alpha)
    {
        for (int k = 0; k < Columns; k++)
        {
            _data[i, k] += _data[j, k] * alpha;
        }

        _maxS = GetMaxLength();
    }


    public static Matrix operator *(Matrix a, Fraction num)
    {
        Matrix c = new Matrix();
        c.Rows = a.Rows;
        c.Columns = a.Columns;
        c._data = new Fraction[a.Rows, a.Columns];
        for (int i = 0; i < c.Rows; i++)
        {
            for (int j = 0; j < c.Columns; j++)
            {
                c._data[i, j] = a._data[i, j] * num;
            }
        }

        c._maxS = c.GetMaxLength();

        return c;
    }

    public static Matrix operator *(Fraction num, Matrix a) => a * num;

    public static bool operator ==(Matrix a, Matrix b)
    {
        if (!((a.Rows == b.Rows) && (a.Columns == b.Columns)))
            return false;


        for (int i = 0; i < a.Columns; i++)
        {
            for (int j = 0; j < a.Rows; j++)
            {
                if (a._data[i, j] != b._data[i, j])
                    return false;
            }
        }

        return true;
    }

    public static bool operator !=(Matrix a, Matrix b) => !(a == b);

    public void AddToRow(int i, int j) => AddToRow(i, j, new Fraction(1));

    public static Matrix Pow(Matrix a, int pow)
    {
        if (a.Columns != a.Rows)
            throw new Exception("Wrong matrix dimensions");
        if (pow == 0)
            return Matrix.E(a.Columns); // ? check

        Fraction[,] d = new Fraction[a.Columns, a.Columns];
        for (int i = 0; i < a.Columns; i++)
        for (int j = 0; j < a.Columns; j++)
            d[i, j] = a._data[i, j];
        Matrix m = new Matrix(d);
        for (int i = 1; i < pow; i++)
            m = m * a;
        return m;
    }

    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < Rows; i++)
        {
            for (int j = 0; j < Columns; j++)
            {
                sb.Append($"{_data[i, j].ToString().PadRight(_maxS)}");
                if (j != (Columns - 1))
                    sb.Append(" ");
            }

            sb.Append("\n");
        }

        return sb.ToString();
    }

    public static Matrix E(int n)
    {
        Fraction[,] a = new Fraction[n, n];
        for (int p = 0; p < n; p++)
        for (int l = 0; l < n; l++)
            if (p == l)
                a[p, l] = new Fraction(1);
            else
                a[p, l] = new Fraction(0);

        return new Matrix(a);
    }

    public static Matrix Eij(int n, int i, int j)
    {
        if (i >= n || j >= n)
            throw new IndexOutOfRangeException();
        Fraction[,] a = new Fraction[n, n];
        for (int p = 0; p < n; p++)
        for (int l = 0; l < n; l++)
            a[p, l] = new Fraction(0);

        a[i, j] = new Fraction(1);

        return new Matrix(a);
    }

    public static Matrix ESwap(int n, int i, int j)
    {
        Matrix m = E(n);
        m._data[i, i] = new Fraction(0);
        m._data[i, j] = new Fraction(1);

        m._data[j, j] = new Fraction(0);
        m._data[j, i] = new Fraction(1);
        return m;
    }

    public static Matrix EMultRow(int n, int i, Fraction p)
    {
        Matrix m = Matrix.E(n);
        m._data[i, i] = p;
        return m;
    }


    public static Matrix EAddRow(int n, int i, int j, int k)
    {
        Matrix m = E(n);
        m._data[i, j] = new Fraction(k);
        return m;
    }

    public static Matrix GetRandIntMatrix(int m, int n, int a, int b)
    {
        Random rand = new Random();
        Fraction[,] arr = new Fraction[m, n];
        for (int i = 0; i < m; i++)
        {
            for (int j = 0; j < n; j++)
            {
                arr[i, j] = new Fraction(rand.Next(a, b + 1));
            }
        }

        return new Matrix(arr);
    }

    public static Matrix GetRandRationalMatrix(int m, int n, long a, long b, long max_den)
    {
        Random rand = new Random();
        Fraction[,] arr = new Fraction[m, n];
        for (int i = 0; i < m; i++)
        {
            for (int j = 0; j < n; j++)
            {
                arr[i, j] = new Fraction(rand.NextInt64(a, b + 1), rand.NextInt64(1, max_den + 1));
            }
        }

        return new Matrix(arr);
    }
    public static Matrix SolveForX()
    {
        throw new NotImplementedException();
    }
}