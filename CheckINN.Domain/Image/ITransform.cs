using System;
using System.Drawing;

namespace CheckINN.Domain.Image
{
    public interface ITransform
    {
        Bitmap ToGreyscale(Bitmap image);
        Bitmap Brighten(Bitmap image);
        Bitmap Sharpen(Bitmap image);
    }
}
