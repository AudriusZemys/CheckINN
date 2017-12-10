using System;
using System.Drawing;

namespace CheckINN.Domain.Image
{
    public interface ITransform
    {
        Bitmap Blur(Bitmap image);
    }
}
