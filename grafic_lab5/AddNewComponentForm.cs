using grafic_lab5.ImageAnalizer;
using grafic_lab5.ImagesData;

namespace grafic_lab5;

/// <summary>
/// Добавить новые компоненты к уже известным
/// </summary>
public partial class AddNewComponentForm : Form
{
    /// <summary>
    /// Набор новых образов
    /// </summary>
    private List<ResultNewComponentItem> _newComponentItems;

    /// <summary>
    /// Набор Найденных образов, куда добавляются новые образы
    /// </summary>
    public List<ResultFindedItem> FindedItems;
    /// <summary>
    /// Набор Найденных образов, куда добавляются новые образы
    /// </summary>
    public List<ComponentData> FindedComponents;

    /// <summary>
    /// Изображение
    /// </summary>
    private Bitmap _bitmap;

    /// <summary>
    /// Индекс с помощью которого проходимся по _newComponentItems
    /// </summary>
    private int _index = 0;

    public AddNewComponentForm(List<ResultNewComponentItem> newComponentItems, Bitmap bitmap)
    {
        InitializeComponent();

        _newComponentItems = newComponentItems;
        _bitmap = bitmap;
        FindedItems = new List<ResultFindedItem>();
        FindedComponents = new List<ComponentData>();

        SetComponentImage();
    }

    /// <summary>
    /// Добавить образ
    /// </summary>
    private void add_Click(object sender, EventArgs e)
    {
        MetaData metaData = new MetaData(textBox1.Text);

        // для отображения обнуружения образа
        FindedItems.Add(new ResultFindedItem(metaData,
            _newComponentItems[_index].Location));

        // для добавления нового образа
        FindedComponents.Add(new ComponentData(metaData,
            _newComponentItems[_index].Hash));

        // перейти к добавлению следующего образа
        NextNewComponent();
    }

    /// <summary>
    /// Пропустить добавление образа
    /// </summary>
    private void cancel_Click(object sender, EventArgs e)
    {
        NextNewComponent();
    }

    /// <summary>
    /// Взять следующий сомпонент
    /// </summary>
    private void NextNewComponent()
    {
        ++_index;

        if (_index >= _newComponentItems.Count)
        {
            // все компоненты были рассмотрены
            this.Close();
        }
        else
        {
            // Обнулить текст
            textBox1.Text = string.Empty;

            SetComponentImage();
        }
    }

    /// <summary>
    /// Установить изображение
    /// </summary>
    private void SetComponentImage()
    {
        // Добавление картинки
        using Bitmap bmp = new Bitmap(_bitmap);
        
        var newImg = bmp.Clone(
            _newComponentItems[_index].Location,
            bmp.PixelFormat);
        pictureBox1.Image = newImg;
    }

    /// <summary>
    /// При закртытии формы, нужно перейти на главное окно
    /// </summary>
    private void AddNewComponentForm_FormClosed(object sender, FormClosedEventArgs e)
    {
        Form ifrm = Application.OpenForms[0];
        ifrm.Show();
    }
}
