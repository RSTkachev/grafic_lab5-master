using grafic_lab5.ImageAnalizer;
using grafic_lab5.ImageFilter;
using grafic_lab5.Storages;

namespace grafic_lab5;

/// <summary>
/// Основная форма
/// </summary>
public partial class Form1 : Form
{
    /// <summary>
    /// Анализатор образов
    /// </summary>
    private ImageAnalyzer _analyzer;
    /// <summary>
    /// Хранилище образов
    /// </summary>
    private ComponentStorage _storage;
    /// <summary>
    /// Предобработчик изображения
    /// </summary>
    private ImagePreparer _imagePreparer;

    /// <summary>
    /// Изображение, которое анализируется
    /// </summary>
    private Bitmap _bitmap;

    public Form1()
    {
        InitializeComponent();

        _imagePreparer = new ImagePreparer();

        _storage = new ComponentStorage();
        _storage.FillComponentStorage("..\\..\\..\\Storages\\сomponents.txt", _imagePreparer);
        _analyzer = new ImageAnalyzer(_storage);

        openFileDialog1.Filter = $"Bitmap files (*.bmp)|*.bmp";

        _imagePreparer.IsInteractive = true;
        _bitmap = new Bitmap(10, 10);
    }

    /// <summary>
    /// Открыть изображение для анализа
    /// </summary>
    private void OpenClick(object sender, EventArgs e)
    {
        // отмена
        if (openFileDialog1.ShowDialog() == DialogResult.Cancel)
            return;

        // получаем выбранный файл
        string filename = openFileDialog1.FileName;

        // установка изображение
        _bitmap = new Bitmap(filename);
        pictureBox1.Image = _bitmap;
    }

    /// <summary>
    /// Найти образы на открытом изображении
    /// </summary>
    private void FindClick(object sender, EventArgs e)
    {
        textBox1.Clear();
        // установка линейного фильтра
        MatrixParser parser = new MatrixParser("..\\..\\..\\ImageFilter\\filter.txt");
        _imagePreparer.SetLinearFilter(new LinearFilter(parser.Matrix, parser.Coefficient));

        var toAnalysis = _imagePreparer.PrepareForAnalysis(_bitmap, comboBox1.SelectedIndex == 1);

        // Распознавание образов, определённых и не найденных 
        var results = _analyzer.AnalyzeImage(toAnalysis);

        var finded = results.ResultFindedItems;
        /*
        // Если есть не расспознанные образы
        if (results.ResultNewComponentItems.Any())
        {
            // Спросить пользователя о добавлении новых образов
            var result = MessageBox.Show(
                "Программа не смогла распознать некоторые образы",
                "Вы хотите их добавить?", MessageBoxButtons.YesNo);

            if (result == DialogResult.Yes)
            {
                // Открытие формы добавления образов
                AddNewComponentForm addForm = new AddNewComponentForm(results.ResultNewComponentItems, _bitmap);
                addForm.ShowDialog();

                // добавление образов
                _storage.AddComponent(addForm.FindedComponents);

                // добавление в результат новых образов
                finded.AddRange(addForm.FindedItems);
            }
        }
        */

        // Отсортировать слева направо и сверху вниз
        finded.Sort();

        pictureBox1.Image = new Bitmap(_bitmap);

        using Graphics graphics = Graphics.FromImage(pictureBox1.Image);

        for (int i = 0; i < finded.Count; i++)
        {
            // выделение прямоугольником
            using Pen pen = new Pen(RGBcolorCreator.GetRandomColor(), 4);
            graphics.DrawRectangle(pen, finded[i].Location);

            // Добавление значения
            string text = finded[i].MetaData.Name + ": " + finded[i].percent.ToString();
            textBox1.AppendText(text);
            textBox1.AppendText(Environment.NewLine);
        }
    }

    /// <summary>
    /// Установить степень юинаризации
    /// </summary>
    private void trackBar1_Scroll(object sender, EventArgs e)
    {
        _imagePreparer.BinarizationMeasure = trackBar1.Value / 10.0;
    }
}

/// <summary>
/// Генератор цветов
/// </summary>
public static class RGBcolorCreator
{
    private static Random _random = new Random();

    public static Color GetRandomColor()
    {
        byte red = (byte)_random.Next(1, 255);
        byte green = (byte)_random.Next(1, 255);
        byte blue = (byte)_random.Next(1, 255);

        return Color.FromArgb(255, red, green, blue);
    }
}