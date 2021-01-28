using pulse.core;
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
            var path = AppDomain.CurrentDomain.BaseDirectory + @"saves\";
            if (Properties.Settings.Default.savesPath == "") Properties.Settings.Default.savesPath = path;

            DirectoryInfo drInfo = new DirectoryInfo(Properties.Settings.Default.savesPath);
            if (!drInfo.Exists) { drInfo.Create(); }

            PythonUtils pu = new PythonUtils(new collection.Record("test"));
            var res = pu.checkRequirements();
            if (res == null)
            {
                MessageBox.Show("Необходимые пакеты не установлены !\n" +
                    "Не завершайте работу пока пакеты устанавливаются ...");
                pu.InstallRequirements();
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new LineAnnotation1());
        }

    }

}
