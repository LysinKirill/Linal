using System.Text;

namespace Linal;

public class Matrix
{
    protected Fraction[][] _data;
    public int Rows => _data.Length;
    public int Columns => Rows == 0 ? 0 : _data[0].Length;

    private int _maxS;

    /// <summary>
    /// Создает матрицу на основе зубчатого массива из дробей
    /// </summary>
    /// <param name="f">Зубчатый массив из дробей</param>
    /// <exception cref="ArgumentException">Исключение, возникающее при попытке создания матрицы из зубчатого массива, не являющегося прямоугольным</exception>
    public Matrix(Fraction[][] f)
    {
        if (!IsValidMatrixRepresentation(f))
            throw new ArgumentException("The given jagged array is not rectangular.");
        _data = f;
        _maxS = GetMaxLength();
    }

    public Matrix(params Vector[] vectors)
    {
        foreach (var vector in vectors)
        {
            if (!vector.IsVertical)
            {
                vector.Transpose();
            }
        }
        var temp = ConcatVectors(vectors);
        _data = temp._data;
        _maxS = temp._maxS;
    }
    
    /// <summary>
    /// Создает матрицу на основе списка из списков дробей - строк матрицы
    /// </summary>
    /// <param name="fractionList">Список из списков дробей, которые будут строками новой матрицы</param>
    /// <exception cref="ArgumentException">Исключение, возникающее при передаче в качестве параметра списка, который не соответствует прямоугольной матрице</exception>
    public Matrix(List<List<Fraction>> fractionList)
    {
        if(!IsValidMatrixRepresentation(fractionList))
            throw new ArgumentException("The given list does not represent a rectangular matrix.");
        _data = new Fraction[fractionList.Count][];

        for (int i = 0; i < Rows; ++i)
        {
            _data[i] = new Fraction[fractionList[0].Count];
            for (int j = 0; j < Columns; ++j)
            {
                _data[i][j] = fractionList[i][j];
            }
        }
    }
    
    /// <summary>
    /// Конструктор без параметров - создает пустую матрицу размера 0 x 0
    /// </summary>
    public Matrix()
    {
        _data = Array.Empty<Fraction[]>();
    }
    
    /// <summary>
    /// Конструктор принимающий размеры матрицы
    /// </summary>
    /// <param name="rows">количество строк в матрице</param>
    /// <param name="columns">количество столбцов в матрице</param>
    /// <exception cref="ArgumentException">Исключение, возникающее при попытке создать матрицу с неположительным числом строк или столбцов</exception>
    public Matrix(int rows, int columns)
    {
        if (rows <= 0 || columns <= 0)
            throw new ArgumentException("Rows and columns of matrix have to be positive integers");
        _data = new Fraction[rows][];
        for (int i = 0; i < Rows; ++i)
            _data[i] = new Fraction[Columns];
    }
    
    /// <summary>
    /// Метод, осуществляющий копирование матрицы
    /// </summary>
    /// <returns>Новая матрица - копия данной</returns>
    public virtual Matrix Copy()
    {
        var copy = new Matrix
        {
            _maxS = _maxS,
            _data = new Fraction[Rows][]
        };
        for (int i = 0; i < copy.Rows; ++i)
        {
            copy._data[i] = new Fraction[Columns];
            for (int j = 0; j < copy.Columns; ++j)
                copy._data[i][j] = new Fraction(_data[i][j]);
        }


        return copy;
    }

    public Vector GetRow(int id)
    {
        if (id >= Rows)
        {
            throw new ArgumentException("Invalid row");
        }
        
        return new Vector(_data[id], false);
    }
    
    public Vector GetColumn(int id)
    {
        if (id >= Columns)
        {
            throw new ArgumentException("Invalid row");
        }
        
        var temp = new Fraction[Rows];
        for (int i = 0; i < Rows; i++)
        {
            temp[i] = _data[i][id];
        }
        return new Vector(temp);
    }

