using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
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
    enum TimeSet {
        MS = 0,
        SEC = 1
    }

    public partial class Form2 : Form
    {
        /* Variables definition */
        PythonUtils pyhton;
        Record _record;

        public const int ms = 1000;
        public const int sec = 1;

        /* Temp variables */
        int[] _peaks;
        bool zoom = false;

        double HZ = 0;
        double HZstep = 0;
        double timestep = 0;

        /* Main constructor    */
        public Form2(Record record = null) { 
            InitializeComponent();
            if(record != null) Initialize(record);
        }

        /*  Main Functions */
        private void SetView()
        {
            double max = Signal.Series[0].Points.Max(p => p.YValues[0]);
            double min = Signal.Series[0].Points.Min(p => p.YValues[0]);

            Signal.ChartAreas[0].AxisY.ScaleView.Size = max - min;
            Signal.ChartAreas[0].AxisY.ScaleView.Zoom(min - 10, max + 10);

            var length = Signal.Series[0].Points.Last().XValue / 10;
            Signal.ChartAreas[0].AxisX.ScaleView.Zoom(0, length);

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
            
            ResetInterval();
        }

        private void FillCharts(List<int> signal)
        {
            Signal.ChartAreas[0].AxisX.LabelStyle.Format = "hh:mm:ss.fff";

            for (int i = 1; i < _peaks.Length; i++) {
                var Y = (_peaks[i] * HZstep) - (_peaks[i - 1] * HZstep);
                CIV.Series[0].Points.AddXY(i, Y / ms);
            }

            var step = 0;
            for (double i = 0; i < timestep; i += HZstep) {
                Signal.Series[0].Points.AddXY(step, signal[step]);
                Signal.Series[0].Points.Last().AxisLabel = GetTime((int)i);
                if (_peaks.Contains(step)) {
                    Signal.Series[1].Points.AddXY(step, signal[step]);
                    Signal.Series[1].Points.Last().AxisLabel = GetTime((int)i);
                }
                step ++;
            }

        }
        public void Initialize(Record record)
        {
            _record = record;
            _record.get();

            pyhton = new PythonUtils(_record);

            _peaks = pyhton.Excute(PythonUtils.SCRIPT_VSRPEAKS)
                           .Select(jv => (int)jv)
                           .ToArray();

            var filename = record.getFileName();
            var signal = File.ReadLines(filename)
                             .Select(s => int.Parse(s))
                             .ToList();

            HZ = signal.Count() / record.duration;
            HZstep = ms / HZ;
            timestep = record.duration * ms;

            FillCharts(signal);
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
        private void Form2_Load(object sender, EventArgs e) => SetView();
        private void ResetViewToolStripMenuItem_Click(object sender, EventArgs e) => SetView();
        private void StatisticsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            JToken jToken = pyhton.Excute(PythonUtils.SCRIPT_VSRSTATS);
            new VSRStatistics(_record.patient, jToken).Show();
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

            var startView = (p - 50) < 0 ? p : (p - 50);
            ZoomAxis(Signal.ChartAreas[0].AxisX, startView, r + 50);
        }
        private void Signal_CursorPositionChanging(object sender, CursorEventArgs e)
        {
            int idx = (int)(e.NewPosition > 0 ? e.NewPosition : 0);

            var Y = Signal.Series[0].Points[idx].YValues[0];
            var XLabel = Signal.Series[0].Points[idx].AxisLabel;
            int msLabel = (int)((e.NewPosition > 0 ? e.NewPosition : 0) * HZstep);

            for (int i = 1; i < _peaks.Length; i++) {
                if (idx <= _peaks[i]) {
                    SelectBar(i - 1 < 0 ? 0 : i - 1);
                    break;
                }
            }

            InfoBox.Text = String.Format("Время: {0}\tms: {1}\tY: {2}", XLabel, msLabel, Y);
        }
        private void HistogramDistributionMenuItem_Click(object sender, EventArgs e)
        {
            var points = CIV.Series[0].Points.Select(s => s.YValues[0]);
            new DistributionHistogram(points).Show();
        }
        private void ScattergramToolStripMenuItem_Click(object sender, EventArgs e)
        {
            JToken jToken = pyhton.Excute(PythonUtils.SCRIPT_VSRPOINCARE);
            new Scatterogram(_peaks, jToken).Show();
        }
        private void PowerSpectralHandler(object sender, EventArgs e)
        {
            JToken jToken = pyhton.Excute(PythonUtils.SCRIPT_VSRFREQUENCY);
            var method = Spectrogram.Method.Welch;

            switch (sender.ToString().ToLower()) {
                case "welch":
                    method = Spectrogram.Method.Welch;
                    break;
                case "lomb-scargle":
                    method = Spectrogram.Method.Lomb;
                    break;
                case "autoregressive":
                    method = Spectrogram.Method.Autoregressive;
                    break;
            }

            new Spectrogram(jToken, method).Show();
        }

        // Utils
        private string GetTime(int ms)
        {
            TimeSpan ts = TimeSpan.FromMilliseconds(ms);
            return ts.ToString(@"hh\:mm\:ss\.fff");
        }
        private void ResetInterval()
        {
            var axis = Signal.ChartAreas[0].AxisX;
            axis.Interval = 100;

        }
        public void SelectBar(int idx)
        {
            foreach (var p in CIV.Series[0].Points) { p.Color = Color.Empty; }
            CIV.Series[0].Points[idx].Color = Color.Red;
        }
        public void ZoomAxis(Axis axis, double viewStart, double viewEnd)
        {
            axis.Interval = (viewEnd - viewStart) / 4;
            axis.ScaleView.Zoom(viewStart, viewEnd);
        }

    }

}
