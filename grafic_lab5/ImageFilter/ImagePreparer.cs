using grafic_lab5.Images;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace grafic_lab5.ImageFilter;

/// <summary>
/// Модуль по подготовке изображения
/// </summary>
public class ImagePreparer
{
    /// <summary>
    /// Барьер бинаризации число от 0 до 255
    /// </summary>
    private double _binarizationBarrier;

    /// <summary>
    /// Степень бинаризации - число от 0 до 1
    /// </summary>
    public double BinarizationMeasure
    {
        set => _binarizationBarrier = value * byte.MaxValue;
        get => _binarizationBarrier / byte.MaxValue;
    }

    /// <summary>
    /// Режим работы с пользователем
    /// </summary>
    public bool IsInteractive { set; get; }

    public ImagePreparer()
    {
        BinarizationMeasure = 0;
        IsInteractive = false;
    }

    /// <summary>
    /// Линейный фильтр
    /// </summary>
    private LinearFilter? _linearFilter;
    
    /// <summary>
    /// Установить линейный фильтр
    /// </summary>
    /// <param name="linearFilter">Линейный фильтр</param>
    public void SetLinearFilter(LinearFilter linearFilter)
    {
        _linearFilter = linearFilter;
    }

    /// <summary>
    /// Подготовка изображения
    /// </summary>
    /// <param name="bitmap">Изображения</param>
    /// <param name="isBlackBackground">Чёрный ли фон</param>
    /// <returns></returns>
    public BinaryImage PrepareForAnalysis(Bitmap old_bitmap, bool isBlackBackground)
    {
        Bitmap bitmap = change_contrast(old_bitmap, 20);

        GrayImage image = GrayImage.Create(bitmap);

        image = min_filter(image, 3);
        
        double binarizationBarrier = Math.Round(BinaryImage.CalcBinarizationBarrier(image));

        if (IsInteractive)
        {
            if (Math.Abs(binarizationBarrier - _binarizationBarrier) >= 10)
            {
                var result = MessageBox.Show(
                    $"Порог бинаризации {Math.Round(binarizationBarrier)} является более предпочтительным, чем {_binarizationBarrier}",
                    "Вы хотите поменять порог бинаризации на рекомендуемый?", MessageBoxButtons.YesNo);

                if (result == DialogResult.No)
                {
                    binarizationBarrier = _binarizationBarrier;
                }
            }
        }

        BinaryImage binaryImage = BinaryImage.Create(image, binarizationBarrier, isBlackBackground);

        binaryImage = MorphologicalFilter.OpeningFilter(binaryImage);

        return binaryImage;
    }


    public static Bitmap change_contrast(Bitmap bitmap, int correction)
    {

        Int32 lAB = 0;
        UInt16 valueR;
        UInt16 valueG;
        UInt16 valueB;

        Int32[] b = new Int32[256];

        for (int y = 0; y < bitmap.Height; ++y)
        {
            for (int x = 0; x < bitmap.Width; ++x)
            {
                Color color = bitmap.GetPixel(x, y);
                valueR = color.R;
                valueG = color.G;
                valueB = color.B;

                lAB += (Int32)(valueR * 0.299 + valueG * 0.587 + valueB * 0.114);
            }
        }

        lAB /= bitmap.Width * bitmap.Height;

        double k = 1.0 + correction / 100.0;


        for (int i = 0; i < 256; i++)
        {
            Int32 delta = (Int32)(i - lAB);
            Int32 temp = (Int32)(lAB + k * delta);

            if (temp < 0)
            {
                temp = 0;
            }
            if (temp >= 255)
            {
                temp = 255;
            }
            b[i] = (Int32)temp;
        }


        for (int y = 0; y < bitmap.Height; ++y)
        {
            for (int x = 0; x < bitmap.Width; ++x)
            {
                Color color = bitmap.GetPixel(x, y);
                valueR = (byte)b[color.R];
                valueG = (byte)b[color.G];
                valueB = (byte)b[color.B];
                bitmap.SetPixel(x, y, Color.FromArgb(valueR, valueG, valueB));
            }
        }
        return bitmap;
    }

    public GrayImage min_filter(GrayImage old_image, int size_of_window)
    {
        byte corner = (byte)(size_of_window / 2);
        int x_max = -1;
        GrayImage new_image = old_image;

        for (int y = 0; y < old_image.Height; ++y)
        {
            for (int x = 0; x < old_image.Width; ++x)
            {
                byte min_color = 255;

                for (int i = -corner; i <= corner; ++i)
                {
                    for (int j = -corner; j <= corner; ++j)
                    {
                        if (x + i >= 0 && x + i < old_image.Width && y + j >= 0 && y + j < old_image.Height)
                        {
                            min_color = Math.Min(min_color, old_image.GetPixel(x + i, y + j));
                        }
                    }
                }
                //new_image.SetPixel(x, y, min_color);
                new_image.SetPixel(x, y, old_image.GetPixel(x, y));

                x_max = Math.Max(x_max, x);
            }

        }

        return new_image;
    }
}
