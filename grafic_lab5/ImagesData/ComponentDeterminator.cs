using grafic_lab5.Images;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace grafic_lab5.ImagesData;

/// <summary>
/// Определитель компонент связностей
/// Выделяет компоненты связности по карте
/// </summary>
public class ComponentDeterminator
{
    /// <summary>
    /// Выделение компонент связностей
    /// </summary>
    /// <param name="map"></param>
    /// <returns></returns>
    public static List<(Rectangle, int)> FindComponents(ComponentMap map)
    {
        var res = new List<(Rectangle, int)>();

        // пытаемся найти очередную компоненту
        for (int componentIndex = 1; componentIndex <= map.MaxComponentCount; componentIndex++)
        {
            // рамки этой компоненты
            int maxY = -1;
            int maxX = -1;

            int minY = map.Height + 1;
            int minX = map.Width + 1;

            // было ли хоть что-то найдено
            int counter = 0;

            for (int y = 0; y < map.Height; y++)
            {
                for (int x = 0; x < map.Width; x++)
                {
                    if (map.GetPixel(x, y) == componentIndex)
                    {
                        if (y > maxY)
                        {
                            maxY = y;
                        }

                        if (y < minY)
                        {
                            minY = y;
                        }

                        if (x > maxX)
                        {
                            maxX = x;
                        }

                        if (x < minX)
                        {
                            minX = x;
                        }

                        ++counter;
                    }
                }
            }

            // если границы были изменены, то мы нашли компоненту связности
            if (counter != 0)
            {
                Rectangle rectangle = new Rectangle(minX, minY, maxX - minX, maxY - minY);

                res.Add((rectangle, componentIndex));
            }
        }

        return res;
    }
}
