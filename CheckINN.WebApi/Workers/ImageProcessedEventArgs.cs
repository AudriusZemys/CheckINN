using System;
using System.Collections.Generic;
using CheckINN.Domain.Entities;

namespace CheckINN.WebApi.Workers
{
    public class ImageProcessedEventArgs : EventArgs
    {
        public ImageProcessedEventArgs(string ocrText, IEnumerable<Product> products)
        {
            OcrText = ocrText;
            Products = products;
        }

        public IEnumerable<Product> Products { get; }
        public string OcrText { get; set; }
    }
}