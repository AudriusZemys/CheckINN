using System.Drawing;

namespace CheckINN.Domain
{
    interface ITextRecongnition
    {
        string GetText();
        void Process(Bitmap image);
    }
}
