using System;
using System.IO;
using System.Windows.Forms;

namespace pulse
{
    static class Program
    {

        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            DirectoryInfo drInfo = new DirectoryInfo(Properties.Settings.Default.savesPath);
            if (!drInfo.Exists) { drInfo.Create(); }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new LineAnnotation1());
        }

    }

}
