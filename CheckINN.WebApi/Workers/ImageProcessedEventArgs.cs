using System;

namespace CheckINN.WebApi.Workers
{
    public class ImageProcessedEventArgs : EventArgs
    {
        public ImageProcessedEventArgs(string ocrText)
        {
            OcrText = ocrText;
        }

        public string OcrText { get; set; }
    }
}