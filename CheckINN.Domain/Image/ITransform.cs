using System;
using System.Drawing;

namespace CheckINN.Domain.Image
{
    public interface ITransform
    {
        void Transform(Bitmap image);
    }
}
