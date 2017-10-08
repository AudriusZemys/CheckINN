using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Tesseract;

namespace CheckINN.Frontend
{
    public partial class Form1 : Form
    {
        private TesseractEngine _tess;

        public Form1()
        {
            InitializeComponent();
            InitTesseract();

            if (_tess == null)
            {
                throw new Exception("Failed to load tessaract");
            }

            var result = DoOCR(new Bitmap(@"samples/cekis_rimi_cropped.bmp"));
            var text = result.GetText();
            File.WriteAllText("output.txt", text);
            MessageBox.Show(Owner, text);
        }

        public void InitTesseract()
        {
            _tess = new TesseractEngine(@"tessdata\", "lit");
            _tess.DefaultPageSegMode = PageSegMode.SingleBlockVertText;
        }

        public Page DoOCR(Bitmap image)
        {
            return _tess.Process(image);
        }

        public new void Dispose()
        {
            base.Dispose();
            _tess.Dispose();
        }
    }
}
