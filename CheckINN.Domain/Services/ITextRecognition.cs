using System;
using System.Drawing;

namespace CheckINN.Domain.Services
{
    public interface ITextRecognition : IDisposable
    {
        string GetText();
        void Process(Bitmap image);
    }
}
