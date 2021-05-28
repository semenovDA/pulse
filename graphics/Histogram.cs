using MathNet.Numerics.Statistics;
using pulse.collection;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using MathNet.Numerics.Distributions;
using pulse.forms;

namespace pulse.graphics
{
    public partial class Histogram : Component
    {
        public Histogram(Signal signal, bool normal_disribution = true)
        {
            InitializeComponent();
            if (normal_disribution) FillDistribution(signal);
            else FillValues(signal);
            chart.Size = new Size(1200, 500);
        }

        private void FillDistribution(Signal signal)
        {
            List<double> points = signal.computeRR(false);
            foreach (var u in points.Distinct()) chart.Series[0].Points.AddXY(u, points.Where(p => p == u).Count());
            foreach (var p in NormalDistribution(points)) chart.Series[1].Points.AddXY(p.X, p.Y * points.Count());

            chart.ChartAreas[0].AxisX.Title = "RR";
            chart.ChartAreas[0].AxisY.Title = "Count";
        }

        private void FillValues(Signal signal)
        {
            List<double> points = signal.computeRR(true);
            for (var i = 0; i < points.Count; i++) chart.Series[0].Points.AddXY(i + 1, points[i] * Signal.ms);
            chart.ChartAreas[0].AxisX.ScaleView.Zoom(0, signal.peaks.Length / 2);

            chart.ChartAreas[0].AxisX.Title = "RR number";
            chart.ChartAreas[0].AxisY.Title = "ms";
        }

        public void Show(string title = "Гистограмма распределение")
        {
            var emptyFrom = new Empty();
            emptyFrom.Text = title;
            emptyFrom.workspace.Controls.Add(chart);
            emptyFrom.Show();
        }

        private List<PointF> NormalDistribution(List<double> points, bool sample = false)
        {
            double mean = points.Average();
            double std = Statistics.StandardDeviation(points);
            double var = std * std;

            List<PointF> n_points = new List<PointF>();
            Normal dist = new Normal(mean, std);

            foreach (var p in points.Distinct())
            {
                n_points.Add(new PointF((float)p, (float)dist.Density(p)));
            }

            return n_points.OrderBy(p => p.X).ToList();
        }
    }
}
