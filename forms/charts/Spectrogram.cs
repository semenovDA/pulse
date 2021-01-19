using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace pulse.forms.charts
{
    public partial class Spectrogram : Form
    {
        private void FillChart(JToken jToken)
        {
            JObject jObject = JObject.Parse(File.ReadAllText(jToken.ToString()));

            double x = 0;
            var step = 4.0 / (double.Parse(jObject["count"].ToString()));

            foreach(var o in jObject) {
                string name = o.Key;
                if (name == "count") continue;

                foreach (var p in o.Value) {
                    Spectogram.Series[name].Points.AddXY(x, double.Parse(p.ToString()));
                    Spectogram.Series[name].Points.Last().AxisLabel = (step * x).ToString();
                    x += 1;
                }
            }

            Spectogram.ChartAreas[0].AxisX.ScrollBar.Size = 10;
            Spectogram.ChartAreas[0].AxisX.ScrollBar.IsPositionedInside = true;
            Spectogram.ChartAreas[0].AxisX.ScrollBar.BackColor = Color.LightGray;
            Spectogram.ChartAreas[0].AxisX.ScrollBar.ButtonColor = Color.White;

            Spectogram.ChartAreas[0].CursorX.IsUserEnabled = true;
            Spectogram.ChartAreas[0].CursorX.IsUserSelectionEnabled = true;
            Spectogram.ChartAreas[0].CursorX.AutoScroll = true;
            Spectogram.ChartAreas[0].AxisX.ScaleView.Zoomable = true;

        }

        public Spectrogram(JToken jToken)
        {
            InitializeComponent();
            FillChart(jToken);
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
