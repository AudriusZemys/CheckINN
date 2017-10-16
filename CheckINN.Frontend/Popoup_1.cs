using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using Tesseract;
using CheckINN.Domain.Services;

namespace CheckINN.Frontend
{
    public partial class Popoup_1 : Form
    {
        private TesseractEngine _tess;
        private readonly ITextRecongnition _textRecognition;

        public Popoup_1()
        {
            InitializeComponent();
            _textRecognition = new TesseractTextRecognition(@"tessdata\", "lit");
        }

        public void InitTesseract()
        {
            _tess = new TesseractEngine(@"tessdata\", "lit");
        }

        public void DoOCR(Bitmap image)
        {
            _textRecognition.Process(image);
        }

        public new void Dispose()
        {
            base.Dispose();
            _tess.Dispose();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.FilterIndex = 0;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string selectedFileName = openFileDialog1.FileName;

                InitTesseract();

                if (_tess == null)
                {
                    throw new Exception("Failed to load tessaract");
                }

                DoOCR(new Bitmap(selectedFileName));
                MessageBox.Show(Owner, _textRecognition.GetText());
            }
        }
    }
}

