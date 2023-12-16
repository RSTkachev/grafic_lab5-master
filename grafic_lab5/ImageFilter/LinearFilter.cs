using grafic_lab5.Images;

namespace grafic_lab5.ImageFilter;

/// <summary>
/// Класс линейной фильтрации
/// </summary>
public class LinearFilter
{
    /// <summary>
    /// матрица преобразований
    /// </summary>
    private readonly int[,] _matrix;
    /// <summary>
    /// коэффициент на который можно вынести из матрицы
    /// </summary>
    private readonly double _coefficient;

    /* Предполагаеться чтение из файла
    _matrix = {
        {1, 1, 1},
        {1, 2, 1},
        {1, 1, 1}
    }

     _coefficient = 0.1;
     */

    /// <summary>
    /// текущее положение фильтра по координате Y
    /// </summary>
    int _matrixCenterY;
    /// <summary>
    /// текущее положение фильтра по координате X
    /// </summary>
    int _matrixCenterX;

    public LinearFilter(int[,] matrix, double coefficient)
    {
        _matrix = matrix;
        _coefficient = coefficient;

        _matrixCenterY = _matrix.GetLength(0) / 2;
        _matrixCenterX = _matrix.GetLength(1) / 2;
    }

    /// <summary>
    /// Фильтрация изображения
    /// </summary>
    /// <param name="image">Исходное изображение</param>
    /// <returns>Фильтрованное изображение</returns>
    public GrayImage Filter(GrayImage image, bool isBlackBackground)
    {
        GrayImage res = new GrayImage(image.Width, image.Height);

        // надо закончить раньше, чем окно выйдет за пределы (перемещаеться центр фильтрационного окна)
        int height = image.Height - _matrixCenterY;
        int width = image.Width - _matrixCenterX;

        for (int y = _matrixCenterY; y < height; y++)
        {
            for (int x = _matrixCenterX; x < width; x++)
            {
                res.SetPixel(x, y, Filter(x, y, image));
            }
        }

        if (!isBlackBackground)
        {
            // надо сделать белую рамку, так как если
            // фон светлый, 0 пиксели воспринимаються, как образ

            // верхние полосы
            for (int y = 0; y < _matrixCenterY; y++)
            {
                for (int x = 0; x < image.Width; x++)
                {
                    res.SetPixel(x, y, byte.MaxValue);

                    res.SetPixel(x, image.Height - y - 1, byte.MaxValue);
                }
            }

            // полосы по бокам
            for (int y = _matrixCenterY; y < height; y++)
            {
                for (int x = 0; x < _matrixCenterX; x++)
                {
                    res.SetPixel(x, y, byte.MaxValue);

                    res.SetPixel(image.Width - 1 - x, y, byte.MaxValue);
                }
            }
        }

        return res;
    }

    /// <summary>
    /// Отклик фильтра
    /// </summary>
    /// <param name="x">номер столбца</param>
    /// <param name="y">номер строки</param>
    /// <param name="image">исходное изображение</param>
    /// <returns>отклик</returns>
    private byte Filter(int x, int y, GrayImage image)
    {
        double result = 0;

        // вычисляется сумма произведений соответствуюхих позиций фильтрационного окна
        // и пикселей находящихся под этим окном
        for (int p = 0; p < _matrix.GetLength(0); p++)
        {
            for (int q = 0; q < _matrix.GetLength(1); q++)
            {
                var i = y - _matrixCenterY + p;
                var j = x - _matrixCenterX + q;

                result += image.GetPixel(j, i) * _matrix[p, q];
            }
        }

        // умножение результата на коэффициент
        return (byte)(result * _coefficient);
    }
}

/// <summary>
/// Парсер фильтрационного окна
/// </summary>
public class MatrixParser
{
    /// <summary>
    /// матрица преобразований
    /// </summary>
    public readonly int[,] Matrix;
    /// <summary>
    /// коэффициент на который можно вынести из матрицы
    /// </summary>
    public readonly double Coefficient;

    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="path">Путь к файлу</param>
    public MatrixParser(string path)
    {
        string[] lines = File.ReadAllLines(path);

        Coefficient = double.Parse(lines[0]);

        // прочитали первую линию
        var tempLine = Array.ConvertAll(lines[1].Trim().Split(' '), Convert.ToInt32);

        // надо учесть наличие коэффициента
        int h = lines.Length - 1;
        int w = tempLine.Length;

        Matrix = new int[h, w];

        for (int j = 0; j < w; j++)
        {
            Matrix[0, j] = tempLine[j];
        }

        for (int i = 1; i < h; i++)
        {
            // не забываем, что в первой строке коэффициент
            tempLine = Array.ConvertAll(lines[i + 1].Trim().Split(' '), Convert.ToInt32);

            for (int j = 0; j < w; j++)
            {
                Matrix[i, j] = tempLine[j];
            }
        }
    }
}
