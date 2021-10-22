using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Threading;

namespace ProSoft.Crypto.ImageSeed
{
    public class Seed
    {
        /// <summary>
        /// Generated a unique seed based on parameters of the passed image
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public string Generate(Config config)
        {
            // initialise seed
            string seed = Environment.TickCount.ToString();

            var pixelsWithHighestRedValue = 0;
            var pixelsWithHighestGreenValue = 0;
            var pixelsWithHighestBlueValue = 0;

            var pixelCount = 0;
            var rgbaTotal = 0;
            var combinedDimensions = 0;
            Bitmap img = new Bitmap(config.ImageLocation);
            combinedDimensions = img.Width;
            combinedDimensions += img.Height;
            Random rnd = new Random();
            float res = 0;


            for (int i = 0; i < img.Width; i++)
            {
                pixelCount++;
                for (int j = 0; j < img.Height; j++)
                {
                    pixelCount++;
                    System.Drawing.Color pixel = img.GetPixel(i, j);
                    rgbaTotal += pixel.A;
                    rgbaTotal += pixel.R;
                    rgbaTotal += pixel.G;
                    rgbaTotal += pixel.B;
                    res = (img.VerticalResolution * img.HorizontalResolution);

                    if ((pixel.R > pixel.G) && (pixel.R > pixel.B))
                        pixelsWithHighestRedValue++;

                    if ((pixel.G > pixel.R) && (pixel.G > pixel.B))
                        pixelsWithHighestGreenValue++;

                    if ((pixel.B > pixel.R) && (pixel.B > pixel.G))
                        pixelsWithHighestBlueValue++;
                }
            }

            var horizontalRandomValue = rnd.Next(0, (int)img.HorizontalResolution);
            var verticalRandomValue = rnd.Next(0, (int)img.VerticalResolution);
            seed += img.GetPixel(horizontalRandomValue, verticalRandomValue).Name;

            Thread.Sleep(new Random().Next(500, 2000));
            horizontalRandomValue = rnd.Next(0, (int)img.HorizontalResolution);
            verticalRandomValue = rnd.Next(0, (int)img.VerticalResolution);
            seed += img.GetPixel(horizontalRandomValue, verticalRandomValue).Name;


            Dictionary<int, string> values = new Dictionary<int, string>();

            var newIndex = new Random().Next(0, ((int)img.VerticalResolution * (int)img.HorizontalResolution) + (img.Height + img.Width));
            values.Add(newIndex, pixelCount.ToString());
            Thread.Sleep(new Random().Next(0, 1000));

            newIndex = new Random().Next(0, ((int)img.VerticalResolution * (int)img.HorizontalResolution) + (img.Height + img.Width));
            values.Add(newIndex, rgbaTotal.ToString());
            Thread.Sleep(new Random().Next(0, 1000));

            newIndex = new Random().Next(0, ((int)img.VerticalResolution * (int)img.HorizontalResolution) + (img.Height + img.Width));
            values.Add(newIndex, combinedDimensions.ToString());
            Thread.Sleep(new Random().Next(0, 1000));

            newIndex = new Random().Next(0, ((int)img.VerticalResolution * (int)img.HorizontalResolution) + (img.Height + img.Width));
            values.Add(newIndex, res.ToString());
            Thread.Sleep(new Random().Next(0, 1000));

            newIndex = new Random().Next(0, ((int)img.VerticalResolution * (int)img.HorizontalResolution) + (img.Height + img.Width));
            values.Add(newIndex, Environment.TickCount.ToString());
            Thread.Sleep(new Random().Next(0, 1000));

            newIndex = new Random().Next(0, ((int)img.VerticalResolution * (int)img.HorizontalResolution) + (img.Height + img.Width));
            values.Add(newIndex, Guid.NewGuid().ToString().Replace("-", ""));
            Thread.Sleep(new Random().Next(0, 1000));

            newIndex = new Random().Next(0, ((int)img.VerticalResolution * (int)img.HorizontalResolution) + (img.Height + img.Width));
            values.Add(newIndex, pixelsWithHighestRedValue.ToString());
            Thread.Sleep(new Random().Next(0, 1000));

            newIndex = new Random().Next(0, ((int)img.VerticalResolution * (int)img.HorizontalResolution) + (img.Height + img.Width));
            values.Add(newIndex, pixelsWithHighestGreenValue.ToString());
            Thread.Sleep(new Random().Next(0, 1000));

            newIndex = new Random().Next(0, ((int)img.VerticalResolution * (int)img.HorizontalResolution) + (img.Height + img.Width));
            values.Add(newIndex, pixelsWithHighestBlueValue.ToString());
            Thread.Sleep(new Random().Next(0, 1000));

            foreach (var kvp in values.OrderBy(x => x.Key))
            {
                seed += kvp.Value;
            }

            // return seed size requested
            if (config.SeedFormat == SeedFormat.Sha1)
            {
                return HashMethods.GetHashForString(seed, HashMethods.HashType.Sha1);
            }

            if (config.SeedFormat == SeedFormat.Sha256)
            {
                return HashMethods.GetHashForString(seed, HashMethods.HashType.Sha256);
            }

            if (config.SeedFormat == SeedFormat.Sha512)
            {
                return HashMethods.GetHashForString(seed, HashMethods.HashType.Sha512);
            }

            // else return default
            return HashMethods.GetHashForString(seed, HashMethods.HashType.Sha512);
        }
    }
}
