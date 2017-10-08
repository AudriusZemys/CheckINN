using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Tesseract;

namespace CheckINN.Frontend
{
    public partial class Popoup_1 : Form
    {
        private TesseractEngine _tess;

        public Popoup_1()
        {
            InitializeComponent();
            InitTesseract();

            if (_tess == null)
            {
                throw new Exception("Failed to load tessaract");
            }

            var result = DoOCR(new Bitmap(@"samples/cekis_maxima_cropped.bmp"));
            MessageBox.Show(Owner, result.GetText());
        }

        public void InitTesseract()
        {
            _tess = new TesseractEngine(@"tessdata\", "lit");
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

