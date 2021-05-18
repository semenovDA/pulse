using Newtonsoft.Json.Linq;
using pulse.collection;
using pulse.forms;
using System.ComponentModel;
using System.Linq;

namespace pulse.graphics
{
    public partial class ACFChart : Component
    {
        public ACFChart(Signal signal)
        {
            InitializeComponent();
            FillChart(signal);
            setView();
        }

        private void FillChart(Signal signal)
        {
            var acf = signal.ComputeACF();
            for(int i = 0; i < acf["lags"].Count(); i++)
            {
                var x = ((JValue)acf["lags"][i]).Value;
                var y = ((JValue)acf["acf_x"][i]).Value;
                chart.Series[0].Points.AddXY(x, y);
            }
        }

        private void setView()
        {
            var length = chart.Series[0].Points.Last().XValue;
            chart.ChartAreas[0].AxisX.ScaleView.Zoom(0, length / 5);
        }

        public void Show(string title = "Автокорреляционная функция")
        {
            var emptyFrom = new Empty();
            emptyFrom.Text = title;
            emptyFrom.workspace.Controls.Add(chart);
            emptyFrom.Show();
        }

    }
}
