namespace grafic_lab5.Images;

/// <summary>
/// перечисление либо ноль, либо 1
/// </summary>
public enum Bit : byte
{
    zero,
    one
}

/// <summary>
/// Бинарное изображение
/// </summary>
public class BinaryImage : BaseImage<Bit>
{
    public BinaryImage(int width, int height) : base(width, height)
    {
    }

    public BinaryImage(Bit[,] pixels) : base(pixels)
    {
    }

    /// <summary>
    /// Функция для создания бинарного изображения
    /// </summary>
    /// <param name="image">Изображение по которому строиться</param>
    /// <param name="binarizationBarrier">Число на основе которого применяеться решение будет 0 или 1</param>
    /// <param name="isBlackBackground">Чёрный ли фон</param>
    /// <returns>Бинарного изображения</returns>
    public static BinaryImage Create(GrayImage image, double binarizationBarrier, bool isBlackBackground)
    {
        BinaryImage result = new BinaryImage(image.Width, image.Height);

        Func<double, double, bool> comporator = isBlackBackground ? ((double a, double b) => a >= b) : ((double a, double b) => a <= b);

        for (int y = 0; y < image.Height; ++y)
        {
            for (int x = 0; x < image.Width; ++x)
            {
                // сравнение с пороговым значением
                if (comporator(image.GetPixel(x, y), binarizationBarrier))
                {
                    result.SetPixel(x, y, Bit.one);
                }
                else
                {
                    result.SetPixel(x, y, Bit.zero);
                }
            }
        }

        return result;
    }

    /// <summary>
    /// Вычисление барьерного значения
    /// </summary>
    /// <param name="image">Анализируемое изображения</param>
    public static double CuclBinarizationBarrier(GrayImage image)
    {
        double sum = 0;

        for (int y = 0; y < image.Height; ++y)
        {
            for (int x = 0; x < image.Width; ++x)
            {
                sum += image.GetPixel(x, y);
            }
        }

        // по сути среднее значение всех пикселей
        return sum / (image.Height * image.Width);
    }
}
