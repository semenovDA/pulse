using Newtonsoft.Json.Linq;
using pulse.collection;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;

namespace pulse.graphics
{
    public partial class Spectogram : Component
    {
        public enum Method
        {
            Welch,
            Lomb,
            Autoregressive,
        }

        public Spectogram(Signal signal, Method method)
        {
            InitializeComponent();
            Initialize(signal, method);
        }

        public void Initialize(Signal signal, Method method)
        {
            var jToken = signal.computeFrequency(method);

            var _params = jToken.SelectToken("params");
            var _freq = jToken.SelectToken("freq");
            var _power = jToken.SelectToken("power");
            var _freq_i = jToken.SelectToken("freq_i");

            var condition = (method == Method.Welch || method == Method.Autoregressive);
            drawGraph(_freq, _power, _freq_i, condition);
            FillLegend(_params);
        }
        private void FillLegend(JToken _params)
        {
            var bands = extractValues(_params, "_bands").Children().ToArray();

            var peaks = extractValues(_params, "_peak").Children().Values<float>().ToArray();
            var abs = extractValues(_params, "_abs").Children().Values<float>().ToArray();
            var rel = extractValues(_params, "_rel").Children().Values<float>().ToArray();
            var log = extractValues(_params, "_log").Children().Values<float>().ToArray();
            var norm = extractValues(_params, "_norm").Children().Values<float>().ToArray();

            var ratio = extractValues(_params, "_ratio").Values<float>().ToArray()[0];
            var total = extractValues(_params, "_total").Values<float>().ToArray()[0];

            for (int i = 0; i < bands.Length; i++)
            {
                var band = bands[i];
                var bandName = ((JProperty)band).Name;
                var item = new LegendItem() { Name = bandName, Color = chart.Series[i].Color };

                // Initialize lowest and highest frequencies
                var low = band.First().First().Value<float>();
                var high = band.First().Last().Value<float>();

                // Adding infor
                item.Cells.Add(new LegendCell(LegendCellType.SeriesSymbol, bandName));
                item.Cells.Add(new LegendCell(LegendCellType.Text, bandName.ToUpper()));
                item.Cells.Add(new LegendCell(LegendCellType.Text, String.Format("{0}Hz - {1}Hz", low, high)));
                item.Cells.Add(new LegendCell(LegendCellType.Text, String.Format("{0:0.###}", peaks[i])));
                item.Cells.Add(new LegendCell(LegendCellType.Text, String.Format("{0:0.###}", abs[i])));
                item.Cells.Add(new LegendCell(LegendCellType.Text, String.Format("{0:0.###}", rel[i])));
                item.Cells.Add(new LegendCell(LegendCellType.Text, String.Format("{0:0.###}", log[i])));

                string norm_string = "-";
                if (bandName == "lf") norm_string = String.Format("{0:0.###}", norm[0]);
                else if (bandName == "hf") norm_string = String.Format("{0:0.###}", norm[1]);

                item.Cells.Add(new LegendCell(LegendCellType.Text, norm_string));

                chart.Legends[0].CustomItems.Add(item);
            }

            // Adding total power params
            var tp = new LegendItem() { Name = "Total Power" };
            tp.Cells.Add(new LegendCell(LegendCellType.Text, ""));
            tp.Cells.Add(new LegendCell(LegendCellType.Text, "Total Power: "));
            tp.Cells.Add(new LegendCell(LegendCellType.Text, String.Format("{0:0.###} [ms^2]", total)));
            chart.Legends[0].CustomItems.Add(tp);

            var lfhf = new LegendItem() { Name = "LF/HF" };
            lfhf.Cells.Add(new LegendCell(LegendCellType.Text, ""));
            lfhf.Cells.Add(new LegendCell(LegendCellType.Text, "LF/HF:"));
            lfhf.Cells.Add(new LegendCell(LegendCellType.Text, String.Format("{0:0.###} [-]", ratio)));
            chart.Legends[0].CustomItems.Add(lfhf);
        }
        private void drawGraph(JToken freq, JToken power, JToken freq_i, bool drawLine)
        {
            int counter = 0;
            foreach (var point in freq.Zip(power, Tuple.Create))
            {
                var x = point.Item1.Value<double>();
                var y = point.Item2.Value<double>();
                var key = searchKey(freq_i, counter);

                if (key != null)
                {
                    chart.Series[key].Points.AddXY(counter, y);
                    chart.Series[key].Points.Last().AxisLabel = x.ToString("0.000");
                }
                if (drawLine)
                {
                    chart.Series["line"].Points.AddXY(counter, y);
                    chart.Series["line"].Points.Last().AxisLabel = x.ToString("0.000");
                }

                counter++;
            }

            var max = searchMax(freq_i);
            chart.ChartAreas[0].AxisX.ScaleView.Zoom(0, max);
        }

        /* Utils */
        private string searchKey(JToken jToken, int counter)
        {
            foreach (var k in jToken.Children())
            {
                var key = ((JProperty)k).Name;
                if (k.Values().Contains(counter)) return key;
            }
            return null;
        }
        private int searchMax(JToken jToken)
        {
            int max = 0;
            foreach (var k in jToken.Children())
            {
                int local_max = (int)k.Values().Max();
                if (local_max > max) max = local_max;
            }
            return max;
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
