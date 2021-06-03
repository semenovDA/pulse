using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using pulse.core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace pulse.forms
{
    public partial class CustomScript : Form
    {
        StringDictionary scripts = new StringDictionary();
        public CustomScript()
        {
            InitializeComponent();
            var data = readScripts();
            if (data != null) scripts = data;
            updateScriptList();
        }

        static public StringDictionary readScripts()
        {
            StringDictionary _scripts = new StringDictionary();
            var filePath = Properties.Settings.Default.scriptsFile;
            var data = File.ReadAllText(filePath);
            var obj = JObject.Parse(data)["scripts"];
            foreach(var script in obj) _scripts.Add((string)script["Key"], (string)script["Value"]);
            return _scripts;
        }

        static public void saveScripts(StringDictionary scripts)
        {
            var filePath = Properties.Settings.Default.scriptsFile;
            string json = JsonConvert.SerializeObject(scripts);
            var obj = new JObject(new JProperty("scripts", JsonConvert.DeserializeObject(json)));
            json = JsonConvert.SerializeObject(obj, Formatting.Indented);
            File.WriteAllText(filePath, json);
        }

        private void updateScriptList()
        {
            scriptsList.Items.Clear();
            foreach(DictionaryEntry script in scripts) {
                scriptsList.Items.Add(script.Key + " | " + script.Value);
            }
        }

        private void choosepath_Click(object sender, EventArgs e)
        {
            filePath.Text = GetFilePath();
            if (filePath.Text != null) infoBox.Clear();
        }

        private string GetFilePath()
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Python скрипт (*.py)|*.py|All files (*.*)|*.*";
                openFileDialog.FilterIndex = 1;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK) return openFileDialog.FileName;
                else return null;
            }
        }

        private void excute_Click(object sender, EventArgs e)
        {
            PythonUtils pu = new PythonUtils(new collection.Record("test"));
            var path = AppDomain.CurrentDomain.BaseDirectory;
            string args = "-i " + Path.GetFullPath(path + "files/test_signal.txt");
            try
            {
                var result = pu.run_cmd(filePath.Text, args);
                infoBox.Text = "Результат:\n" + result;
                if (scripts.ContainsKey(name.Text)) MessageBox.Show("Ошибка. Наименование должно быть уникальным.");
                else if(name.Text == "") MessageBox.Show("Ошибка. Наименование не должен быть пустым.");
                else
                {
                    scripts.Add(name.Text, filePath.Text);
                    saveScripts(scripts);
                    updateScriptList();
                }
            }
            catch (Exception exp) { infoBox.Text = "[!] Ошибка:\n" + exp.Message; }
        }

        private void deleteBtn_Click(object sender, EventArgs e)
        {
            if (scriptsList.SelectedItem == null) return;
            var key = scriptsList.SelectedItem.ToString().Split('|')[0];
            scripts.Remove(key.Substring(0, key.Length - 1));
            saveScripts(scripts);
            updateScriptList();
        }
    }

}
