using pulse.collection;
using System;
using System.ComponentModel;
using System.Linq;

namespace pulse.graphics
{
    public partial class SignalChart : Component
    {

        public SignalChart(Signal signal)
        {
            InitializeComponent();
            FillCharts(signal);
            setView();
        }

        private void FillCharts(Signal signal)
        {
            chart.ChartAreas[0].AxisX.LabelStyle.Format = "hh:mm:ss.fff";

            var step = 0;
            for (double i = 0; i < signal.timestep; i += signal.HZstep)
            {
                chart.Series[0].Points.AddXY(step, signal.signal[step]);
                chart.Series[0].Points.Last().AxisLabel = GetTime((int)i);

                if (signal.peaks.Contains(step))
                {
                    chart.Series[1].Points.AddXY(step, signal.signal[step]);
                    chart.Series[1].Points.Last().AxisLabel = GetTime((int)i);
                }
                step++;
            }

        }

        private void setView()
        {
            double max = chart.Series[0].Points.Max(p => p.YValues[0]);
            double min = chart.Series[0].Points.Min(p => p.YValues[0]);

            chart.ChartAreas[0].AxisY.ScaleView.Size = max - min;
            chart.ChartAreas[0].AxisY.ScaleView.Zoom(min - 10, max + 10);

            var length = chart.Series[0].Points.Last().XValue / 10;
            chart.ChartAreas[0].AxisX.ScaleView.Zoom(0, length);


            chart.ChartAreas[0].AxisX.Interval = 100;
        }

        public static string GetTime(int ms)
        {
            TimeSpan ts = TimeSpan.FromMilliseconds(ms);
            return ts.ToString(@"hh\:mm\:ss\.fff");
        }
    }
}
