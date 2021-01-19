using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using Newtonsoft.Json.Linq;
using pulse.collection;
using pulse.core;
using pulse.forms;
using pulse.forms.charts;

namespace pulse
{
    public partial class Form2 : Form
    {
        /* Variables definition */
        PythonUtils pyhton;
        Record _record;

        int[] _peaks;
        bool zoom = false;

        /* Main constructor    */
        public Form2(Record record = null) { 
            InitializeComponent();
            if(record != null) Initialize(record);
        }

        /*  Main Functions */
        private void setView()
        {
            double max = Signal.Series[0].Points.Max(p => p.YValues[0]);
            double min = Signal.Series[0].Points.Min(p => p.YValues[0]);

            Signal.ChartAreas[0].AxisY.ScaleView.Size = max - min;
            Signal.ChartAreas[0].AxisY.ScaleView.Zoom(min - 10, max + 10);
            Signal.ChartAreas[0].AxisX.ScaleView.Zoom(0, 2000);

            // Set AxisX scrollbar style
            Signal.ChartAreas[0].AxisX.ScrollBar.Size = 10;
            Signal.ChartAreas[0].AxisX.ScrollBar.ButtonStyle = ScrollBarButtonStyles.SmallScroll;
            Signal.ChartAreas[0].AxisX.ScrollBar.IsPositionedInside = true;
            Signal.ChartAreas[0].AxisX.ScrollBar.BackColor = Color.LightGray;
            Signal.ChartAreas[0].AxisX.ScrollBar.ButtonColor = Color.White;

            CIV.ChartAreas[0].AxisX.ScrollBar.Size = 10;
            CIV.ChartAreas[0].AxisX.ScrollBar.ButtonStyle = ScrollBarButtonStyles.SmallScroll;
            CIV.ChartAreas[0].AxisX.ScrollBar.IsPositionedInside = true;
            CIV.ChartAreas[0].AxisX.ScrollBar.BackColor = Color.LightGray;
            CIV.ChartAreas[0].AxisX.ScrollBar.ButtonColor = Color.White;

            // Settings
            CIV.ChartAreas[0].CursorX.IsUserEnabled = true;

            Signal.ChartAreas[0].CursorX.IsUserEnabled = true;
            Signal.ChartAreas[0].CursorX.IsUserSelectionEnabled = true;
            Signal.ChartAreas[0].AxisX.ScaleView.Zoomable = true;

            CIV.ChartAreas[0].AxisX.ScaleView.Zoom(0, _peaks.Length / 2);

            resetInterval();
        }
        private void FillCharts(Record record)
        {
            String rl;
            int tm = 0;
            int dol2 = 0;

            Signal.Series[0].XValueType = ChartValueType.Time;
            Signal.Series[1].XValueType = ChartValueType.Time;

            Signal.ChartAreas[0].AxisX.LabelStyle.Format = "hh:mm:ss.fff";

            _peaks = pyhton.Excute(PythonUtils.SCRIPT_VSRPEAKS)
                            .Select(jv => (int)jv)
                            .ToArray();


            for (int i = 1; i < _peaks.Length; i++)
            {
                CIV.Series[0].Points.AddXY(i, _peaks[i] - _peaks[i - 1]);
            }

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
                        Signal.Series[0].Points.Last().AxisLabel = getTime(tm);
                        if (_peaks.Contains(tm))
                        {
                            Signal.Series[1].Points.AddXY(tm, rl);
                            Signal.Series[1].Points.Last().AxisLabel = getTime(tm);
                        }
                        tm++;
                    }
                }
                f.Close();
            }
        }
        public void Initialize(Record record)
        {
            _record = record;
            _record.get();

            pyhton = new PythonUtils(_record);
            FillCharts(_record);
        }

        /*  Events  */
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
        private void Form2_Load(object sender, EventArgs e) => setView();
        private void сбросToolStripMenuItem_Click(object sender, EventArgs e) => setView();
        private void вСРToolStripMenuItem_Click(object sender, EventArgs e)
        {
            JToken jToken = pyhton.Excute(PythonUtils.SCRIPT_VSRSTATS);
            VSRStatistics statistics = new VSRStatistics(_record.patient, jToken);
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
        private void CIV_CursorPositionChanged(object sender, CursorEventArgs e)
        {
            int idx = (int)e.NewPosition;

            var p = _peaks[idx == 0 ? idx : idx - 1];
            var r = _peaks[idx];

            ZoomAxis(Signal.ChartAreas[0].AxisX, p - 50, r + 50);
        }
        private void Signal_CursorPositionChanging(object sender, CursorEventArgs e)
        {
            int idx = (int)(e.NewPosition > 0 ? e.NewPosition : 0);
            var Y = Signal.Series[0].Points[idx].YValues[0];
            var XLabel = Signal.Series[0].Points[idx].AxisLabel;

            for (int i = 0; i < _peaks.Length; i++)
            {
                if (idx <= _peaks[i])
                {
                    selectBar(i - 1 < 0 ? 0 : i - 1);
                    break;
                }
            }


            InfoBox.Text = String.Format("Время: {0}\tms: {1}\tY: {2}", XLabel, idx, Y);
        }
        private void HistogramDistributionMenuItem_Click(object sender, EventArgs e)
        {
            var points = CIV.Series[0].Points.Select(s => s.YValues[0]);
            new DistributionHistogram(points).ShowDialog();
        }
        private void скатерграммаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            JToken jToken = pyhton.Excute(PythonUtils.SCRIPT_VSRPOINCARE);
            new Scatterogram(_peaks, jToken).ShowDialog();
        }
        private void спектрToolStripMenuItem_Click(object sender, EventArgs e)
        {
            JToken jToken = pyhton.Excute(PythonUtils.SCRIPT_VSRFOURIER);
            new Spectrogram(jToken).ShowDialog();
        }

        // Utils
        private string getTime(int ms)
        {
            TimeSpan ts = TimeSpan.FromMilliseconds(ms);
            return ts.ToString(@"hh\:mm\:ss\.fff");
        }
        private void resetInterval()
        {
            var axis = Signal.ChartAreas[0].AxisX;
            axis.Interval = 251;
            axis.IntervalOffset = (-axis.Minimum) % axis.Interval;
        }
        public void selectBar(int idx)
        {
            foreach (var p in CIV.Series[0].Points) { p.Color = Color.Empty; }
            CIV.Series[0].Points[idx].Color = Color.Red;
        }
        public void ZoomAxis(Axis axis, double viewStart, double viewEnd)
        {
            axis.Interval = (viewEnd - viewStart) / 4;
            axis.IntervalOffset = (-axis.Minimum) % axis.Interval;
            axis.ScaleView.Zoom(viewStart, viewEnd);
        }

    }
}
