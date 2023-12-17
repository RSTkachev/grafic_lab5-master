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
    public BinaryImage PrepareForAnalysis(Bitmap bitmap, bool isBlackBackground)
    {
        GrayImage image = GrayImage.Create(bitmap);
        
        if (_linearFilter == null)
        {
            MatrixParser parser = new MatrixParser("..\\..\\..\\ImageFilter\\filter.txt");

            _linearFilter = new LinearFilter(parser.Matrix, parser.Coefficient);
        }

        image = _linearFilter.Filter(image, isBlackBackground);
        
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
}
