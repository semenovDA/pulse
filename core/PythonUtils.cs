﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Scripting.Hosting;
using Microsoft.Win32;
using pulse.collection;

namespace pulse.core
{
    class PythonUtils
    {
        // Public variables
        static public string SCRIPT_VSRSTATS = "scripts/test.py";

        // Private variables
        private Record _record;

        // Getters & Setters
        public Record Record { get => _record; set => _record = value; }

        // Constructors
        public PythonUtils(Record record)
        {
            _record = record;
        }

        // Public functions
        public void Excute(string script)
        {
            try
            {
                Console.WriteLine(run_cmd(script, ""));
            } 
            catch(Exception e)
            {
                throw e;
            }
        }

        // Utils functions
        private string getRegistryValue(string path, string value)
        {
            try
            {
                // TODO: Get python path iterable (recursive)
                using (RegistryKey key = Registry.CurrentUser.OpenSubKey(path))
                {
                    if (key == null) return null;
                    return key.GetValue(value) as string;

                }
            }
            catch (Exception ex)  //just for demonstration...it's always best to handle specific exceptions
            {
                throw ex;
            }
        }
        private string run_cmd(string script, string args)
        {
            ProcessStartInfo start = new ProcessStartInfo();
            start.FileName = getRegistryValue(@"Software\Python\PythonCore\3.8\InstallPath", "ExecutablePath");
            start.Arguments = string.Format("\"{0}\" \"{1}\"", script, args);
            start.UseShellExecute = false;// Do not use OS shell
            start.CreateNoWindow = true; // We don't need new window
            start.RedirectStandardOutput = true;// Any output, generated by application will be redirected back
            start.RedirectStandardError = true; // Any error in standard output will be redirected back (for example exceptions)
            using (Process process = Process.Start(start))
            {
                using (StreamReader reader = process.StandardOutput)
                {
                    string stderr = process.StandardError.ReadToEnd(); // Here are the exceptions from our Python script
                    string result = reader.ReadToEnd(); // Here is the result of StdOut(for example: print "test")
                    return result;
                }
            }
        }
    }
}
