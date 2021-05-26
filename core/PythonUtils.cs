using System;
using System.Diagnostics;
using System.IO;
using Newtonsoft.Json.Linq;
using Microsoft.Win32;
using pulse.collection;
using System.Windows.Forms;

namespace pulse.core
{
    public class PythonUtils : CacheHandler
    {
        // Public variables
        static public string SCRIPT_VSRSTATS = "scripts/VSR_STATS.py";
        static public string SCRIPT_VSRSIGNAL = "scripts/VSR_SIGNAL.py";
        static public string SCRIPT_VSRNONLINEAR = "scripts/VSR_NONLINEAR.py";
        static public string SCRIPT_VSRFOURIER = "scripts/VSR_FOURIER.py";

        static public string SCRIPT_VSRPARS = "scripts/VSR_PARS.py";

        static public string SCRIPT_VSRFREQUENCY = "scripts/VSR_FREQUENCY.py";
        static public string SCRIPT_VSRTEST = "scripts/VSR_TEST.py";

        // Private variables
        private Record _record;

        // Constructors
        public PythonUtils(Record record) : base(record) => _record = record;

        // Public functions
        public JToken Excute(string script, bool base_path = true, string[] args = null)
        {
            try
            {
                var update = false;
                string propretyName = formatPropretyName(script);
                var cache = base.Cache["data"][propretyName];

                if (cache != null)
                { // Check if computes should be updated
                    base.Upload();
                    cache = base.Cache["data"][propretyName];
                    if (cache["update"] != null) update = cache["update"].ToString() == "true";
                    if(!update) return cache;
                }

                string arguments = "-i " + Path.GetFullPath(_record.getFileName());
                if(args != null) { foreach (var arg in args) arguments += " " + arg; }
                Console.WriteLine(arguments);
                return run_cmd(script, arguments, base_path);
            } 
            catch(Exception e)
            {
                MessageBox.Show(string.Format(
                    "Произошла ошибка при выполнении python скрипта !\n" +
                    "Путь: {0}\n" +
                    "Описание: {1}\n",
                    script, e.Message));
                return null;
            }
        }

        // Utils functions
        public void InstallRequirements()
        {
            var path = AppDomain.CurrentDomain.BaseDirectory;
            var spectrum_path = Path.GetFullPath(path + @"redistribution/spectrum");

            var requirments = new string[3] { spectrum_path + "/.", "pyhrv", "VSRstats" };
            for(int i = 0; i < requirments.Length; i++)
            {
                Process process = new Process();
                process.StartInfo.FileName = getRegistryValue(@"Software\Python\PythonCore", "ExecutablePath");
                var args = string.Format("-m pip install \"{0}\" --log \"{1}\"", requirments[i], @"C:\\Temp\\" + i + ".txt");
                process.StartInfo.Arguments = args;

                try
                {
                    process.Start();
                    process.WaitForExit();
                }
                catch (Exception e)
                {
                    MessageBox.Show(string.Format(
                        "Не удалось установить необходимые пакет\n" +
                        "Путь: {0}\n" +
                        "Описание: {1}",
                        path, e.Message));
                }
            }
        }
        public JToken checkRequirements()
        {
            try {
                var key = getRegistryValue(@"Software\Python\PythonCore", "ExecutablePath");
                if (key == null) InstallPython();
            }
            catch { InstallPython(); }

            try { return run_cmd(SCRIPT_VSRTEST, "", true); } 
            catch { return null;  }
        }
        private void InstallPython()
        {
            var path = AppDomain.CurrentDomain.BaseDirectory;
            path = Path.GetFullPath(path + @"redistribution/python_install.exe");

            Process process = new Process();
            process.StartInfo.FileName = path;
            process.StartInfo.Arguments = "/passive PrependPath=1";

            try
            {
                process.Start();
                process.WaitForExit();
            }
            catch(Exception e)
            {
                MessageBox.Show(string.Format(
                    "Не удалось найти файл: {0}" +
                    "\nОписание: {1}",
                    path, e.Message));
            }
            
        }
        private string getRegistryValue(string path, string value)
        {
            try
            {
                RegistryKey key = Registry.CurrentUser.OpenSubKey(path);
                var keys = key.GetSubKeyNames();

                foreach(var k in key.GetSubKeyNames())
                {
                    string p = path + "\\" + k;
                    RegistryKey key1 = Registry.CurrentUser.OpenSubKey(p);
                    var val = key1.GetValue(value);

                    var res = getRegistryValue(p, value);

                    if (val != null) return val as string;
                    if (res == null) continue;

                    return res;
                }
                return null;
            }
            catch (Exception ex) 
            {
                throw ex;
            }
        }
        public JToken run_cmd(string script, string args, bool base_path = false)
        {
            var path = base_path ? AppDomain.CurrentDomain.BaseDirectory : "";

            ProcessStartInfo start = new ProcessStartInfo();
            start.FileName = getRegistryValue(@"Software\Python\PythonCore", "ExecutablePath");
            start.Arguments = string.Format("{0} {1}", Path.GetFullPath(path + script), args);
            start.UseShellExecute = false;// Do not use OS shell
            start.CreateNoWindow = true; // We don't need new window
            start.RedirectStandardOutput = true;// Any output, generated by application will be redirected back
            start.RedirectStandardError = true; // Any error in standard output will be redirected back (for example exceptions)
            using (Process process = Process.Start(start))
            {
                using (StreamReader reader = process.StandardOutput)
                {
                    string stderr = process.StandardError.ReadToEnd(); // Here are the exceptions from our Python script
                    if (stderr != "") throw new Exception(stderr);
                    string result = reader.ReadToEnd(); // Here is the result of StdOut(for example: print "test")
                    JToken obj = JObject.Parse(result).First;

                    base.Write(obj); // Save results to cache
                    string propretyName = formatPropretyName(script);
                    var cache = base.Cache["data"][propretyName];

                    return cache;
                }
            }
        }
        public static string formatPropretyName(string script) {
            string key = "VSR_";
            int s = script.IndexOf(key);
            int e = script.IndexOf(".py");
            return script.Substring(s + key.Length, e - s - key.Length).ToLower();
        }

    }
}
