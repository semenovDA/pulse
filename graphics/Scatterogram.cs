using pulse.collection;
using pulse.forms;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms.DataVisualization.Charting;

namespace pulse.graphics
{
    public partial class Scatterogram : Component
    {
        Signal signal;
        public Scatterogram(Signal signal)
        {
            InitializeComponent();

            this.signal = signal;
            chart.Size = new Size(400, 400);

            Initialize();
        }

        private void Initialize()
        {
            var peaks = signal.computePeaks();

            for (int i = 1; i < peaks.Length - 1; i++)
            {
                double x = Math.Abs(peaks[i] - peaks[i - 1]) * signal.HZstep;
                double y = Math.Abs(peaks[i] - peaks[i + 1]) * signal.HZstep;
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

            chart.ChartAreas[0].AxisY.LabelStyle.Format = "0";
            chart.ChartAreas[0].AxisX.LabelStyle.Format = "0";

        }
        // Events
        private void chart_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            var poincare = signal.ComputePoincare();

            var width = poincare.SelectToken("sd1").ToObject<float>() * 100;
            var height = poincare.SelectToken("sd2").ToObject<float>() * 100;

            var min = chart.Series[0].Points.Min(s => s.XValue);
            var max = chart.Series[0].Points.Max(s => s.XValue);

            var center_x = Avarage(s => s.XValue, chart.Series[0].Points);
            var center_y = Avarage(s => s.YValues[0], chart.Series[0].Points);

            var center = new Point(
                (int)chart.ChartAreas[0].AxisX.ValueToPixelPosition(center_x),
                (int)chart.ChartAreas[0].AxisY.ValueToPixelPosition(center_y));

            var rect = new RectangleF(center, new SizeF(width, height));

            // AVOID THIS STUFF
            center_x = 360;
            center_y = 70;

            e.Graphics.TranslateTransform(center_x, center_y);
            e.Graphics.RotateTransform(45);

            rect = new RectangleF(0, 0, width, height);
            e.Graphics.DrawEllipse(new Pen(Color.Red, 2), rect);
        }

        // Utils
        private float Avarage(Func<DataPoint, double> selector, DataPointCollection points)
        {
            var sum = 0;
            foreach (var point in points.Select(selector)) sum += (int)point;
            return sum / points.Count;
        }

        public void Show(string title = "Скатерграмма")
        {
            var emptyFrom = new Empty { Text = title };
            emptyFrom.workspace.Controls.Add(chart);
            emptyFrom.Show();
        }
    }
}
