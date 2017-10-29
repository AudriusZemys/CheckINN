using System;
using System.Drawing;
using System.Windows.Forms;
using CheckINN.Domain.Entities;
using CheckINN.Domain.Parser;
using CheckINN.Domain.Processing;
using CheckINN.Domain.Services;
using Unity;
using Unity.Resolution;
using static CheckINN.Domain.Entities.ShopIdentifier;

namespace CheckINN.Frontend
{
    public partial class Form1 : Form
    {
        private readonly IUnityContainer _container;
        private readonly ICheckProcessor _processor;
        private readonly IShopParser _parser;

        public Form1(IUnityContainer container, ICheckProcessor processor, IShopParser parser)
        {
            _container = container;
            _processor = processor;
            _parser = parser;
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
            MessageBox.Show(this, "Not implemented");
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
                    var ocrText = ttr.GetText();
                    var products = _parser.ParseProductList(ocrText);
                    var check = new Check(
                        checkBody: new CheckBody(products),
                        checkFooter: new CheckFooter("321654"),
                        checkHeader: new CheckHeader(Maxima));
                    if (!_processor.TryProcess(check))
                    {
                        throw new Exception("Cant process check");
                    }
                    MessageBox.Show(Owner, ocrText);
                }
            }
        }

        private TesseractTextRecognition ResolveTesseract()
        {
            return (TesseractTextRecognition)_container.Resolve<ITextRecognition>(
                new ParameterOverride("datapath", @"tessdata\"),
                new ParameterOverride("language", "lit"));
        }
    }
}
