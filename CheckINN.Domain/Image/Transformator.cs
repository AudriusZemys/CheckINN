using System.Drawing;
using Emgu.CV;
using Emgu.CV.Structure;

namespace CheckINN.Domain.Image
{
    public class Transformator : ITransform
    {
        public Bitmap Blur(Bitmap bitmap)
        {
            return Image<Gray, float>.FromIplImagePtr(new Image<Gray, byte>(bitmap)).SmoothBlur(5, 5).Bitmap;
        }
    }
}
