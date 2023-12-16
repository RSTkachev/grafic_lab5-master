using grafic_lab5.ImageFilter;
using grafic_lab5.Images;
using grafic_lab5.ImagesData;

namespace grafic_lab5.Storages;

/// <summary>
/// Хранилище образов
/// </summary>
public class ComponentStorage
{
    /// <summary>
    /// Массив компонент
    /// </summary>
    private List<ComponentData> _components = new List<ComponentData>();

    /// <summary>
    /// Добавить образ
    /// </summary>
    /// <param name="component">образ</param>
    public void AddComponent(ComponentData component)
    {
        _components.Add(component);
    }

    /// <summary>
    /// Добавить образ
    /// </summary>
    /// <param name="components">список образов</param>
    public void AddComponent(List<ComponentData> components)
    {
        components.AddRange(components);
    }

    /// <summary>
    /// Найти наиболее подходящий образ
    /// </summary>
    /// <param name="hash">хэш изображения, которому нужно подобрать образ</param>
    /// <returns></returns>
    public ComponentData? FindCloserComponent(ulong hash)
    {
        //int min_i = -1;
        //ulong minDistances = ulong.MaxValue;

        //for (int i = 0; i < _components.Count; i++)
        //{
        //    var currDistances = PerceptualHash.HammingDistances(_components[i].Hash, hash);

        //    if (currDistances < minDistances)
        //    {
        //        minDistances = currDistances;
        //        min_i = i;
        //    }
        //}

        //return min_i != -1 ? _components[min_i] : null;
        return _components.MinBy((component) => PerceptualHash.HammingDistances(component.Hash, hash));
    }

    /// <summary>
    /// Заполнить базу образов из файла
    /// </summary>
    /// <param name="path">путь к списку образов</param>
    public void FillComponentStorage(string path, ImagePreparer imagePreparer)
    {
        string[] ComponentNameAndImage = File.ReadAllLines(path);

        for (int i = 0; i < ComponentNameAndImage.Length; i++)
        {
            try
            {
                string[] nameAndImagePath = ComponentNameAndImage[i].Trim().Split();
                BinaryImage toAdd = imagePreparer.PrepareForAnalysis(new Bitmap(nameAndImagePath[2]), nameAndImagePath[1] == "1");

                AddComponent(new MetaData(nameAndImagePath[0]), toAdd);
            }
            catch (Exception) { }
        }
    }

    /// <summary>
    /// Добавление образа - считается, что он один
    /// </summary>
    /// <param name="metaData">Методанные</param>
    /// <param name="toAdd">Изображение с образом</param>
    private void AddComponent(MetaData metaData, BinaryImage toAdd)
    {
        // нахождение карты компонент связности
        ComponentMap componentMap = ComponentMap.Create(toAdd);
        // нахождение компонент связности по карте
        var components = ComponentDeterminator.FindComponents(componentMap);

        if (components.Count == 1)
        {
            BinaryImage componentToAdd = componentMap.ClipComponent(components[0].Item1, components[0].Item2);

            AddComponent(new ComponentData(metaData, componentToAdd));
        }
    }

}