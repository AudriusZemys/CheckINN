using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Windows.Forms;
using CheckINN.Domain.Cache;
using CheckINN.Domain.Entities;
using Newtonsoft.Json;

namespace CheckINN.Frontend
{
    class ProductDto
    {
        public decimal Cost { get; set; }
        public string ProductEntry { get; set; }
    }
    public partial class Popoup_2 : Form
    {
        public Popoup_2(ICheckCache cache)
        {
            InitializeComponent();

            dataGridView1.ColumnCount = 2;
            dataGridView1.Columns[0].Name = "Item";
            dataGridView1.Columns[1].Name = "Cost";

            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AllowUserToDeleteRows = false;
            dataGridView1.AllowDrop = false;

            using (var client = new HttpClient())
            {
                var request = new HttpRequestMessage
                {
                    RequestUri = new Uri("http://127.0.0.1:8080/api/cache"),
                    Method = HttpMethod.Get
                };

                var response = client.SendAsync(request).Result;
                var json = JsonConvert.DeserializeObject<List<ProductDto>>(
                    response.Content.ReadAsStringAsync().Result,
                    new JsonSerializerSettings
                    {
                        Culture = CultureInfo.CurrentCulture,
                        StringEscapeHandling = StringEscapeHandling.EscapeNonAscii
                    });
                var produce = json.Select(product => new[] {$"{product.ProductEntry}", $"{product.Cost}"}).ToList();
                produce.ForEach(dataArray => dataGridView1.Rows.Add(dataArray));
            }
        }
    }
}
