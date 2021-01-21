using Newtonsoft.Json.Linq;
using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace pulse.forms.charts
{
    public partial class Spectrogram : Form
    {
        public enum Method
        {
            Welch,
            Lomb,
            Autoregressive,
        }

        private string searchKey(JToken jToken, int counter)
        {
            foreach (var k in jToken.Children()) {
                var key = ((Newtonsoft.Json.Linq.JProperty)k).Name;
                if (k.Values().Contains(counter)) return key;
            }
            return null;
        }

        private int searchMax(JToken jToken)
        {
            int max = 0;
            foreach (var k in jToken.Children()) {
                int local_max = (int)k.Values().Max();
                if (local_max > max) max = local_max;
            }
            return max;
        }

        private void defaultChartArea()
        {
            Spectogram.ChartAreas[0].AxisX.ScrollBar.Size = 10;
            Spectogram.ChartAreas[0].AxisX.ScrollBar.IsPositionedInside = true;
            Spectogram.ChartAreas[0].AxisX.ScrollBar.BackColor = Color.LightGray;
            Spectogram.ChartAreas[0].AxisX.ScrollBar.ButtonColor = Color.White;

            Spectogram.ChartAreas[0].CursorX.IsUserEnabled = true;
            Spectogram.ChartAreas[0].CursorX.IsUserSelectionEnabled = true;
            Spectogram.ChartAreas[0].CursorX.AutoScroll = true;
            Spectogram.ChartAreas[0].AxisX.ScaleView.Zoomable = true;
        }

        private void drawGraph(JObject jObject, String method)
        {
            var _params = jObject[method].SelectToken("params");
            var _freq = jObject[method].SelectToken("freq");
            var _power = jObject[method].SelectToken("power");
            var _freq_i = jObject[method].SelectToken("freq_i");

            int counter = 0;
            foreach (var point in _freq.Zip(_power, Tuple.Create))
            {
                var x = point.Item1.Value<double>();
                var y = point.Item2.Value<double>();
                var key = searchKey(_freq_i, counter);

                if(key != null) {
                    Spectogram.Series[key].Points.AddXY(counter, y);
                    Spectogram.Series[key].Points.Last().AxisLabel = x.ToString();
                }

                Spectogram.Series["line"].Points.AddXY(counter, y);
                Spectogram.Series["line"].Points.Last().AxisLabel = x.ToString();

                counter++;
            }

            var max = searchMax(_freq_i);
            Spectogram.ChartAreas[0].AxisX.ScaleView.Zoom(0, max);
        }

        public Spectrogram(JToken jToken, Method method)
        {
            InitializeComponent();

            JObject jObject = JObject.Parse(File.ReadAllText(jToken.ToString()));

            if (method == Method.Welch) drawGraph(jObject, "welch");
            else if (method == Method.Lomb) drawGraph(jObject, "lomb");
            else if (method == Method.Autoregressive) drawGraph(jObject, "ar");

            defaultChartArea();
        }

        private void Spectogram_CursorPositionChanging(object sender, CursorEventArgs e)
        {
            int idx = (int)(e.NewPosition > 0 ? e.NewPosition : 0);
            foreach(var series in Spectogram.Series) {
                var arr = series.Points.Where(p => p.XValue == idx);
                if(arr.Count() != 0) {
                    var X = arr.First().AxisLabel;
                    var Y = arr.First().YValues[0];
                    infoBox.Text = String.Format("X:{0} Y:{1}", X, Y);
                    break;  
                }
            }

        }
    }
}
