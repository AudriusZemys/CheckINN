using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace CheckINN.Frontend
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var form = new Popoup_1();
            form.Show(this);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var form = new Popoup_2();
            form.Show(this);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var form = new Popoup_3();
            form.Show(this);
        }
    }
}
