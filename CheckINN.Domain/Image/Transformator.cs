using System.Drawing;
using System.Drawing.Imaging;

namespace CheckINN.Domain.Image
{
    public class Transformator : ITransform
    {
        public Bitmap ToGreyscale(Bitmap bitmap)
        {
            Bitmap newBitmap = new Bitmap(bitmap.Width, bitmap.Height);
            Graphics graphics = Graphics.FromImage(newBitmap);

            ColorMatrix colorMatrix = new ColorMatrix(
                new float[][]
                {
                    new float[] {.3f, .3f, .3f, 0, 0},
                    new float[] {.59f, .59f, .59f, 0, 0},
                    new float[] {.11f, .11f, .11f, 0, 0},
                    new float[] {0, 0, 0, 1, 0},
                    new float[] {0, 0, 0, 0, 1}
                });

            ImageAttributes attributes = new ImageAttributes();

            attributes.SetColorMatrix(colorMatrix);

            graphics.DrawImage(bitmap, new Rectangle(0, 0, bitmap.Width, bitmap.Height),
                0, 0, bitmap.Width, bitmap.Height, GraphicsUnit.Pixel, attributes);

            graphics.Dispose();

            return newBitmap;
        }

        public Bitmap Brighten(Bitmap bitmap)
        {
            float b = 0.1f;

            Bitmap newBitmap = new Bitmap(bitmap.Width, bitmap.Height);
            Graphics graphics = Graphics.FromImage(newBitmap);

            {
                ColorMatrix colorMatrix = new ColorMatrix(
                    new float[][]
                    {
                        new float[] {1, 0, 0, 0, 0},
                        new float[] {0, 1, 0, 0, 0},
                        new float[] {0, 0, 1, 0, 0},
                        new float[] {0, 0, 0, 1, 0},
                        new float[] {b, b, b, 0, 1}
                    });
                ImageAttributes attributes = new ImageAttributes();

                attributes.SetColorMatrix(colorMatrix);

                graphics.DrawImage(bitmap, new Rectangle(0, 0, bitmap.Width, bitmap.Height),
                    0, 0, bitmap.Width, bitmap.Height, GraphicsUnit.Pixel, attributes);

                graphics.Dispose();

                return newBitmap;
            }
        }

        public Bitmap Sharpen(Bitmap bitmap)
        {
            float c = 2f;
            float t = (1f - c) / 2f;

            ColorMatrix colorMatrix = new ColorMatrix(
                new float[][]
                {
                    new float[]{ c, 0, 0, 0, 0},
                    new float[]{ 0, c, 0, 0, 0},
                    new float[]{ 0, 0, c, 0 , 0},
                    new float[]{ 0,  0, 0, 1, 0},
                    new float[]{ t, t, t, 0, 1}
                });
            Bitmap newBitmap = new Bitmap(bitmap.Width, bitmap.Height);
            Graphics graphics = Graphics.FromImage(newBitmap);
            ImageAttributes attributes = new ImageAttributes();

            attributes.SetColorMatrix(colorMatrix);

            graphics.DrawImage(bitmap, new Rectangle(0, 0, bitmap.Width, bitmap.Height),
                0, 0, bitmap.Width, bitmap.Height, GraphicsUnit.Pixel, attributes);

            graphics.Dispose();

            return newBitmap;
        }
    }
}
