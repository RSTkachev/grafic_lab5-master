using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace grafic_lab5.Images;

/// <summary>
/// Серое изображение (полутоновое)
/// </summary>
public class GrayImage : BaseImage<byte>
{
    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="width">ширина</param>
    /// <param name="height">высота</param>
    public GrayImage(int width, int height) : base(width, height)
    {
    }

    /// <summary>
    /// Создать полутоновое изображение из цветного
    /// </summary>
    /// <param name="bitmap">Цветное изображение</param>
    /// <returns>Полутоновое изображение</returns>
    public static GrayImage Create(Bitmap bitmap)
    {
        GrayImage result = new GrayImage(bitmap.Width, bitmap.Height);

        for (int y = 0; y < bitmap.Height; ++y)
        {
            for (int x = 0; x < bitmap.Width; ++x)
            {
                // преобразование соответствующих пикселей в серый цвет
                result.SetPixel(x, y, ToGray(bitmap.GetPixel(x, y)));
            }
        }

        return result;
    }

    /// <summary>
    /// Преобразование к серому цвету
    /// </summary>
    /// <param name="color">RGB pixel</param>
    private static byte ToGray(Color color)
    {
        return (byte)((color.R + color.G + color.B) / 3.0);
        // return (byte)(0.299 * color.R + 0.587 * color.G + 0.114 * color.B);
    }
}
