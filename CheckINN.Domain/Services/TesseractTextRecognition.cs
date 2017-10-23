using System.Drawing;
using Tesseract;
using static System.String;

namespace CheckINN.Domain.Services
{
    public class TesseractTextRecognition : ITextRecognition
    {
        private readonly TesseractEngine _tess;
        private Page _processedPage;

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
            return _processedPage.GetText() ?? Empty;
        }

        public void Dispose()
        {
            _tess.Dispose();
        }
    }
}