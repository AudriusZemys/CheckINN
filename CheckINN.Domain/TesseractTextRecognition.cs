using System.Drawing;
using Tesseract;
using static System.String;

namespace CheckINN.Domain
{
    public class TesseractTextRecognition : ITextRecongnition
    {
        private readonly TesseractEngine _tess;
        private Page _processedPage;

        public TesseractTextRecognition()
        {
            _tess = new TesseractEngine(@"tessdata\", "lit")
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
    }
}