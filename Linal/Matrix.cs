using System.Text;

namespace Linal;

public class Matrix
{
    private Fraction[][] _data;
    public int Rows { get; private set; }
    public int Columns { get; private set; }

    private int _maxS;

    public Matrix(Fraction[][] f)
    {
        Rows = f.Length;
        Columns = f[0].Length;
        _data = f;
        _maxS = GetMaxLength();

        // Добавить проверку, что матрица прямоугольная?
    }

    public Matrix(List<List<Fraction>> fractionList)
    {
        Rows = fractionList.Count;
        Columns = fractionList[0].Count;
        _data = new Fraction[Rows][];

        for (int i = 0; i < Rows; ++i)
        {
            _data[i] = new Fraction[Columns];
            for (int j = 0; j < Columns; ++j)
            {
                _data[i][j] = fractionList[i][j];
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
        _data = new Fraction[rows][];
        for (int i = 0; i < Rows; ++i)
            _data[i] = new Fraction[Columns];
    }

    public Matrix Copy()
    {
        var copy = new Matrix
        {
            Rows = Rows,
            Columns = Columns,
            _data = new Fraction[Rows][]
        };
        for (int i = 0; i < copy.Rows; ++i)
        {
            copy._data[i] = new Fraction[copy.Columns];
            for (int j = 0; j < copy.Columns; ++j)
                copy._data[i][j] = new Fraction(_data[i][j]);
        }


        return copy;
    }

    public bool IsDiagonal()
    {
        for (int i = 0; i < Rows; ++i)
        for (int j = 0; j < Columns; ++j)
        {
            if (i == j)
                continue;
            if (_data[i][j] != 0)
                return false;
        }

        return true;
    }

    public bool IsSymmetrical()
    {
        if (!IsSquare())
            return false;
        for (int i = 0; i < Rows; ++i)
        for (int j = i + 1; j < Rows; ++j)
            if (_data[i][j] != _data[j][i])
                return false;
        return true;
    }

    public Matrix Diagonalize()
    {
        if (!IsSymmetrical())
            throw new ArgumentException("Matrix should be symmetrical");

        var m = Copy();

        for (int i = 0; i < m.Rows; ++i)
        {
            if (m[i, i] != 0)
            {
                var k = Fraction.Abs(m[i, i]);
                m.ApplyRow(x => x / k, i);
                for (int col = i + 1; col < m.Columns; ++col)
                {
                    m[col, col] -= k * m[i, col] * m[i, col];
                    for (int j = col + 1; j < m.Columns; j++)
                    {
                        m[col, j] -= k * m[i, col] * m[i, j];
                    }

                    m[i, col] = new Fraction(0);
                }
            }
        }

        for (int i = 0; i < m.Rows; ++i)
        for (int j = i + 1; j < m.Columns; ++j)
            if (i < j)
                m[j, i] = m[i, j];
        return m;
    }

    // public static Matrix Parse(string s)
    // {
    //     string[] lines = s.Split("\n");
    //     int m = lines.Length;
    //     int n = lines[0].Split(" ").Length;
    //     Matrix a = new Matrix();
    //     a._data = new Fraction[m, n];
    //     a.Rows = m;
    //     a.Columns = n;
    //     for (int i = 0; i < m; i++)
    //     {
    //         string[] arr = lines[i].Split(" ");
    //         for (int j = 0; j < n; j++)
    //         {
    //             a._data[i, j] = Fraction.Parse(arr[j]);
    //         }
    //     }
    //
    //     a._maxS = a.GetMaxLength();
    //
    //     return a;
    // }

    public void Read(int m, int n)
    {
        (int, int) size = (m, n);
        _data = new Fraction[size.Item1][];
        for (int i = 0; i < size.Item1; i++)
        {
            _data[i] = new Fraction[size.Item2];
            var s = Console.ReadLine()!.Split(" ");
            for (int j = 0; j < size.Item2; j++)
                _data[i][j] = Fraction.Parse(s[j]);
        }

        _maxS = GetMaxLength();
        Console.WriteLine();
    }

    public static Matrix ReadVector()
    {
        List<List<Fraction>> rows = new() { Array.ConvertAll(Console.ReadLine().Split(), Fraction.Parse).ToList() };
        return new Matrix(rows).Transpose();
    }

    public static Matrix ConcatColumns(Matrix a, Matrix b)
    {
        var newMatrix = new Matrix
        {
            Columns = a.Columns + b.Columns,
            Rows = a.Rows + b.Rows
        };
        newMatrix._data = new Fraction[newMatrix.Rows][];
        for (int i = 0; i < newMatrix.Rows; ++i)
        {
            newMatrix._data[i] = new Fraction[newMatrix.Columns];
            for (int j = 0; j < newMatrix.Columns; ++j)
            {
                newMatrix._data[i][j] = j < a.Columns ? a._data[i][j] : b._data[i][j - a.Columns];
            }
        }

        return newMatrix;
    }

    public static Matrix ConcatColumns(params Matrix[] matrices)
    {
        var commonRowCount = matrices[0].Rows;

        List<List<Fraction>> rows = new();

        for (int i = 0; i < commonRowCount; ++i)
            rows.Add(new List<Fraction>());

        for (int i = 0; i < commonRowCount; ++i)
        {
            foreach (var matrix in matrices)
            {
                for (int j = 0; j < matrix.Columns; ++j)
                {
                    rows[i].Add(matrix[i, j]);
                }
            }
        }

        return new Matrix(rows);
    }

    public List<Matrix> GetVectors()
    {
        var temp = Transpose();


        var rows = temp._data.Select(row => row.ToList()).ToList();
        List<Matrix> vectors = new();
        foreach (var row in rows)
        {
            var tempData = new Fraction[row.Count][];
            for (int i = 0; i < row.Count; ++i)
            {
                tempData[i] = new Fraction[1];
                tempData[i][0] = row[i];
            }


            vectors.Add(new Matrix(tempData));
        }

        return vectors;
    }

    public int Rank()
    {
        var r = Reduce();
        for (int i = Rows - 1; i >= 0; --i)
        {
            for (int j = 0; j < Columns; ++j)
            {
                if (r._data[i][j] != 0)
                    return i + 1;
            }
        }

        return 0;
    }

    public void Read()
    {
        var s = Console.ReadLine()!.Split(" ");
        var size = (int.Parse(s[0]), int.Parse(s[1]));
        _data = new Fraction[size.Item1][];
        (Rows, Columns) = size;
        for (int i = 0; i < size.Item1; i++)
        {
            _data[i] = new Fraction[size.Item2];
            s = Console.ReadLine()!.Split(" ");
            for (int j = 0; j < size.Item2; j++)
                _data[i][j] = Fraction.Parse(s[j]);
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
        var max = -1;
        for (int i = 0; i < Rows; i++)
        for (int j = 0; j < Columns; j++)
            if (_data[i][j].GetLenS() > max)
                max = _data[i][j].GetLenS();
        return max;
    }

    public Matrix Transpose()
    {
        var tr = new Matrix();
        (tr.Rows, tr.Columns) = (Columns, Rows);
        tr._data = new Fraction[tr.Rows][];
        for (int i = 0; i < tr.Rows; i++)
        {
            tr._data[i] = new Fraction[tr.Columns];
            for (int j = 0; j < tr.Columns; j++)
                tr._data[i][j] = _data[j][i];
        }

        return tr;
    }

    public Matrix Select(Func<Fraction, Fraction> func)
    {
        var copy = Copy();
        copy.Apply(func);
        return copy;
    }

    public Matrix SelectRows(Func<Fraction, Fraction> func, params int[] rows)
    {
        var copy = Copy();
        copy.ApplyRows(func, rows);
        return copy;
    }

    public Matrix SelectRow(Func<Fraction, Fraction> func, int row) => SelectRows(func, row);

    public Matrix SelectColumns(Func<Fraction, Fraction> func, params int[] columns)
    {
        var copy = Copy();
        copy.ApplyColumns(func, columns);
        return copy;
    }

    public Matrix SelectColumn(Func<Fraction, Fraction> func, int column) => SelectColumns(func, column);

    public void Apply(Func<Fraction, Fraction> func)
    {
        for (int i = 0; i < Rows; ++i)
        for (int j = 0; j < Columns; ++j)
            _data[i][j] = func(_data[i][j]);
    }

    // This method has not been tested yet
    public void Apply(Func<Fraction, Fraction> func, bool[][] filter)
    {
        if (Rows != filter.Length || Columns != filter[0].Length)
            throw new ArgumentException(
                "The dimensions of the boolean matrix do not match the dimension of the matrix being changed");
        for (int i = 0; i < Rows; ++i)
        for (int j = 0; j < Columns; ++j)
            if (filter[i][j])
                _data[i][j] = func(_data[i][j]);
    }

    // This method has not been tested yet
    public bool[][] Filter(Predicate<Fraction> condition)
    {
        var filter = new bool[Rows][];
        for (int i = 0; i < Rows; ++i)
        {
            filter[i] = new bool[Columns];
            for (int j = 0; j < Columns; ++j)
                filter[i][j] = condition(_data[i][j]);
        }

        return filter;
    }

    // This method has not been tested yet
    public bool[][] Filter(Func<int, int, bool> checkPos)
    {
        var filter = new bool[Rows][];
        for (int i = 0; i < Rows; ++i)
        {
            filter[i] = new bool[Columns];
            for (int j = 0; j < Columns; ++j)
                filter[i][j] = checkPos(i, j);
        }

        return filter;
    }

    public void ApplyRows(Func<Fraction, Fraction> func, params int[] rows)
    {
        foreach (var i in rows)
            for (int j = 0; j < Columns; ++j)
                _data[i][j] = func(_data[i][j]);
    }

    public void ApplyRow(Func<Fraction, Fraction> func, int row) => ApplyRows(func, row);

    public void ApplyColumns(Func<Fraction, Fraction> func, params int[] columns)
    {
        for (int i = 0; i < Rows; ++i)
            foreach (var j in columns)
                _data[i][j] = func(_data[i][j]);
    }

    public void ApplyColumn(Func<Fraction, Fraction> func, int column) => ApplyRows(func, column);

    // This method has not been tested yet
    public Matrix Reduce()
    {
        var m = Copy();


        var shift = 0;
        for (int i = 0; i < m.Rows; ++i)
        {
            var inverse = new Fraction();
            if (i < m.Columns)
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
                inverse = Fraction.Inverse(m[i - shift, i]);
                m.ApplyRow(x => x * inverse, i - shift);
            }

            for (int k = i - shift + 1; k < m.Rows; ++k)
            {
                if (m[k, i] == 0)
                    continue;
                inverse = Fraction.Inverse(m[k, i]);
                m.ApplyRow(x => x * inverse, k);
                m.AddToRow(k, i - shift, new Fraction(-1));
            }
        }

        return m;
    }

    // This method has not been tested yet
    public Matrix Canonical()
    {
        var m = Reduce();

        for (int row = m.Rows - 1; row >= 0; --row)
        {
            var aux = 0;
            while (aux < m.Columns && m[row, aux] == 0)
                ++aux;
            if (aux == m.Columns)
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
    public Matrix Adj()
    {
        if (!IsSquare())
            throw new Exception("Cannot get an adjugate of a non-square matrix");
        var arr = new Fraction[Rows][];
        for (int i = 0; i < Rows; i++)
        {
            arr[i] = new Fraction[Rows];
            for (int j = 0; j < Rows; j++)
            {
                arr[i][j] = A(i, j);
            }
        }


        return new Matrix(arr).Transpose();
    }

    public bool TryInverse(out Matrix invMatrix)
    {
        var det = Det();
        if (det == 0)
        {
            invMatrix = null;
            return false;
        }

        invMatrix = (1 / det) * Adj();
        return true;
    }

    public Matrix Inverse()
    {
        var det = Det();
        if (det == 0)
            throw new Exception("The matrix is degenerate, thus it has no inverse");
        return (1 / det) * Adj();
    }

    /// <summary>
    /// Calculates Co-factor matrix
    /// </summary>
    /// <param name="a">square matrix A</param>
    /// <param name="i">omitted row</param>
    /// <param name="j">omitted column</param>
    /// <returns>Co-factor matrix for A</returns>
    /// <exception cref="Exception"></exception>
    public Fraction A(int i, int j)
    {
        if (i >= Rows || j >= Columns)
            throw new Exception("Cannot get such algebraic complement");
        if ((i + j) % 2 == 0)
        {
            return Minor(i, j).Det();
        }

        return (-1) * Minor(i, j).Det();
    }

    public Matrix Minor(int i, int j)
    {
        if (i >= Rows || j >= Columns)
            throw new Exception("Cannot get such Minor");
        var f = new Fraction[Rows - 1][];
        for (int t = 0; t < Rows - 1; ++t)
            f[t] = new Fraction[Columns - 1];

        for (int p = 0; p < Rows; p++)
        {
            if (p == i)
                continue;
            var y = p;
            if (p >= i)
                y -= 1;
            for (int l = 0; l < Columns; l++)
            {
                if (l == j)
                    continue;
                var x = l;
                if (l >= j)
                    x -= 1;
                f[y][x] = _data[p][l];
            }
        }

        return new Matrix(f);
    }

    public Fraction Det()
    {
        if (!IsSquare())
            throw new Exception();

        switch (Columns)
        {
            case 1:
                return _data[0][0];
            case 2:
                return _data[0][0] * _data[1][1] - _data[0][1] * _data[1][0];
        }

        var sum = new Fraction(0);
        //  разложение по 1-ой строке
        for (int j = 0; j < Columns; j++)
        {
            sum += _data[0][j] * A(0, j); // need to check this
        }

        return sum;
    }

    public Fraction Trace()
    {
        if (!IsSquare())
            throw new ArgumentException("Trace is only defined for square matrices");

        var sum = new Fraction(0);

        for (int i = 0; i < Columns; ++i)
            sum += _data[i][i];
        return sum;
    }

    public bool IsSquare() => Rows == Columns;

    public void SwapRows(int row1, int row2)
    {
        // Сделано
        // Можно сделать нормальную реализацию, т.к. массив теперь зубчатый
        if (row1 == row2)
            return;
        (_data[row1], _data[row2]) = (_data[row2], _data[row1]);
        //for (int i = 0; i < Columns; i++)
        //    (_data[row1][i], _data[row2][i]) = (_data[row2][i], _data[row1][i]);
    }

    public void SwapColumns(int col1, int col2)
    {
        if (col1 == col2)
            return;
        for (int i = 0; i < Rows; i++)
            (_data[i][col1], _data[i][col2]) = (_data[i][col2], _data[i][col1]);
    }

    public Fraction this[int i, int j]
    {
        get => _data[i][j];
        set => _data[i][j] = value;
    }

    public static Matrix operator *(Matrix a, Matrix b)
    {
        if (a.Columns != b.Rows)
            throw new ArgumentException("Неверные размерности");
        var c = new Matrix
        {
            Rows = a.Rows,
            Columns = b.Columns,
            _data = new Fraction[a.Rows][]
        };
        for (int i = 0; i < a.Rows; i++)
        {
            c._data[i] = new Fraction[c.Columns];
            for (int j = 0; j < b.Columns; j++)
            {
                var s = new Fraction(0);
                for (int k = 0; k < a.Columns; k++)
                    s += a._data[i][k] * b._data[k][j];
                c._data[i][j] = s;
            }
        }

        c._maxS = c.GetMaxLength();
        return c;
    }
    
    public static Matrix operator *(Matrix a, int num)
    {
        var c = new Matrix
        {
            Rows = a.Rows,
            Columns = a.Columns,
            _data = new Fraction[a.Rows][]
        };
        for (int i = 0; i < c.Rows; i++)
        {
            c._data[i] = new Fraction[c.Columns];
            for (int j = 0; j < c.Columns; j++)
                c._data[i][j] = a._data[i][j] * num;
        }

        c._maxS = c.GetMaxLength();
        return c;
    }
    
    public static Matrix operator *(int num, Matrix a) => a * num;
    
    public static Matrix operator *(Matrix a, Fraction num)
    {
        var c = new Matrix
        {
            Rows = a.Rows,
            Columns = a.Columns,
            _data = new Fraction[a.Rows][]
        };
        for (int i = 0; i < c.Rows; i++)
        {
            c._data[i] = new Fraction[a.Columns];
            for (int j = 0; j < c.Columns; j++)
            {
                c._data[i][j] = a._data[i][j] * num;
            }
        }

        c._maxS = c.GetMaxLength();

        return c;
    }

    public static Matrix operator *(Fraction num, Matrix a) => a * num;

    public static Matrix operator +(Matrix a, Matrix b)
    {
        if (!((a.Rows == b.Rows) & (a.Columns == b.Columns)))
            throw new Exception();
        var c = new Matrix();
        (c.Rows, c.Columns) = (a.Rows, a.Columns);
        c._data = new Fraction[c.Rows][];
        for (int i = 0; i < c.Rows; i++)
        {
            c._data[i] = new Fraction[c.Columns];
            for (int j = 0; j < c.Columns; j++)
                c._data[i][j] = a._data[i][j] + b._data[i][j];
        }

        c._maxS = c.GetMaxLength();
        return c;
    }

    public static Matrix operator -(Matrix a, Matrix b)
    {
        if (!((a.Rows == b.Rows) & (a.Columns == b.Columns)))
            throw new Exception();
        var c = new Matrix();
        (c.Rows, c.Columns) = (a.Rows, a.Columns);
        c._data = new Fraction[c.Rows][];
        for (int i = 0; i < c.Rows; i++)
        {
            c._data[i] = new Fraction[c.Columns];
            for (int j = 0; j < c.Columns; j++)
                c._data[i][j] = a._data[i][j] - b._data[i][j];
        }

        c._maxS = c.GetMaxLength();
        return c;
    }

    public static Matrix operator ^(Matrix matrix, int power)
    {
        if (!matrix.IsSquare())
            throw new ArgumentException("Cannot raise non-square matrix to a power");
        if (power == 0)
            return E(matrix.Columns);
        
        // What is the meaning of negative power other than -1?
        if (power < 0)
            return (matrix ^ Math.Abs(power)).Inverse();
        
        if (power % 2 == 1)
            return matrix * (matrix ^ (power - 1));

        return (matrix * matrix) ^ (power / 2);
    }

    public void AddToRow(int i, int j, Fraction alpha)
    {
        for (int k = 0; k < Columns; k++)
        {
            _data[i][k] += _data[j][k] * alpha;
        }

        _maxS = GetMaxLength();
    }
    
    public void AddToRow(int i, int j) => AddToRow(i, j, new Fraction(1));

    public static bool operator ==(Matrix a, Matrix b)
    {
        if (!((a.Rows == b.Rows) && (a.Columns == b.Columns)))
            return false;


        for (int i = 0; i < a.Rows; i++)
        {
            for (int j = 0; j < a.Columns; j++)
            {
                if (a._data[i][j] != b._data[i][j])
                    return false;
            }
        }

        return true;
    }

    public static bool operator !=(Matrix a, Matrix b) => !(a == b);

    // Удалить бесполезный метод?
    /*public static Matrix Pow(Matrix a, int pow)
    {
        if (!a.IsSquare())
            throw new Exception("Wrong matrix dimensions");
        if (pow == 0)
            return Matrix.E(a.Columns); // ? check

        Fraction[][] d = new Fraction[a.Columns][];
        for (int i = 0; i < a.Columns; i++)
        {
            d[i] = new Fraction[a.Columns];
            for (int j = 0; j < a.Columns; j++)
                d[i][j] = a._data[i][j];
        }

        Matrix m = new Matrix(d);
        for (int i = 1; i < pow; i++)
            m *= a;
        return m;
    }*/

    public override string ToString()
    {
        var sb = new StringBuilder();
        for (int i = 0; i < Rows; i++)
        {
            for (int j = 0; j < Columns; j++)
            {
                sb.Append($"{_data[i][j].ToString().PadRight(_maxS)}");
                if (j != (Columns - 1))
                    sb.Append(" ");
            }

            sb.Append("\n");
        }

        return sb.ToString();
    }

    public static Matrix E(int n)
    {
        var a = new Fraction[n][];
        for (int p = 0; p < n; p++)
        {
            a[p] = new Fraction[n];
            for (int l = 0; l < n; l++)
                if (p == l)
                    a[p][l] = new Fraction(1);
                else
                    a[p][l] = new Fraction(0);
        }

        return new Matrix(a);
    }

    public static Matrix Eij(int n, int i, int j)
    {
        if (i >= n || j >= n)
            throw new IndexOutOfRangeException();
        var a = new Fraction[n][];
        for (int p = 0; p < n; p++)
        {
            a[p] = new Fraction[n];
            for (int l = 0; l < n; l++)
                a[p][l] = new Fraction(0);
        }
        a[i][j] = new Fraction(1);
        return new Matrix(a);
    }

    public static Matrix ESwap(int n, int i, int j)
    {
        var m = E(n);
        m._data[i][i] = new Fraction(0);
        m._data[i][j] = new Fraction(1);

        m._data[j][j] = new Fraction(0);
        m._data[j][i] = new Fraction(1);
        return m;
    }

    public static Matrix EMultRow(int n, int i, Fraction p)
    {
        var m = Matrix.E(n);
        m._data[i][i] = p;
        return m;
    }


    public static Matrix EAddRow(int n, int i, int j, int k)
    {
        var m = E(n);
        m._data[i][j] = new Fraction(k);
        return m;
    }

    public static Matrix GetRandIntMatrix(int rows, int columns, int intervalStart, int intervalEnd)
    {
        var rand = new Random();
        var arr = new Fraction[rows][];
        for (int i = 0; i < rows; i++)
        {
            arr[i] = new Fraction[columns];
            for (int j = 0; j < columns; j++)
            {
                arr[i][j] = new Fraction(rand.Next(intervalStart, intervalEnd + 1));
            }
        }

        return new Matrix(arr);
    }


    public Matrix TakeColumns(Predicate<int> f)
    {
        List<List<Fraction>> rows = new();


        for (int i = 0; i < Rows; ++i)
        {
            rows[i] = new List<Fraction>();
            for (int j = 0; j < Columns; ++j)
            {
                if (f(j))
                    rows[i].Add(_data[i][j]);
            }
        }

        return new Matrix(rows);
    }

    public static Matrix GetRandRationalMatrix(int rows, int columns, long intervalStart, long intervalEnd, long maxDenominator)
    {
        var rand = new Random();
        var arr = new Fraction[rows][];
        for (int i = 0; i < rows; i++)
        {
            arr[i] = new Fraction[columns];
            for (int j = 0; j < columns; j++)
            {
                arr[i][j] = new Fraction(rand.NextInt64(intervalStart, intervalEnd + 1), rand.NextInt64(1, maxDenominator + 1));
            }
        }

        return new Matrix(arr);
    }

    public static Matrix SolveForX()
    {
        throw new NotImplementedException();
    }
}