using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using CheckINN.Domain.Cache;
using CheckINN.Domain.Processing;
using Tesseract;
using CheckINN.Domain.Services;
using Unity;
using Unity.Resolution;

namespace CheckINN.Frontend
{
    public partial class Form1 : Form
    {
        private readonly IUnityContainer _container;
        private readonly ICheckCache _cache;
        private readonly ICheckProcessor _processor;

        public Form1(IUnityContainer container, ICheckCache cache, ICheckProcessor processor)
        {
            _container = container;
            _cache = cache;
            _processor = processor;
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFile();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var form = _container.Resolve<Popoup_2>();
            form.Show(this);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var form = new Popoup_3();
            form.Show(this);
        }

        public void OpenFile()
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog
            {
                FilterIndex = 0,
                RestoreDirectory = true
            };


            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string selectedFileName = openFileDialog1.FileName;

                using (var ttr = ResolveTesseract())
                {
                    ttr.Process(new Bitmap(selectedFileName));
                    MessageBox.Show(Owner, ttr.GetText());
                }
            }
        }

        private ITextRecognition ResolveTesseract()
        {
            return _container.Resolve<TesseractTextRecognition>(
                new ParameterOverride("datapath", @"tessdata\"),
                new ParameterOverride("language", "lit"));
        }
    }
}
