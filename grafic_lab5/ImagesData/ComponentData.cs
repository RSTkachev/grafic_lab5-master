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


        double length_step = Math.Max(binaryImage.Height / 64.0, binaryImage.Width / 8.0);


        int x, y;

        if (binaryImage.Width > 0 && binaryImage.Height > 0)
        {
            int width_steps = 0;
            int height_steps = 0;

            while(height_steps * length_step < binaryImage.Height - 1) 
            {
                width_steps = 0;
                while (width_steps * length_step < binaryImage.Width - 1)
                {
                    perceptualHash <<= 1;

                    perceptualHash |= (byte)binaryImage.GetPixel((int)(width_steps * length_step), (int)(height_steps * length_step));
                    width_steps++;
                }
                height_steps++;
            }
        }

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