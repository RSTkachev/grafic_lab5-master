using grafic_lab5.Images;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace grafic_lab5.ImagesData;

/// <summary>
/// Карта связных областей
/// хранит связные области на бинарном изображении
/// </summary>
public class ComponentMap : BaseImage<int>
{
    /// <summary>
    /// Количество компонентов связности
    /// </summary>
    public int MaxComponentCount { get; private set; }

    /// <summary>
    /// Конструктор инициализирует индексом 1 все пиксили, где Bit.one
    /// </summary>
    /// <param name="image">Исходное изображение</param>
    private ComponentMap(BinaryImage image) : base(image.Width, image.Height)
    {
        for (int y = 0; y < image.Height; y++)
        {
            for (int x = 0; x < image.Width; x++)
            {
                if (image.GetPixel(x, y) == Bit.one)
                {
                    SetPixel(x, y, 1);
                }
            }
        }
    }

    /// <summary>
    /// Замена всех старых индексов новым
    /// </summary>
    /// <param name="oldIndex"></param>
    /// <param name="newIndex"></param>
    public void ReplaseIndex(int oldIndex, int newIndex)
    {
        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                if (GetPixel(x, y) == oldIndex)
                {
                    SetPixel(x, y, newIndex);
                }
            }
        }
    }

    /// <summary>
    /// Создание карты компонентов связнасти по бинарному изображению
    /// Алгоритм ABC
    /// </summary>
    /// <param name="image"></param>
    /// <returns>карту компонентов связнасти</returns>
    public static ComponentMap Create(BinaryImage image)
    {
        ComponentMap result = new ComponentMap(image);
        int componentCounter = 0;

        for (int y = 0; y < result.Height; y++)
        {
            for (int x = 0; x < result.Width; x++)
            {
                int a = result.GetPixel(x, y);
                if (a == 0)
                    continue;

                int b = 0, c = 0;

                int temp_j = x - 1;

                if (temp_j > 0)
                {
                    b = result.GetPixel(temp_j, y);
                }

                int temp_i = y - 1;

                if (temp_i > 0)
                {
                    c = result.GetPixel(x, temp_i);
                }

                if (b == 0 && c == 0)
                {
                    ++componentCounter;

                    result.SetPixel(x, y, componentCounter);
                }
                else
                {
                    if (b != 0 && c == 0)
                    {
                        result.SetPixel(x, y, b);
                    }
                    else if (b == 0 && c != 0)
                    {
                        result.SetPixel(x, y, c);
                    }
                    else if (b == c)
                    {
                        result.SetPixel(x, y, b);
                    }
                    else
                    {
                        result.SetPixel(x, y, c);
                        result.ReplaseIndex(b, c);
                    }
                }
            }
        }

        result.MaxComponentCount = componentCounter;

        return result;
    }

    /// <summary>
    /// Выделение прямоугольником соответствующей компоненты связности
    /// </summary>
    /// <param name="region"></param>
    /// <param name="componentIndex"></param>
    /// <returns></returns>
    public BinaryImage ClipComponent(Rectangle region, int componentIndex)
    {
        int maxY = region.Bottom;
        int maxX = region.Right;

        int minY = region.Y;
        int minX = region.X;

        BinaryImage result = new BinaryImage(region.Width, region.Height);

        int y = minY;

        while (y < maxY)
        {
            int x = minX;

            while (x < maxX)
            {
                if (GetPixel(x, y).Equals(componentIndex))
                {
                    result.SetPixel(x - minX, y - minY, Bit.one);
                }

                ++x;
            }

            ++y;
        }

        return result;
    }

}
