using System;
using System.Drawing;

namespace CheckINN.Domain.Services
{
    public interface ITextRecongnition : IDisposable
    {
        string GetText();
        void Process(Bitmap image);
    }
}
