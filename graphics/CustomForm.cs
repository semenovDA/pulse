using Newtonsoft.Json.Linq;
using pulse.collection;
using pulse.forms;
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

    public partial class CustomForm : Component
    {
        string _key = "none";
        public bool isGraphical;

        public CustomForm(Signal signal, string key)
        {
            InitializeComponent();

            _key = key;
            var result = signal.ComputeCustomScript(key);

            isGraphical = (string)result["type"] == "graphical";
            if (isGraphical) drawGraphics(result);
            else drawTable(result);
        }

        private void drawTable(JToken result)
        {
            chart.Hide();
            foreach (var row in result["data"].AsJEnumerable())
            {
                var prop = row.ToObject<JProperty>();
                data.Rows.Add(prop.Name, prop.Value);
            }
        }

        private void drawGraphics(JToken result)
        {
            foreach (var c in result["data"]["charts"])
            {
                Series series = new Series() { IsXValueIndexed = true };
                if (c["type"] != null) series.Name = c["type"].ToString();
                if (c["type"] != null) series.ChartType = getChartType(c["type"].ToString());
                if (c["markerSize"] != null) series.MarkerSize = (int)c["markerSize"];
                if (c["markerStyle"] != null) series.MarkerStyle = getMarkerStyle(c["markerStyle"].ToString());
                if (c["color"] != null) series.Color = Color.FromName(c["color"].ToString());

                setPoints(series, c);
                chart.Series.Add(series);
            }
        }

        private void setPoints(Series series, JToken c)
        {
            if (c["x"] == null)
            {
                var Y = c["y"].Children().Select(jv => (double)jv).ToList();
                for (var i = 0; i < Y.Count; i++) series.Points.AddXY(i, Y[i]);
            }
            else if (c["y"] == null)
            {
                var X = c["x"].Children().Select(jv => (double)jv).ToList();
                for (var i = 0; i < X.Count; i++) series.Points.AddXY(X[i], i);
            }
            else
            {
                var X = c["x"].Children().Select(jv => (double)jv).ToList();
                var Y = c["y"].Children().Select(jv => (double)jv).ToList();
                for (var i = 0; i < X.Count; i++) series.Points.AddXY(X[i], Y[i]);
            }
        }
        private SeriesChartType getChartType(string type)
        {
            SeriesChartType chartType = SeriesChartType.Line;
            if (type.ToLower() == "scatter") chartType = SeriesChartType.Point;
            if (type.ToLower() == "bar") chartType = SeriesChartType.Bar;
            if (type.ToLower() == "line") chartType = SeriesChartType.Line;
            if (type.ToLower() == "area") chartType = SeriesChartType.Area;
            if (type.ToLower() == "boxplot") chartType = SeriesChartType.BoxPlot;
            if (type.ToLower() == "pie") chartType = SeriesChartType.Pie;
            return chartType;
        }
        private MarkerStyle getMarkerStyle(string style)
        {
            MarkerStyle marker = MarkerStyle.None;
            if (style.ToLower() == "circle") marker = MarkerStyle.Circle;
            if (style.ToLower() == "cross") marker = MarkerStyle.Cross;
            if (style.ToLower() == "diamond") marker = MarkerStyle.Diamond;
            if (style.ToLower() == "star") marker = MarkerStyle.Star5;
            if (style.ToLower() == "triangle") marker = MarkerStyle.Triangle;
            if (style.ToLower() == "square") marker = MarkerStyle.Square;
            return marker;
        } 
        public void Show()
        {
            var emptyFrom = new Empty();
            emptyFrom.Text = _key;
            emptyFrom.workspace.Controls.Add(chart);
            emptyFrom.Show();
        }
    }

}
