using System;
using System.Collections.Generic;
using System.Windows.Forms;
using pulse.core;

namespace pulse.forms
{
    // Main class
    public partial class Settings : Form
    {
        public static int UNCHANGED = 0;
        public static int CHANGED_SAVED = 1;
        public static int CHANGED_NOT_SAVED = 2;
        

        private int _state = UNCHANGED;
        List<Parameter> parameters = new List<Parameter>();

        public Settings()
        {
            InitializeComponent();
            dbPath.Text = Properties.Settings.Default.DBPath;
            savesPath.Text = Properties.Settings.Default.savesPath;

            // Initialize parametr for each TextBox
            parameters.Add(new Parameter("dbPath", dbPath, dbPath.Text));
            parameters.Add(new Parameter("savesPath", savesPath, savesPath.Text));
        }

        /*  Additional functions */
        private string GetFilePath()
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Базы Данных SQLite (*.db)|*.db|All files (*.*)|*.*";
                openFileDialog.FilterIndex = 1;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK) return openFileDialog.FileName;
                else return null;
            }
        }
        private string GetDirPath()
        {
            using (FolderBrowserDialog fbd = new FolderBrowserDialog())
            {
                DialogResult result = fbd.ShowDialog();

                if (result == DialogResult.OK &&
                    !string.IsNullOrWhiteSpace(fbd.SelectedPath)) return fbd.SelectedPath;

                else return null;
            }
        }

        /*  Event functions    */
        private void dbBtn_Click(object sender, EventArgs e)
        {
            string filePath = GetFilePath();
            if (filePath == null) return;

            string connString = String.Format(DBconnection.connString, filePath);

            try { new DBconnection(connString); }
            catch(Exception exp) {
                MessageBox.Show("Ошибка. Невозможно подключится к БД.\n Подробнее: " + exp.Message);
            }
            finally {
                dbPath.Text = filePath;
                Properties.Settings.Default.DBPath = filePath;

                _state = CHANGED_NOT_SAVED;
            }
        }

        private void saveBtn_Click(object sender, EventArgs e)
        {
            _state = CHANGED_SAVED;
            Properties.Settings.Default.Save();
            MessageBox.Show("Настройки успешно сохранены");

            foreach (Parameter parameter in parameters) { parameter.update(); }
        }

        private void resetBtn_Click(object sender, EventArgs e)
        {
            _state = CHANGED_NOT_SAVED;
            Properties.Settings.Default.Reset();
            
            dbPath.Text = Properties.Settings.Default.DBPath;
            savesPath.Text = Properties.Settings.Default.savesPath;

            foreach(Parameter parameter in parameters) { parameter.update(); }
        }

        private void Settings_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(_state == CHANGED_NOT_SAVED)
            {
                var answer = MessageBox.Show("Вы хотите сохранить изменения ?",
                    "Предупреждение", MessageBoxButtons.YesNoCancel);

                if (answer == DialogResult.Yes) Properties.Settings.Default.Save();
                if (answer == DialogResult.Cancel) e.Cancel = true;
                if (answer == DialogResult.No)
                {
                    foreach(Parameter parameter in parameters) { parameter.revert(); }
                    Properties.Settings.Default.Save();
                }
            }
        }

        private void savesBtn_Click(object sender, EventArgs e)
        {
            string dirPath = GetDirPath();
            if (dirPath == null) return;
            savesPath.Text = dirPath;
            Properties.Settings.Default.savesPath = dirPath;
        }
    }
    // Additional class
    public class Parameter
    {
        public string Name { get; set; }
        public string Param { get; set; }
        public TextBox TextBox { get; set; }
        public Parameter(string name, TextBox textBox, String param)
        {
            Name = name;
            Param = param;
            TextBox = textBox;
        }
        public void update() { Param = TextBox.Text;  }
        public void revert()
        {
            if (Param != TextBox.Text) Properties.Settings.Default[Name] = Param;
        }
    }

}
