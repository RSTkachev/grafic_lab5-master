using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace grafic_lab5.Images;

/// <summary>
/// Базовый класс изображения
/// </summary>
/// <typeparam name="T">тип пикселя</typeparam>
public abstract class BaseImage <T> where T : struct
{
    /// <summary>
    /// высота
    /// </summary>
    public int Height { get; private set; }
    /// <summary>
    /// ширина
    /// </summary>
    public int Width { get; private set; }

    /// <summary>
    /// матрица пикселей
    /// </summary>
    private T[,] _pixels;

    /// <summary>
    /// коструктор - все ячейки нулевые
    /// </summary>
    /// <param name="width">ширина</param>
    /// <param name="height">высота</param>
    public BaseImage(int width, int height)
    {
        Width = width;
        Height = height;

        _pixels = new T[height, width];
    }

    /// <summary>
    /// конструктор
    /// </summary>
    /// <param name="pixels">матрица пикселей</param>
    public BaseImage(T[,] pixels)
    {
        _pixels = pixels;

        Height = pixels.GetLength(0);
        Width = pixels.GetLength(1);
    }

    /// <summary>
    /// установить значение пикселю
    /// </summary>
    /// <param name="x">номер столбца</param>
    /// <param name="y">номер строки</param>
    /// <param name="pixel">значение</param>
    public void SetPixel(int x, int y, T pixel)
    {
        _pixels[y, x] = pixel;
    }

    /// <summary>
    /// Получить значение пикселя
    /// </summary>
    /// <param name="x">номер столбца</param>
    /// <param name="y">номер строки</param>
    /// <returns>значение</returns>
    public T GetPixel(int x, int y)
    {
        return _pixels[y, x];
    }
}
