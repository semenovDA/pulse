using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace pulse.forms.charts
{

    public partial class Scatterogram : Form
    {
        private JToken stats;

        private void drawEllipse(double center, double h = 5, double w = 5, double angle = 0) {

            var rot = angle * Math.PI / 180.0;

            for (int i = -180; i < 180; i++) {
                // TODO: check formula
                var t = i * Math.PI / 180.0;
                var x = w * Math.Cos(t) * Math.Cos(rot) - h * Math.Sin(t) * Math.Sin(rot) + center;
                var y = h * Math.Sin(t) * Math.Cos(rot) + w * Math.Cos(t) * Math.Sin(rot) + center;
                ScatterogramChart.Series[2].Points.AddXY(x, y);
            }
        }

        private void Initialize(int[] peaks, JToken stats) {

            for (int i = 1; i < peaks.Length - 1; i++) {
                int x = Math.Abs(peaks[i] - peaks[i - 1]);
                int y = Math.Abs(peaks[i] - peaks[i + 1]);
                ScatterogramChart.Series[0].Points.AddXY(x, y);
            }

            var max_x = ScatterogramChart.Series[0].Points.Max(s => s.XValue) + 20;
            var max_y = ScatterogramChart.Series[0].Points.Max(s => s.YValues[0]) + 20;

            var min_x = ScatterogramChart.Series[0].Points.Min(s => s.XValue) - 20;
            var min_y = ScatterogramChart.Series[0].Points.Min(s => s.YValues[0]) - 20;

            ScatterogramChart.Series[1].Points.AddXY(0, 0);
            ScatterogramChart.Series[1].Points.AddXY(max_x, max_y);

            ScatterogramChart.ChartAreas[0].AxisX.ScaleView.Zoom(min_x, max_x);
            ScatterogramChart.ChartAreas[0].AxisY.ScaleView.Zoom(min_y, max_y);

            ScatterogramChart.ChartAreas[0].AxisX.Title = "RR (ms)";
            ScatterogramChart.ChartAreas[0].AxisY.Title = "RR[n-1] (ms)";

            // TODO: Fix double first
            var sd1 = stats["sd1"].ToObject<double>();
            var sd2 = stats["sd2"].ToObject<double>();

            var center = ScatterogramChart.Series[0].Points
                .Where(s => s.YValues[0] == s.XValue)
                .Select(s => s.XValue)
                .Average();

            drawEllipse(center, sd1, sd2);
        }

        public Scatterogram(int[] peaks, JToken res)
        {
            InitializeComponent();

            Initialize(peaks, res);
            this.stats = res;
        }

        private void angleScroll_Scroll(object sender, ScrollEventArgs e)
        {
            ScatterogramChart.Series[2].Points.Clear();

            var sd1 = stats["sd1"].ToObject<double>();
            var sd2 = stats["sd2"].ToObject<double>();

            var center = ScatterogramChart.Series[0].Points
                .Where(s => s.YValues[0] == s.XValue)
                .Select(s => s.XValue)
                .Average();

            drawEllipse(center, sd1, sd2, e.NewValue);
            angleLabel.Text = e.NewValue.ToString();
        }
    }
}
