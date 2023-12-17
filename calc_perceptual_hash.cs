// Первый вариант        
		
		UInt64 perceptualHash = 0;

        // шаг по осям координат
        double scale = Math.Min(binaryImage.Height / 8.0, binaryImage.Width / 8.0);

        // получение масштабируемого размера
        // либо высота либо ширина равна 8, другая характеристика меньше
        double height_step = binaryImage.Height * scale;
        double width_step = binaryImage.Width * scale;


        int x, y = 0;

        if (binaryImage.Width > 0 && binaryImage.Height > 0)
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    perceptualHash <<= 1;
                    // значение в исходном изображении по масштабируемым значениям нового изображения
                    x = Math.Min((int)Math.Round((double)j * binaryImage.Width), binaryImage.Width - 1);
                    y = Math.Min((int)Math.Round((double)j * binaryImage.Height), binaryImage.Height - 1);

                    perceptualHash |= (byte)binaryImage.GetPixel(x, y);
                }

            }
        }

        return perceptualHash;


// Второй вариант

        UInt64 perceptualHash = 0;

        int totalArea = binaryImage.Width * binaryImage.Height;
        double pointArea = totalArea / 64.0;
        double length = Math.Sqrt(pointArea);

        if (binaryImage.Width > 0 && binaryImage.Height > 0)
        {
            for (double i = 0.0; (int)i < binaryImage.Height - 1; i += length)
            {
                for (double j = 0.0; (int)j < binaryImage.Width - 1; j += length)
                {
                    perceptualHash <<= 1;
                    perceptualHash |= (byte)binaryImage.GetPixel((int)j, (int)i);
                }
            }
        }

        return perceptualHash;
		