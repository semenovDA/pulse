using Newtonsoft.Json.Linq;
using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
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

        private void FillChart(JObject jObject, String method)
        {
            var _params = jObject[method].SelectToken("params");
            var _freq = jObject[method].SelectToken("freq");
            var _power = jObject[method].SelectToken("power");
            var _freq_i = jObject[method].SelectToken("freq_i");

            var condition = (method == "welch" || method == "arr");
            drawGraph(_freq, _power, _freq_i, condition); 
            FillLegend(_params, method);
        }
        private void drawGraph(JToken freq, JToken power, JToken freq_i, bool drawLine)
        {
            int counter = 0;
            foreach (var point in freq.Zip(power, Tuple.Create))
            {
                var x = point.Item1.Value<double>();
                var y = point.Item2.Value<double>();
                var key = searchKey(freq_i, counter);

                if (key != null) {
                    Spectogram.Series[key].Points.AddXY(counter, y);
                    Spectogram.Series[key].Points.Last().AxisLabel = x.ToString("0.000");
                }
                if(drawLine)  {
                    Spectogram.Series["line"].Points.AddXY(counter, y);
                    Spectogram.Series["line"].Points.Last().AxisLabel = x.ToString("0.000");
                }

                counter++;
            }

            var max = searchMax(freq_i);
            Spectogram.ChartAreas[0].AxisX.ScaleView.Zoom(0, max);
        }
        private void FillLegend(JToken _params, String method)
        {
            var bands = extractValues(_params, "_bands").Children().ToArray();

            var peaks = extractValues(_params, "_peak").Children().Values<float>().ToArray();
            var abs = extractValues(_params, "_abs").Children().Values<float>().ToArray();
            var rel = extractValues(_params, "_rel").Children().Values<float>().ToArray();
            var log = extractValues(_params, "_log").Children().Values<float>().ToArray();
            var norm = extractValues(_params, "_norm").Children().Values<float>().ToArray();

            var ratio = extractValues(_params, "_ratio").Values<float>().ToArray()[0];
            var total = extractValues(_params, "_total").Values<float>().ToArray()[0];

            powerTextBox.Text += String.Format("Total Power: {0:0.###} [ms^2]\n", total);
            powerTextBox.Text += String.Format("LF/HF: {0:0.###} [-]", ratio);

            for (int i= 0; i < bands.Length; i++)
            {
                var band = bands[i];
                var bandName = ((JProperty)band).Name;

                // Initialize lowest and highest frequencies
                var low = band.First().First().Value<float>();
                var high = band.First().Last().Value<float>();

                // Choose right textbox
                RichTextBox textBox = ULFtextBox;
                if (bandName == "ulf") textBox = ULFtextBox;
                else if (bandName == "vlf") textBox = VLFtextBox;
                else if (bandName == "lf") textBox = LFtextBox;
                else if (bandName == "hf") textBox = HFtextBox;

                // Freqency info
                textBox.Text += bandName.ToUpper() + String.Format(": {0}Hz - {1}Hz\n", low, high); 
                textBox.Text += String.Format("Peak: {0:0.###} [Hz]\n", peaks[i]);
                textBox.Text += String.Format("Abs: {0:0.###} [ms^2]\n", abs[i]);
                textBox.Text += String.Format("Rel: {0:0.###} [%]\n", rel[i]);
                textBox.Text += String.Format("Log: {0:0.###} [-]", log[i]);
                
                if(bandName == "lf") textBox.Text += String.Format("\nNorm: {0:0.###} [-]", norm[0]);
                if(bandName == "hf") textBox.Text += String.Format("\nNorm: {0:0.###} [-]", norm[1]);
            }

        }

        public Spectrogram(JToken jToken, Method method)
        {
            InitializeComponent();

            JObject jObject = JObject.Parse(File.ReadAllText(jToken.ToString()));

            if (method == Method.Welch) FillChart(jObject, "welch");
            else if (method == Method.Lomb) FillChart(jObject, "lomb");
            else if (method == Method.Autoregressive) FillChart(jObject, "ar");

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

        private JEnumerable<JToken> extractValues(JToken jToken, String key)
        {
            return JObject.FromObject(jToken)
                   .Properties()
                   .First(p => p.Name.Contains(key))
                   .Children();
        }
    }
}
