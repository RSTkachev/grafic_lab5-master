using grafic_lab5.Images;

namespace grafic_lab5.ImagesData;

/// <summary>
/// Метаданные об объекте
/// </summary>
public class MetaData
{
    /// <summary>
    /// Название образа
    /// </summary>
    public string Name;

    public MetaData(string name)
    {
        Name = name;
    }

    public MetaData()
    {
        Name = string.Empty;
    }
}

/// <summary>
/// Расчитыватель перцептивного хэша
/// </summary>
public class PerceptualHash
{
    /// <summary>
    /// Вычисление перцептивного хэша
    /// </summary>
    /// <param name="binaryImage">изображение для которого считается хэш</param>
    /// <returns>перцептивный хэш</returns>
    public static ulong CalcPerceptualHash(BinaryImage binaryImage)
    {
        UInt64 perceptualHash = 0;

        // масштабирующее значение по осям координат
        double scale = Math.Min(8.0 / binaryImage.Height, 8.0 / binaryImage.Width);

        // получение масштабируемого размера
        // либо высота либо ширина равна 8, другая характеристика меньше
        int height = (int)Math.Round(binaryImage.Height * scale);
        int width = (int)Math.Round(binaryImage.Width * scale);

        int x = 0, y = 0;

        for (int i = 0; i < height - 1; i++)
        {
            for (int j = 0; j < width; j++)
            {
                // значение в исходном изображении по масштабируемым значениям нового изображения
                x = Math.Min((int)Math.Round(j / scale), binaryImage.Width - 1);
                y = Math.Min((int)Math.Round(i / scale), binaryImage.Height - 1);
                
                perceptualHash |= (byte)binaryImage.GetPixel(x, y);

                perceptualHash <<= 1;
            }

            // если width меньше 8, то недостающие пиксили равны 0
            perceptualHash <<= 8 - width;
        }

        // необходимо развернуть цикл, так как будет переполнение на последем шаге
        for (int j = 0; j < width - 1; j++)
        {
            // значение в исходном изображении по масштабируемым значениям нового изображения
            x = Math.Min((int)Math.Round(j / scale), binaryImage.Width - 1);
            y = Math.Min((int)Math.Round((height - 1) / scale), binaryImage.Height - 1);

            perceptualHash |= (byte)binaryImage.GetPixel(x, y);

            perceptualHash <<= 1;
        }

        // значение в исходном изображении по масштабируемым значениям нового изображения
        x = Math.Min((int)Math.Round((width - 1) / scale), binaryImage.Width - 1);
        y = Math.Min((int)Math.Round((height - 1) / scale), binaryImage.Height - 1);

        perceptualHash |= (byte)binaryImage.GetPixel(x, y);

        // если width меньше 8, то недостающие пиксили равны 0
        perceptualHash <<= 8 - width;

        // если height меньше 8, то недостающие пиксили равны 0
        perceptualHash <<= (8 - height) * 8;

        return perceptualHash;
    }

    /// <summary>
    /// Нахождение расстояния между двумя хэшами
    /// </summary>
    /// <param name="first">первый хэщ</param>
    /// <param name="second">второй хэш</param>
    /// <returns>количество различающихся бит</returns>
    public static byte HammingDistances(ulong first, ulong second)
    {
        byte distances = 0;

        // Операторы ^ устанавливают в 1 только те биты, которые отличаются
        for (ulong val = first ^ second; val > 0; ++distances)
        {
            // Затем мы считаем бит, установленный в 1, используя Peter Wegnerспособ
            val = val & val - 1; // Установить равным нулю значение младшего порядка val 1
        }

        // Возвращает количество различных битов
        return distances;
    }
}

/// <summary>
/// Класс для хранения данных о образе
/// </summary>
public class ComponentData
{
    /// <summary>
    /// Данные об образе
    /// </summary>
    public MetaData Data;
    /// <summary>
    /// Перцептивный хэш
    /// </summary>
    public ulong Hash;

    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="data">Мета-данные</param>
    /// <param name="binaryImage">бинарное изображение</param>
    public ComponentData(MetaData data, BinaryImage binaryImage)
    {
        Data = data;
        Hash = PerceptualHash.CalcPerceptualHash(binaryImage);
    }

    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="data">Мета-данные</param>
    /// <param name="hash">Хэш образа</param>
    public ComponentData(MetaData data, ulong hash)
    {
        Data = data;
        Hash = hash;
    }
}