using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CheckINN.Frontend
{
    public partial class CheckINN_UI : Form
    {
        public CheckINN_UI()
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