    /// <summary>
    /// Проверка матрицы на диагональность
    /// </summary>
    /// <returns>Логическое значение - является матрица диагональной или нет</returns>
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
    
    /// <summary>
    /// Проверка матрицы на симметричность
    /// </summary>
    /// <returns>Логическое значение - является матрица симметричной или нет</returns>
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

    /// <summary>
    /// Приведение матрицы к диагональному виду
    /// </summary>
    /// <returns>Новая матрица - диагональное представление исходной матрицы</returns>
    /// <exception cref="ArgumentException">Исключение, возникающее при попытке диагонализации недиагонализируемой матрицы</exception>
    public Matrix Diagonalize()
    {
        if (!IsSymmetrical())
            // Является ли это необходимым условием?
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

                    m[i, col] = new Fraction();
                }
            }
        }

        for (int i = 0; i < m.Rows; ++i)
        for (int j = i + 1; j < m.Columns; ++j)
            if (i < j)
                m[j, i] = m[i, j];

        throw new NotImplementedException();
        return m;
    }

    /// <summary>
    /// Осуществляет считывание матрицы заданного размера из консоли
    /// </summary>
    /// <param name="m">Количество строк в считываемой матрице</param>
    /// <param name="n">Количество столбцов в считываемой матрице</param>
    public void Read(int m, int n)
    {
        _data = new Fraction[m][];
        for (int i = 0; i < m; i++)
        {
            _data[i] = new Fraction[n];
            var s = Console.ReadLine()!.Split(" ");
            for (int j = 0; j < n; j++)
                _data[i][j] = Fraction.Parse(s[j]);
        }

        _maxS = GetMaxLength();
        Console.WriteLine();
    }

    // /// <summary>
    // /// Осуществляет считывание вектора из консоли
    // /// </summary>
    // /// <returns>Новый вектор</returns>
    // public static Matrix ReadVector()
    // {
    //     List<List<Fraction>> rows = new() { Array.ConvertAll(Console.ReadLine()!.Split(), Fraction.Parse).ToList() };
    //     return new Matrix(rows).Transpose();
    // }

    /// <summary>
    /// Составляет новую матрицу путем объединения (конкатенации) переданных матриц по столбцам
    /// </summary>
    /// <param name="matrices">Матрицы, которые необходимо объединить</param>
    /// <returns>Новая матрица - результат конкатенации матриц по столбцам</returns>
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
    
    public static Matrix ConcatVectors(params Vector[] vectors)
    {
        if (vectors.Length == 0)
            return new Matrix();
        
        var commonRowCount = vectors[0].Rows;

        List<List<Fraction>> rows = new();

        /*for (int i = 0; i < commonRowCount; ++i)
            rows.Add(new List<Fraction>());
        */
        for (int i = 0; i < commonRowCount; ++i)
        {
            rows.Add(new List<Fraction>());
            foreach (var vector in vectors)
            {
                rows[i].Add(vector[i]);
            }
        }

        return new Matrix(rows);
    }

    /// <summary>
    /// Представляет матрицу в виде набора вектор-столбцов
    /// </summary>
    /// <returns>Список из вектор-столбцов</returns>
    public List<Vector> GetVectors()
    {
        var temp = Transpose();


        var rows = temp._data.Select(row => row.ToList()).ToList();
        List<Vector> vectors = new();
        foreach (var row in rows)
        {
            var tempData = new Fraction[row.Count];
            for (int i = 0; i < row.Count; ++i)
            {
                tempData[i] = row[i];
            }


            vectors.Add(new Vector(tempData));
        }

        return vectors;
    }

    /// <summary>
    /// Вычисляет ранг данной матрицы
    /// </summary>
    /// <returns>Ранг матрицы</returns>
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

    /// <summary>
    /// Считывает матрицу из консоли
    /// </summary>
    public void Read()
    {
        var s = Console.ReadLine()!.Split(" ");
        var size = (int.Parse(s[0]), int.Parse(s[1]));
        _data = new Fraction[size.Item1][];
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

    /// <summary>
    /// Вычисляет максимальную длину символьного представления элементов матрицы
    /// </summary>
    /// <returns>Наибольшая длина символьного представления для элемента матрицы</returns>
    private int GetMaxLength()
    {
        var max = -1;
        for (int i = 0; i < Rows; i++)
        for (int j = 0; j < Columns; j++)
            if (_data[i][j].GetLenS() > max)
                max = _data[i][j].GetLenS();
        return max;
    }

    /// <summary>
    /// Вычисляет транспонированную матрицу
    /// </summary>
    /// <returns>Транспонированная матрица</returns>
    public virtual Matrix Transpose()
    {
        var tr = new Matrix
        {
            _data = new Fraction[Columns][]
        };
        for (int i = 0; i < tr.Rows; i++)
        {
            tr._data[i] = new Fraction[Rows];
            for (int j = 0; j < tr.Columns; j++)
                tr._data[i][j] = _data[j][i];
        }

        return tr;
    }

    /// <summary>
    /// Создает новую матрицу путем применения отображения ко всем элементам матрицы
    /// </summary>
    /// <param name="func">Применяемое отображение φ: Fraction ⟼ Fraction</param>
    /// <returns>Матрица, составленная из образов элементов под действием отображения</returns>
    public Matrix Select(Func<Fraction, Fraction> func)
    {
        var copy = Copy();
        copy.Apply(func);
        return copy;
    }

    /// <summary>
    /// Создает новую матрицу путем применения отображения к выбранным строкам текущей матрицы
    /// </summary>
    /// <param name="func">Применяемое отображение φ: Fraction ⟼ Fraction</param>
    /// <param name="rows">Список строк, к которым применяется отображение</param>
    /// <returns>Новая матрица с изменёнными столбцами</returns>
    public Matrix SelectRows(Func<Fraction, Fraction> func, params int[] rows)
    {
        var copy = Copy();
        copy.ApplyRows(func, rows);
        return copy;
    }

    /// <summary>
    /// Создает новую матрицу путем применения отображения к выбранной строке текущей матрицы
    /// </summary>
    /// <param name="func">Применяемое отображение φ: Fraction ⟼ Fraction</param>
    /// <param name="row">Индекс строки, к которой применяется отображение</param>
    /// <returns>Новая матрица с изменённой строкой</returns>
    public Matrix SelectRow(Func<Fraction, Fraction> func, int row) => SelectRows(func, row);

    /// <summary>
    /// Создает новую матрицу путем применения отображения к выбранным столбцам текущей матрицы
    /// </summary>
    /// <param name="func">Применяемое отображение φ: Fraction ⟼ Fraction</param>
    /// <param name="columns">Список столбцов, к которым применяется отображение</param>
    /// <returns>Новая матрица с изменёнными столбцами</returns>
    public Matrix SelectColumns(Func<Fraction, Fraction> func, params int[] columns)
    {
        var copy = Copy();
        copy.ApplyColumns(func, columns);
        return copy;
    }

    /// <summary>
    /// Создает новую матрицу путем применения отображения к выбранному столбцу текущей матрицы
    /// </summary>
    /// <param name="func">Применяемое отображение φ: Fraction ⟼ Fraction</param>
    /// <param name="column">Индекс столбца, к которому применяется отображение</param>
    /// <returns>Новая матрица с изменённым столбцом</returns>
    public Matrix SelectColumn(Func<Fraction, Fraction> func, int column) => SelectColumns(func, column);

    /// <summary>
    /// Применяет отображение ко всем элементам матрицы (изменяет текущую матрицу)
    /// </summary>
    /// <param name="func">Применяемое отображение φ: Fraction ⟼ Fraction</param>
    public void Apply(Func<Fraction, Fraction> func)
    {
        for (int i = 0; i < Rows; ++i)
        for (int j = 0; j < Columns; ++j)
            _data[i][j] = func(_data[i][j]);
    }

    /// <summary>
    /// Применяет отображение к элементам матрицы, удовлетворяющим булевой маске (изменяет текущую матрицу)
    /// </summary>
    /// <param name="func">Применяемое отображение φ: Fraction ⟼ Fraction</param>
    /// <param name="filter">Булева матрица-маска, соответствующая тем элементам, к которым требуется применить отображение</param>
    /// <exception cref="ArgumentException">Исключение, возникающее при попытке передать в качестве булевой маски матрицу с размерами, не соответствующими размерам данной матрицы</exception>
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

    /// <summary>
    /// Строит булеву матрицу-маску, соответствующую элементам, удовлетворяющим условию
    /// </summary>
    /// <param name="condition">Предикат φ на элементы матрицы φ: Fraction ⟼ bool</param>
    /// <returns>Матрица-маска из булевых значений</returns>
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

    /// <summary>
    /// Строит булеву матрицу-маску, соответствующую элементам, удовлетворяющим условию на индексы
    /// </summary>
    /// <param name="checkPos">Условие на индексы элементов</param>
    /// <returns>Матрица-маска из булевых значений</returns>
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

    /// <summary>
    /// Применяет отображение к заданному набору строк матрицы (изменяет текущую матрицу)
    /// </summary>
    /// <param name="func">Применяемое отображение φ: Fraction ⟼ Fraction</param>
    /// <param name="rows">Список строк, к которым применяется отображение</param>
    public void ApplyRows(Func<Fraction, Fraction> func, params int[] rows)
    {
        foreach (var i in rows)
            for (int j = 0; j < Columns; ++j)
                _data[i][j] = func(_data[i][j]);
    }

    /// <summary>
    /// Применяет отображение к заданной строке матрицы (изменяет текущую матрицу)
    /// </summary>
    /// <param name="func">Применяемое отображение φ: Fraction ⟼ Fraction</param>
    /// <param name="row">Индекс строки, к которой применяется отображение</param>
    public void ApplyRow(Func<Fraction, Fraction> func, int row) => ApplyRows(func, row);

    /// <summary>
    /// Применяет отображение к заданному набору столбцов матрицы (изменяет текущую матрицу)
    /// </summary>
    /// <param name="func">Применяемое отображение φ: Fraction ⟼ Fraction</param>
    /// <param name="columns">Список столбцов, к которым применяется отображение</param>
    public void ApplyColumns(Func<Fraction, Fraction> func, params int[] columns)
    {
        for (int i = 0; i < Rows; ++i)
            foreach (var j in columns)
                _data[i][j] = func(_data[i][j]);
    }

    /// <summary>
    /// Применяет отображение к заданному столбцу матрицы (изменяет текущую матрицу)
    /// </summary>
    /// <param name="func">Применяемое отображение φ: Fraction ⟼ Fraction</param>
    /// <param name="column">Индекс столбца, к которому применяется отображение</param>
    public void ApplyColumn(Func<Fraction, Fraction> func, int column) => ApplyColumns(func, column);

    /// <summary>
    /// Приводит матрицу к ступенчатому виду
    /// </summary>
    /// <returns>Новая матрица в ступенчатом виде</returns>
    public Matrix Reduce()
    {
        var m = Copy();


        var shift = 0;
        for (int i = 0; i < m.Rows && i < Columns; ++i)
        {
            Fraction inverse;
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

    /// <summary>
    /// Приведение данной матрицы к её каноническому виду
    /// </summary>
    /// <returns>Новая матрица в каноническом виде</returns>
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

            if (m[row, aux] != 1)
            {
                var inverse = 1 / m[row, aux];
                m.ApplyRow(x => x * inverse, row);
            }

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
    /// Вычисляет союзную к текущей матрицу
    /// </summary>
    /// <returns>Матрица, союзная к данной</returns>
    /// <exception cref="Exception">Исключение, возникающее при попытке вычислить союзную матрицу для не квадратной матрицы</exception>
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

    /// <summary>
    /// Осуществляет попытку вычисление матрицы, обратной к данной, записывает обратную матрицу или null (в случае её отсутствия) в выходной параметр invMatrix
    /// </summary>
    /// <param name="invMatrix">выходной параметр - найденная обратная матрица или null</param>
    /// <returns>логическое значение, показывающее, была ли найдена обратная матрица</returns>
    public bool TryInverse(out Matrix? invMatrix)
    {
        try
        {
            invMatrix = Inverse();
            return true;
        }
        catch (ArgumentException e)
        {
            Console.WriteLine(e);
            invMatrix = null;
            return false;
        }
    }

    /// <summary>
    /// Вычисляет матрицу, обратную к данной
    /// </summary>
    /// <returns>Новая матрица - A^(-1)</returns>
    /// <exception cref="ArgumentException">Исключение, возникающее при попытке вычислить обратную матрицу для вырожденной матрицы</exception>
    public Matrix Inverse()
    {
        var det = Det();
        if (det == 0)
            throw new ArgumentException("The matrix is degenerate, thus it has no inverse");
        return (1 / det) * Adj();
    }

    /// <summary>
    /// Вычисляет алгебраическое дополнение элемента [A]ij данной матрицы
    /// </summary>
    /// <param name="i">индекс строки элемента [A]ij</param>
    /// <param name="j">индекс столбца элемента [A]ij</param>
    /// <returns>Алгебраическое дополнение элемента [A]ij</returns>
    /// <exception cref="IndexOutOfRangeException">Исключение, возникающее при попытке вычислить алгебраическое дополнение элемента, не принадлежащего матрице</exception>
    public Fraction A(int i, int j)
    {
        if (i >= Rows || j >= Columns)
            throw new IndexOutOfRangeException("Cannot get such algebraic complement");
        if ((i + j) % 2 == 0)
        {
            return Minor(i, j).Det();
        }

        return (-1) * Minor(i, j).Det();
    }
    
    /// <summary>
    /// Вычисление дополняющего минора для данной матрицы и заданного элемента
    /// </summary>
    /// <param name="i">Индекс исключаемой строки</param>
    /// <param name="j">Индекс исключаемого столбца</param>
    /// <returns>Матрица - дополняющий минор элемента [A]ij данной матрицы</returns>
    /// <exception cref="IndexOutOfRangeException">Исключение, возникающее при попытке вычисления дополняющего минора элемента, не принадлежащего матрице</exception>
    public Matrix Minor(int i, int j)
    {
        if (i >= Rows || j >= Columns)
            throw new IndexOutOfRangeException("Cannot get such Minor");
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
    
    /// <summary>
    /// Вычисление определителя данной матрицы
    /// </summary>
    /// <returns>Рациональное число - определитель данной матрицы</returns>
    /// <exception cref="Exception">Исключение, возникающее при попытке вычисления определителя для неквадратной матрицы</exception>
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

        var sum = new Fraction();
        //  разложение по 1-ой строке
        for (int j = 0; j < Columns; j++)
        {
            sum += _data[0][j] * A(0, j); // need to check this
        }

        return sum;
    }

    /// <summary>
    /// Вычисление следа матрицы - Tr(A)
    /// </summary>
    /// <returns>Рациональное число - след данной матрицы</returns>
    /// <exception cref="ArgumentException">Исключение, возникающее при попытке вычисления следа для неквадратной матрицы</exception>
    public Fraction Trace()
    {
        if (!IsSquare())
            throw new ArgumentException("Trace is only defined for square matrices");

        var sum = new Fraction();

        for (int i = 0; i < Columns; ++i)
            sum += _data[i][i];
        return sum;
    }

    /// <summary>
    /// Проверка, что матрица квадратная
    /// </summary>
    /// <returns>Логическое значение - является ли матрица квадратной или нет</returns>
    public bool IsSquare() => Rows == Columns;

    /// <summary>
    /// Смена местами двух строк матрицы
    /// </summary>
    /// <param name="row1">Индекс первой строки</param>
    /// <param name="row2">Индекс второй строки</param>
    public void SwapRows(int row1, int row2)
    {
        if (row1 == row2)
            return;
        (_data[row1], _data[row2]) = (_data[row2], _data[row1]);
    }

    /// <summary>
    /// Смена местами двух столбцов матрицы
    /// </summary>
    /// <param name="col1">Индекс первого столбца</param>
    /// <param name="col2">Индекс второго столбца</param>
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
            _data = new Fraction[a.Rows][]
        };
        for (int i = 0; i < a.Rows; i++)
        {
            c._data[i] = new Fraction[b.Columns];
            for (int j = 0; j < b.Columns; j++)
            {
                var s = new Fraction();
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
            throw new ArgumentException("Incorrect dimensions");
        var c = new Matrix
        {
            _data = new Fraction[a.Rows][]
        };
        for (int i = 0; i < c.Rows; i++)
        {
            c._data[i] = new Fraction[a.Columns];
            for (int j = 0; j < c.Columns; j++)
                c._data[i][j] = a._data[i][j] + b._data[i][j];
        }

        c._maxS = c.GetMaxLength();
        return c;
    }
    
    public static Matrix operator -(Matrix matrix) => matrix.Select(fraction => -fraction);

    public static Matrix operator -(Matrix a, Matrix b) => a + -b;

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

    /// <summary>
    /// Прибавление одной строки матрицы, умноженной на коэффициент, к другой
    /// </summary>
    /// <param name="i">Индекс строки, к которой прибавляется другая строка</param>
    /// <param name="j">Индекс прибавляемой строки</param>
    /// <param name="alpha">Коэффициент, но который умножается прибавляемая строка</param>
    public void AddToRow(int i, int j, Fraction alpha)
    {
        for (int k = 0; k < Columns; k++)
        {
            _data[i][k] += _data[j][k] * alpha;
        }

        _maxS = GetMaxLength();
    }
    
    /// <summary>
    /// Прибавление одной строки матрицы к другой
    /// </summary>
    /// <param name="i">Индекс строки, к которой прибавляется другая строка</param>
    /// <param name="j">Индекс прибавляемой строки</param>
    public void AddToRow(int i, int j) => AddToRow(i, j, new Fraction(1));

    // Возможно стоит как-нибудь перегрузить Equals() и GetHashCode()
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

    /// <summary>
    /// Создает строковое представление матрицы
    /// </summary>
    /// <returns>Строковое представление данной матрицы</returns>
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

    /// <summary>
    /// Проверяет, что все элементы матрицы равны переданному элементу
    /// </summary>
    /// <param name="element"> Элемент для сравнения</param>
    /// <returns> True, если все элементы матрицы равны element. False в обратном случае</returns>
    public bool All(int element)
    {
        for (int i = 0; i < Rows; i++)
        {
            for (int j = 0; j < Columns; j++)
            {
                if (_data[i][j] != element)
                {
                    return false;
                }
            }
        }

        return true;
    }

    /// <summary>
    /// Единичная матрица заданного размера
    /// </summary>
    /// <param name="n">Размер единичной матрицы</param>
    /// <returns>Единичная матрица</returns>
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
                    a[p][l] = new Fraction();
        }

        return new Matrix(a);
    }

    /// <summary>
    /// Матричная единица Eij
    /// </summary>
    /// <param name="n">Размер матрицы</param>
    /// <param name="i">Строка единичного элемента</param>
    /// <param name="j">Столбец единичного элемента</param>
    /// <returns>Матричная единица Eij</returns>
    /// <exception cref="IndexOutOfRangeException">Исключение, возникающее при попытке создать матричную единицу, которая не принадлежит матрице n x n</exception>
    public static Matrix Eij(int n, int i, int j)
    {
        if (i >= n || j >= n)
            throw new IndexOutOfRangeException();
        var a = new Fraction[n][];
        for (int p = 0; p < n; p++)
        {
            a[p] = new Fraction[n];
            for (int l = 0; l < n; l++)
                a[p][l] = new Fraction();
        }

        a[i][j] = new Fraction(1);
        return new Matrix(a);
    }
    
    /// <summary>
    /// Матрица элементарного преобразования, соответствующая замене строк местами
    /// </summary>
    /// <param name="n">Размер матрицы</param>
    /// <param name="i">Индекс первой строки</param>
    /// <param name="j">Индекс второй строки</param>
    /// <returns>Матрица элементарного преобразования</returns>
    public static Matrix ESwap(int n, int i, int j)
    {
        var m = E(n);
        m._data[i][i] = new Fraction();
        m._data[i][j] = new Fraction(1);

        m._data[j][j] = new Fraction();
        m._data[j][i] = new Fraction(1);
        return m;
    }

    /// <summary>
    /// Матрица элементарного преобразования, соответствующая умножению заданной строки на коэффициент
    /// </summary>
    /// <param name="n">Размер матрицы</param>
    /// <param name="i">Индекс изменяемой строки</param>
    /// <param name="f">Коэффициент, на который умножается строка</param>
    /// <returns>Матрица элементарного преобразования</returns>
    /// <exception cref="ArgumentException">Исключение, возникающее при попытке создать матрицы элементарного преобразования для умножения строки на 0</exception>
    public static Matrix EMultRow(int n, int i, Fraction f)
    {
        var m = E(n);
        m._data[i][i] = f == 0 ? throw new ArgumentException("Multiplying a row by 0 is not an elementary row operation") : f;
        return m;
    }
    
    /// <summary>
    /// Матрица элементарного преобразования, соответствующая прибавлению строки, умноженной на константу к другой строке 
    /// </summary>
    /// <param name="n">Размер матрицы</param>
    /// <param name="i">Индекс первой строки (той, к которой прибавляют)</param>
    /// <param name="j">Индекс второй строки (той, которую прибавляют)</param>
    /// <param name="f">Коэффициент, на который умножается вторая строка</param>
    /// <returns>Матрица элементарного преобразования</returns>
    public static Matrix EAddRow(int n, int i, int j, Fraction f)
    {
        var m = E(n);
        m._data[i][j] = f;
        return m;
    }
    
    /// <summary>
    /// Создает случайную матрицу заданного размера, где элементы являются целыми числами, лежащими в заданном диапазоне
    /// </summary>
    /// <param name="rows">Количество строк в создаваемой матрице</param>
    /// <param name="columns">Количество столбцов в создаваемой матрице</param>
    /// <param name="intervalStart">Минимальное возможное значение для целого числа (включительно)</param>
    /// <param name="intervalEnd">Максимальное возможное значение для целого числа (включительно)</param>
    /// <returns></returns>
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
    
    /// <summary>
    /// Метод, осуществляющий выбор столбцов матрицы по индексу
    /// </summary>
    /// <param name="f">Предикат φ: int ⟼ bool для выбора столбцов матрицы по индексу</param>
    /// <returns>Новая матрица из выбранных столбцов</returns>
    public Matrix TakeColumns(Predicate<int> f)
    {
        List<List<Fraction>> rows = new();


        for (int i = 0; i < Rows; ++i)
        {
            rows.Add( new List<Fraction>());
            for (int j = 0; j < Columns; ++j)
            {
                if (f(j))
                    rows[i].Add(_data[i][j]);
            }
        }

        return new Matrix(rows);
    }
    
    /// <summary>
    /// Создает случайную матрицу заданного размера, где элементы являются дробями, лежащими в заданном диапазоне, знаменатель которых не превосходит заданного числа (он является натуральным числом)
    /// </summary>
    /// <param name="rows">Количество строк в создаваемой матрице</param>
    /// <param name="columns">Количество столбцов в создаваемой матрице</param>
    /// <param name="intervalStart">Минимальное возможное значение для дроби (включительно)</param>
    /// <param name="intervalEnd">Максимальное возможное значение для дроби (исключительно)</param>
    /// <param name="maxDenominator">Максимальное возможное значение для знаменателя дроби (включительно). Знаменатель является числом от 1 до N</param>
    /// <returns>Матрица из случайных дробных значений</returns>
    public static Matrix GetRandRationalMatrix(int rows, int columns, long intervalStart, long intervalEnd,
        long maxDenominator)
    {
        var rand = new Random();
        var arr = new Fraction[rows][];
        for (int i = 0; i < rows; i++)
        {
            arr[i] = new Fraction[columns];
            for (int j = 0; j < columns; j++)
            {
                arr[i][j] = new Fraction(rand.NextInt64(intervalStart, intervalEnd + 1),
                    rand.NextInt64(1, maxDenominator + 1));
            }
        }

        return new Matrix(arr);
    }

    /// <summary>
    /// Проверка на то, является ли зубчатый массив из дробей корректным представлением прямоугольной матрицы
    /// </summary>
    /// <param name="f">Зубчатый массив из дробей для проверки</param>
    /// <returns>Логическое значение, показывающее является ли переданный аргумент корректным представлением матрицы</returns>
    private bool IsValidMatrixRepresentation(Fraction[][] f)
    {
        int firstRowLength = f[0].Length;
        for(int i = 1; i < f.Length; ++i)
            if (f[i].Length != firstRowLength)
                return false;
        return true;
    }

    public List<Vector> FSR()
    {
        List<Vector> fsr = new();
        Matrix m = Canonical();
        
        int n = 0;
        for (int i = 1; i < m.Rows; ++i)
        {
            for (int j = n + 1; j < m.Columns; ++j)
            {
                if (m[i, j] != 1)
                {
                    Vector vec = new Vector(m.Columns, 0);
                    for (int k = 0; k < i; ++k)
                    {
                        vec[k] = -m[k, j];
                    }

                    vec[j] = 1;
                    fsr.Add(vec);
                }
                else
                {
                    n = j;
                    break;
                }
            }
        }

        return fsr;
    }
    
    /// <summary>
    /// Проверка на то, является ли список из списков дробей корректным представлением прямоугольной матрицы
    /// </summary>
    /// <param name="f">Список из списков дробей для проверки</param>
    /// <returns>Логическое значение, показывающее является ли переданный аргумент корректным представлением матрицы</returns>
    private bool IsValidMatrixRepresentation(List<List<Fraction>> f)
    {
        int firstRowLength = f[0].Count;
        for(int i = 1; i < f.Count; ++i)
            if (f[i].Count != firstRowLength)
                return false;
        return true;
    }
    
    //  Decompositions

    /// <summary>
    /// Находит QR-разложение данной матрицы
    /// </summary>
    /// <returns>Пара матриц:  Q - ортогональная матрица; R - верхнетреугольная матрица</returns>
    public (Matrix Q, Matrix R) QR(bool print = false)
    {
        if (Det() == 0)
            throw new ArgumentException("Cannot perform a QR decomposition on a degenerate matrix");
        //var E = new EuclideanSpace(GetVectors());
        var E = new EuclideanSpace(this);
        var Q = E.Orthogonalize(print).MatrixBasis;
        for (int i = 0; i < Q.Columns; ++i)
        {
            Fraction sqrSum = new();
            for (int j = 0; j < Q.Rows; ++j)
                sqrSum += Q[j, i] * Q[j, i];
            Fraction aux1 = Fraction.Inverse(new(1, 1, sqrSum.Numerator));
            Fraction aux2 = new(1, 1, sqrSum.Denominator);

            Fraction inverse = aux1 * aux2;
            Q.ApplyColumn(x => x * inverse, i);
            if (print)
            {
                Console.WriteLine($"q{i} = {inverse} * {Q.GetColumn(i)}");
            }
        }

        var R = Q.Transpose() * this;

        if (print)
        {
            Console.WriteLine(R);
        }
        return (Q, R);
    }
}