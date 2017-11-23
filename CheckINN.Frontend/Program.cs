using System;
using System.Windows.Forms;
using Unity;
using Unity.Lifetime;

namespace CheckINN.Frontend
{
    static class Program
    {
        static IUnityContainer BuildContainer()
        {
            IUnityContainer container = new UnityContainer();
            container.RegisterInstance(container);
            container.RegisterType<Form1>();
            container.RegisterType<Popoup_2>();
            return container;
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            var container = BuildContainer();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(container.Resolve<Form1>());
        }
    }
}
