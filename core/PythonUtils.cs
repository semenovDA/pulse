using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IronPython.Hosting;
using Microsoft.Scripting.Hosting;
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
                // Microsoft Visual C++ 14.0 required to be installed
                ScriptEngine engine = Python.CreateEngine();
                engine.ExecuteFile(script);
                // Console.Read();
            } 
            catch(Exception e)
            {
                throw e;
            }
        }
    }
}
