using pulse.collection;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace pulse.graphics
{
    public partial class SignalChart : Component
    {

        public SignalChart(Signal signal)
        {
            InitializeComponent();
            chart.Size = new Size(1600, 400);
            FillCharts(signal);
            setView();
        }

        private void addVerticalLine(int X, bool added = false)
        {
            var VA = new VerticalLineAnnotation()
            {
                AxisX = chart.ChartAreas[0].AxisX,
                AllowMoving = added,
                AllowSelecting = true,
                IsInfinitive = true,
                ClipToChartArea = chart.ChartAreas[0].Name,
                LineColor = Color.FromArgb(150, Color.Red),
                LineWidth = 1,
                X = X,
            };

            chart.Annotations.Add(VA);
        }

        private void FillCharts(Signal signal)
        {
            var step = 0;
            var filtred = signal.computeFiltredSignal();
            chart.ChartAreas[0].AxisX.LabelStyle.Format = "hh:mm:ss.fff";
            chart.ChartAreas[0].AxisY.LabelStyle.Format = "0.00";

            for (double i = 0; i < signal.timestep; i += signal.HZstep)
            {
                chart.Series[0].Points.AddXY(step, signal.norm_signal[step]);
                chart.Series[0].Points.Last().AxisLabel = GetTime((int)i);

                chart.Series[2].Points.AddXY(step, filtred[step]);
                chart.Series[2].Points.Last().AxisLabel = GetTime((int)i);

                if (signal.peaks.Contains(step)) addVerticalLine(step);
                step++;
            }

        }

        public void setView()
        {
            chart.ChartAreas[0].AxisY.ScaleView.Zoom(-3, 3);

            var length = chart.Series[0].Points.Last().XValue / 10;
            chart.ChartAreas[0].AxisX.ScaleView.Zoom(0, length);

            chart.ChartAreas[0].AxisX.Interval = 100;
        }

        public static string GetTime(int ms)
        {
            TimeSpan ts = TimeSpan.FromMilliseconds(ms);
            return ts.ToString(@"hh\:mm\:ss\.fff");
        }

        private void chart_AxisViewChanging(object sender, ViewEventArgs e)
        {
            int start = (int)e.Axis.ScaleView.ViewMinimum;
            int end = (int)e.Axis.ScaleView.ViewMaximum;

            List<double> allNumbers = new List<double>();

            allNumbers.AddRange(
                chart.Series[0].Points.Where((x, i) => i >= start && i <= end)
                                      .Select(x => x.YValues[0])
                                       .ToList());

            double ymin = allNumbers.Min() - 1;
            double ymax = allNumbers.Max() + 1;

            chart.ChartAreas[0].AxisY.ScaleView.Position = ymin;
            chart.ChartAreas[0].AxisY.ScaleView.Size = ymax - ymin;
        }

        private void chart_DoubleClick(object sender, EventArgs e)
        {
            addVerticalLine((int)chart.ChartAreas[0].CursorX.Position, true);
        }

    }
}
