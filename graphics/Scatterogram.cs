using Newtonsoft.Json.Linq;
using pulse.collection;
using pulse.forms;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;

namespace pulse.graphics
{
    public partial class Scatterogram : Component
    {
        public Scatterogram(Signal signal)
        {
            InitializeComponent();
            Initialize(signal);
        }
        private void drawEllipse(float[] x, float[] y)
        {
            Pen redPen = new Pen(Color.Red, 2);
            Graphics g = chart.CreateGraphics();
            PointF[] points = new PointF[x.Length];

            for(int i = 0; i < x.Length; i++) points.Append(new PointF(x[i], y[i]));

            g.DrawLines(redPen, points);
            g.DrawCurve(redPen, points);
        }
        private void Initialize(Signal signal)
        {
            var poincare = signal.ComputePoincare();
            var peaks = signal.computePeaks();

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

            float[] ellipse_x = poincare.SelectToken("ellipse.x").ToObject<float[]>();
            float[] ellipse_y = poincare.SelectToken("ellipse.y").ToObject<float[]>();

            drawEllipse(ellipse_x, ellipse_y);
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
