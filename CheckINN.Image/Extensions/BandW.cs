using System.Drawing;
using Emgu.CV;
using Emgu.CV.Structure;

namespace CheckINN.Image.Extensions
{
    internal static partial class Extension
    {
        public static Bitmap Transform (this Bitmap img)
        {
            return Emgu.CV.Image<Gray, float>.FromIplImagePtr(new Image<Gray, byte>(img)).SmoothBlur(5, 5).Bitmap;
        }
    }
}
