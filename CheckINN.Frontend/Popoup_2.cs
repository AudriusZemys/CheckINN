using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using CheckINN.Domain.Cache;

namespace CheckINN.Frontend
{
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

            var produce = cache.SelectMany(check => check.CheckBody.Products)
                .Select(product => new[] { $"{product.ProductEntry}", $"{product.Cost}" })
                .ToList();
            produce.ForEach(dataArray => dataGridView1.Rows.Add(dataArray));
        }
    }
}
