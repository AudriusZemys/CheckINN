using System;
using System.Drawing;
using Tesseract;

namespace CheckINN.Domain.Services
{
    public class TesseractTextRecognition : ITextRecongnition
    {
        private readonly TesseractEngine _tess;
        private Page _processedPage;

        public TesseractTextRecognition()
        {
        }

        public TesseractTextRecognition(string datapath, string language)
        {
            _tess = new TesseractEngine(datapath, language)
            {
                DefaultPageSegMode = PageSegMode.SingleBlockVertText
            };
        }

        public void Process(Bitmap image)
        {
            _processedPage = _tess.Process(image);
        }

        public string GetText()
        {
            return _processedPage.GetText() ?? String.Empty;
        }

        public void Dispose()
        {
            _tess.Dispose();
        }
    }
}