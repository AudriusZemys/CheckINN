using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net.Http;
using System.Windows.Forms;
using Newtonsoft.Json;
using Unity;

namespace CheckINN.Frontend
{
    public partial class Form1 : Form
    {
        private readonly IUnityContainer _container;

        public Form1(IUnityContainer container)
        {
            _container = container;
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

    }
}
