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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            InitTesseract();
        }

        public void InitTesseract()
        {
            var tess = new tessnet2.Tesseract();
            tess.Init(@"tessdata", "lt", true);
        }
    }
}
