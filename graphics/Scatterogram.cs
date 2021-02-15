using pulse.collection;
using pulse.forms;
using System;
using System.ComponentModel;
using System.Linq;

namespace pulse.graphics
{
    public partial class Scatterogram : Component
    {
        Signal _signal;

        public Scatterogram(Signal signal)
        {
            InitializeComponent();
            _signal = signal;
            Initialize();
        }
        private void drawEllipse(double center, double h = 5, double w = 5, double angle = 0)
        {
            var rot = angle * Math.PI / 180.0;

            for (int i = -180; i < 180; i++)
            {
                // TODO: check formula
                var t = i * Math.PI / 180.0;
                var x = w * Math.Cos(t) * Math.Cos(rot) - h * Math.Sin(t) * Math.Sin(rot) + center;
                var y = h * Math.Sin(t) * Math.Cos(rot) + w * Math.Cos(t) * Math.Sin(rot) + center;
                chart.Series[2].Points.AddXY(x, y);
            }
        }
        private void Initialize()
        {
            var poincare = _signal.ComputePoincare();
            var peaks = _signal.computePeaks();

            for (int i = 1; i < peaks.Length - 1; i++)
            {
                int x = Math.Abs(peaks[i] - peaks[i - 1]);
                int y = Math.Abs(peaks[i] - peaks[i + 1]);
                chart.Series[0].Points.AddXY(x, y);
            }

            var max_x = chart.Series[0].Points.Max(s => s.XValue) + 20;
            var max_y = chart.Series[0].Points.Max(s => s.YValues[0]) + 20;

            var min_x = chart.Series[0].Points.Min(s => s.XValue) - 20;
            var min_y = chart.Series[0].Points.Min(s => s.YValues[0]) - 20;

            chart.Series[1].Points.AddXY(0, 0);
            chart.Series[1].Points.AddXY(max_x, max_y);

            chart.ChartAreas[0].AxisX.ScaleView.Zoom(min_x, max_x);
            chart.ChartAreas[0].AxisY.ScaleView.Zoom(min_y, max_y);

            // TODO: Fix double first
            var sd1 = poincare["sd1"].ToObject<double>();
            var sd2 = poincare["sd2"].ToObject<double>();

            var center = chart.Series[0].Points
                .Where(s => s.YValues[0] == s.XValue)
                .Select(s => s.XValue)
                .Average();

            drawEllipse(center, sd1, sd2);
        }
        public void Show(string title = "Скатерграмма")
        {
            var emptyFrom = new Empty();
            emptyFrom.Text = title;
            emptyFrom.workspace.Controls.Add(chart);
            emptyFrom.Show();
        }
    }
}
