using System;
using System.Windows.Forms;

namespace CheckINN.Frontend
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            InitTesseract();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }


        public static void InitTesseract()
        {
            var tess = new tessnet2.Tesseract();
            tess.Init(@"tessdata", "lt", true);
        }
    }
}
