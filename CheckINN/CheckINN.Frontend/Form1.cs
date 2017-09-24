using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using tessnet2;

namespace CheckINN.Frontend
{
    public partial class Form1 : Form
    {
        private tessnet2.Tesseract tess = null;

        public Form1()
        {
            InitializeComponent();
            InitTesseract();

            if (tess == null)
            {
                throw new Exception("Failed to load tessaract");
            }

            foreach (var word in DoOCR(new Bitmap(@"samples/cekis_lidl.bmp")))
            {
                Console.WriteLine(word.ToString());
            }
        }

        public void InitTesseract()
        {

            tess = new tessnet2.Tesseract();
            tess.Init(@"tessdata", "lt", true);
        }

        public List<Word> DoOCR(Bitmap image)
        {
            return tess.DoOCR(image, Rectangle.Empty);
        }
    }
}
