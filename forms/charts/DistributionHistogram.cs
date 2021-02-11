using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using MathNet.Numerics.Statistics;
using MathNet.Numerics.Distributions;

namespace pulse.forms.charts
{

    public partial class DistributionHistogram : Form
    {

        private List<PointF> NormalDistribution(IEnumerable<double> points, bool sample = false)
        {
            double mean = points.Average();
            double std = Statistics.StandardDeviation(points);
            double var = std * std;

            List<PointF> n_points = new List<PointF>();
            Normal dist = new Normal(mean, std);

            foreach (var p in points.Distinct()) {
                n_points.Add(new PointF((float)p, (float)dist.Density(p)));
            }

            return n_points.OrderBy(p => p.X).ToList();
        }

        private void FillValues(IEnumerable<double> points)
        {
            foreach (var u in points.Distinct()) {
                Histogram.Series[0].Points.AddXY(u, points.Where(p => p == u).Count());
            }

            foreach (var p in NormalDistribution(points)) {
                Histogram.Series[1].Points.AddXY(p.X, (p.Y * points.Count()));
            }

        }

        public DistributionHistogram(IEnumerable<double> points)
        {
            InitializeComponent();
            
            // Settings
            Histogram.Series[1].Color = Color.Red;
            Histogram.Series[1].BorderWidth = 2;

            Histogram.Series[2].Color = Color.Blue;
            Histogram.Series[2].BorderWidth = 3;

            // 
            FillValues(points);
        }
    }

}
