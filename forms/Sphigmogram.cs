using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using Newtonsoft.Json.Linq;
using pulse.collection;
using pulse.core;
using pulse.forms;

namespace pulse
{
    public partial class Form2 : Form
    {
        PythonUtils pyhton;

        Record _record;
        int[] _peaks;
        bool zoom = true;

        public void GraphicInstalization(Record record)
        {
            _record = record;
            _record.get();

            pyhton = new PythonUtils(_record);

            String rl;
            int tm = 0;
            int dol2 = 0;
            int x2 = 0;

            string filename = record.getFileName();

            using (StreamReader f = new StreamReader(filename))
            {
                while (!f.EndOfStream)
                {
                    rl = f.ReadLine();
                    dol2 = rl.IndexOf('$');
                    if (rl != "" && dol2 == -1)
                    {
                        Signal.Series[0].Points.AddXY(tm, rl);
                        tm++;
                    }
                }
                f.Close();
                x2 = 0;
            }

            string[] result = pyhton.Excute(PythonUtils.SCRIPT_VSRPEAKS).Split(' ');
            _peaks = Array.ConvertAll(result, int.Parse);

            for (int i = 1; i < _peaks.Length; i++) {
                CIV.Series[0].Points.AddXY(i, _peaks[i] - _peaks[i - 1]);
            }
        }

        public Form2(Record record = null) { 
            InitializeComponent();
            if(record != null) GraphicInstalization(record);
        }
        
        private void Signal_AxisViewChanging(object sender, ViewEventArgs e)
        {
            if ((e.Axis.AxisName == AxisName.X) && (zoom == true))

            {
                int start = (int)e.Axis.ScaleView.ViewMinimum;
                int end = (int)e.Axis.ScaleView.ViewMaximum;

                List<double> allNumbers = new List<double>();

                foreach (Series item in Signal.Series)
                {
                    allNumbers
                        .AddRange(item.Points.Where((x, i) => i >= start && i <= end)
                        .Select(x => x.YValues[0]).ToList());
                }

                double ymin = allNumbers.Min();
                double ymax = allNumbers.Max();

                Signal.ChartAreas[0].AxisY.ScaleView.Position = ymin;
                Signal.ChartAreas[0].AxisY.ScaleView.Size = ymax - ymin;
                Signal.ChartAreas[0].AxisX.ScaleView.Zoom(0, 1000);
            }
        }


        private void Form2_Load(object sender, EventArgs e)
        {
            Signal.ChartAreas[0].AxisX.ScaleView.Zoom(0, 200);
            Signal.ChartAreas[0].CursorX.IsUserEnabled = true;
            Signal.ChartAreas[0].CursorX.IsUserSelectionEnabled = true;
            Signal.ChartAreas[0].AxisX.ScaleView.Zoomable = true;
            //Signal.ChartAreas[0].AxisX.ScrollBar.IsPositionedInside = true;
            Signal.ChartAreas[0].AxisY.ScaleView.Zoom(0, 30000);
            Signal.ChartAreas[0].CursorY.IsUserEnabled = true;
            Signal.ChartAreas[0].CursorY.IsUserSelectionEnabled = true;
            Signal.ChartAreas[0].AxisY.ScaleView.Zoomable = true;
            //Signal.ChartAreas[0].AxisY.ScrollBar.IsPositionedInside = true;
        }

        private void вСРToolStripMenuItem_Click(object sender, EventArgs e)
        {
            String result = pyhton.Excute(PythonUtils.SCRIPT_VSRSTATS);
            JObject jObject = JObject.Parse(result);
            Statistics statistics = new Statistics(_record.patient, jObject);
            statistics.ShowDialog();
        }

        private void ShowValuesCb_Click(object sender, EventArgs e)
        {
            ShowValuesCb.Checked = !ShowValuesCb.Checked;
            Signal.Series[0].IsValueShownAsLabel = ShowValuesCb.Checked;
        }

        private void FocusSignalCb_Click(object sender, EventArgs e)
        {
            FocusSignalCb.Checked = !FocusSignalCb.Checked;
            zoom = FocusSignalCb.Checked;
        }

        private void CIV_Click(object sender, EventArgs e)
        {
            //CIV.ChartAreas[0].AxisX.PixelPositionToValue(e.X);
        }

        // Utils
        private int [] parseInt(string [] str) {
            int[] arr = new int[str.Length];
            foreach (string s in str) {
                try { arr.Append(int.Parse(s)); }
                catch (Exception) { }
            }
            return arr;
        }
    }
}
