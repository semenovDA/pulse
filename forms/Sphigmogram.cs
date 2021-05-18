using System;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using Newtonsoft.Json.Linq;
using pulse.collection;
using pulse.core;
using pulse.forms;
using pulse.graphics;

namespace pulse
{
    public partial class Sphigmogram : Form
    {
        /* Variables definition */
        Record _record;
        Signal _signal;

        SignalChart signalObject;
        Chart signalChart;
        Chart histogramChart;

        /* Main constructor    */
        public Sphigmogram(Record record = null) { 
            InitializeComponent();
            if(record != null) Initialize(record);
        }

        public void Initialize(Record record)
        {
            _record = record;
            _record.get();

            _signal = new Signal(record);

            signalObject = new SignalChart(_signal);
            signalChart = signalObject.chart;
            signalChart.CursorPositionChanged += Signal_CursorPositionChanging;
            workspace.Controls.Add(signalChart, 0, 1);

            histogramChart = new Histogram(_signal, false).chart;
            histogramChart.CursorPositionChanging += CIV_CursorPositionChanged;
            workspace.Controls.Add(histogramChart, 0, 2);
        }

        /*  Events  */
        private void StatisticsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            JToken jToken = _signal.ComputeStatistics();
            new VSRStatistics(_record.patient, jToken).Show();
        }
        private void ShowValuesCb_Click(object sender, EventArgs e)
        {
            ShowValuesCb.Checked = !ShowValuesCb.Checked;
            signalChart.Series[0].IsValueShownAsLabel = ShowValuesCb.Checked;
        }
        private void CIV_CursorPositionChanged(object sender, CursorEventArgs e)
        {
            int idx = (int)e.NewPosition;

            var p = _signal.peaks[idx == 0 ? idx : idx - 1];
            var r = _signal.peaks[idx];

            var startView = (p - 50) < 0 ? p : (p - 50);
            ZoomAxis(signalChart.ChartAreas[0].AxisX, startView, r + 50);
        }
        private void Signal_CursorPositionChanging(object sender, CursorEventArgs e)
        {
            var chart = (Chart)sender;
            int idx = (int)(e.NewPosition > 0 ? e.NewPosition : 0);

            var Y = chart.Series[0].Points[idx].YValues[0];
            var XLabel = chart.Series[0].Points[idx].AxisLabel;
            int msLabel = (int)((e.NewPosition > 0 ? e.NewPosition : 0) * _signal.HZstep);

            for (int i = 1; i < _signal.peaks.Length; i++) {
                if (idx <= _signal.peaks[i]) {
                    SelectBar(i - 1 < 0 ? 0 : i - 1);
                    break;
                }
            }

            InfoBox.Text = String.Format("Время: {0}\tms: {1}\tY: {2}", XLabel, msLabel, Y);
        }
        private void HistogramDistributionMenuItem_Click(object sender, EventArgs e) => new Histogram(_signal, true).Show();
        private void ScattergramToolStripMenuItem_Click(object sender, EventArgs e) => new Scatterogram(_signal).Show();
        private void сбросToolStripMenuItem_Click(object sender, EventArgs e) => signalObject.setView();
        private void аКФToolStripMenuItem_Click(object sender, EventArgs e) => new ACFChart(_signal).Show();
        private void пАРСToolStripMenuItem_Click(object sender, EventArgs e) => new ParsRating(_signal).Show();
        private void PowerSpectralHandler(object sender, EventArgs e)
        {
            var method = Spectrogram.Method.Welch;
            string title = null;

            switch (sender.ToString().ToLower()) {
                case "welch":
                    method = Spectrogram.Method.Welch;
                    title = "Cпектральный анализ Велча";
                    break;
                case "lomb-scargle":
                    method = Spectrogram.Method.Lomb;
                    title = "Спектральный анализ методом наименьших квадратов";
                    break;
                case "autoregressive":
                    method = Spectrogram.Method.Autoregressive;
                    title = "Авторегрессионная оценка спектра";
                    break;
            }

            new Spectrogram(_signal, method).Show(title);
        }
        private void AllToolStripMenuItem_Click(object sender, EventArgs e) => new AnalysisForm(_signal).Show();

        // Utils
        public void SelectBar(int idx)
        {
            foreach (var p in histogramChart.Series[0].Points) { p.Color = Color.Empty; }
            histogramChart.Series[0].Points[idx].Color = Color.Red;
        }
        public void ZoomAxis(Axis axis, double viewStart, double viewEnd)
        {
            axis.Interval = (viewEnd - viewStart) / 4;
            axis.ScaleView.Zoom(viewStart, viewEnd);
        }
    }

}
