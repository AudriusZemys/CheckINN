using System.Drawing;

namespace CheckINN.Domain.Services
{
    public interface ITextRecongnition
    {
        string GetText();
        void Process(Bitmap image);
    }
}
