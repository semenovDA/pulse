using MathNet.Numerics.Statistics;
using pulse.collection;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using MathNet.Numerics.Distributions;

namespace pulse.graphics
{
    public partial class Histogram : Component
    {
        public Histogram(Signal signal)
        {
            InitializeComponent();
            FillValues(signal.computeRR(false));
        }

        public Histogram(IContainer container, Signal signal)
        {
            container.Add(this);
            InitializeComponent();
            FillValues(signal.computeRR(false));
        }

        private void FillValues(List<double> points)
        {
            foreach (var u in points.Distinct())
            {
                chart.Series[0].Points.AddXY(u, points.Where(p => p == u).Count());
            }

            foreach (var p in NormalDistribution(points))
            {
                chart.Series[1].Points.AddXY(p.X, p.Y * points.Count());
            }
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
