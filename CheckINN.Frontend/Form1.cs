using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Windows.Forms;
using CheckINN.Domain.Entities;
using CheckINN.Domain.Parser;
using CheckINN.Domain.Processing;
using CheckINN.Domain.Services;
using Newtonsoft.Json;
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

                using (var client = new HttpClient())
                using (var stream = new MemoryStream())
                {
                    new Bitmap((selectedFileName)).Save(stream, ImageFormat.Bmp);
                    var request = new HttpRequestMessage
                    {
                        RequestUri = new Uri("http://127.0.0.1:8080/api/receipt/PostReceiptGetText"),
                        Method = HttpMethod.Post,
                        Content = new ByteArrayContent(stream.ToArray())
                    };
                    request.Content.Headers.Add("Content-Type", "image/bmp");

                    var response = client.SendAsync(request).Result;
                    var definition = new { ocrText = "", success = "", message = "" };
                    var result = JsonConvert.DeserializeAnonymousType(response.Content.ReadAsStringAsync().Result, definition);
                    MessageBox.Show(Owner, result.ocrText);
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
