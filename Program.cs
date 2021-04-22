using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using AnimatedGif;

namespace Pixelator
{
    class Program
    {
        static void Main(string[] args)
        {
            var bmp = Bitmap.FromFile("Assets/bugier.jpg");
            var filename =
                Path.Combine(
                    "Path/To/Output",
                    "output.gif");
            using (var gif = AnimatedGif.AnimatedGif.Create(filename, 1000))
            {
                for (var i = 15; i >= 0; i--)
                {
                    Console.WriteLine("Frame {0} Added", 16 - i);
                    if (gif == null) continue;
                    gif.AddFrame(
                        Pixelate((Bitmap) bmp, new Rectangle(0, 0, 1200, 1200),
                            i * 15), delay: -1, quality: GifQuality.Bit8);
                }
            }
        }

        private static Bitmap Pixelate(Bitmap image, Rectangle rectangle,
            Int32 pixelateSize)
        {
            if (pixelateSize <= 0)
            {
                return image;
            }

            Bitmap pixelated =
                new System.Drawing.Bitmap(image.Width, image.Height);
            using (Graphics graphics =
                System.Drawing.Graphics.FromImage(pixelated))
                graphics.DrawImage(image,
                    new System.Drawing.Rectangle(0, 0, image.Width,
                        image.Height),
                    new Rectangle(0, 0, image.Width, image.Height),
                    GraphicsUnit.Pixel);
            for (Int32 xx = rectangle.X;
                xx < rectangle.X + rectangle.Width && xx < image.Width;
                xx += pixelateSize)
            {
                for (Int32 yy = rectangle.Y;
                    yy < rectangle.Y + rectangle.Height && yy < image.Height;
                    yy += pixelateSize)
                {
                    Int32 offsetX = pixelateSize / 2;
                    Int32 offsetY = pixelateSize / 2;
                    while (xx + offsetX >= image.Width) offsetX--;
                    while (yy + offsetY >= image.Height) offsetY--;
                    Color pixel =
                        pixelated.GetPixel(xx + offsetX, yy + offsetY);
                    for (Int32 x = xx;
                        x < xx + pixelateSize && x < image.Width;
                        x++)
                    for (Int32 y = yy;
                        y < yy + pixelateSize && y < image.Height;
                        y++)
                        pixelated.SetPixel(x, y, pixel);
                }
            }

            return pixelated;
        }
    }
}