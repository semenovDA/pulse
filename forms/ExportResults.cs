﻿using pulse.collection;
using pulse.core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace pulse.forms
{
    public partial class ExportResults : Form
    {
        Signal signal;
        AnalysisForm analysis;
        public ExportResults(Signal signal)
        {
            InitializeComponent();
            this.signal = signal;
            analysis = new AnalysisForm(signal);
            Initialize();
        }
        public void Initialize()
        {
            chartList.Items.AddRange(analysis.charts.Select(s => s.Key).ToArray());
            for (var i = 0; i < chartList.Items.Count; i++) chartList.SetItemChecked(i, true);
        }

        // Events
        private void saveBtn_Click(object sender, EventArgs e)
        {
            List<Data> data = new List<Data>();
            // var path = saveToFileDialog();
            foreach(var checkedItem in chartList.CheckedItems) {

                var pair = analysis.charts.Where(
                    s => s.Key == checkedItem.ToString())
                    .First();

                var control = analysis.GetChart(pair.Value);
                fixChartView(pair.Value, control);

                data.Add(new Data(pair, control, ""));
            }
            var generator = new GeneratePDF(signal, data, "C:/Users/Admin/Desktop/test.pdf");
        }

        // Utils
        private string saveToFileDialog()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "PDF Document|*.pdf";
            saveFileDialog.Title = "Сохранить отчет";
            saveFileDialog.ShowDialog();
            return saveFileDialog.FileName == "" ? null : saveFileDialog.FileName;
        }
        private void fixChartView(string chartname, Control control)
        {
            Chart chart = null;
            switch (chartname)
            {
                case "SIGNAL":
                    chart = (Chart)control;
                    chart.Size = new Size(550, 200);
                    chart.ChartAreas[0].AxisX.ScaleView
                        .Zoom(0, chart.Series[0].Points.Count / 2);
                    break;
                case "DISTRIBUTION_HISTOGRAM_RR":
                    chart = (Chart)control;
                    chart.Size = new Size(500, 170);
                    break;
                case "DISTRIBUTION_HISTOGRAM_SIGNAL":
                    chart = (Chart)control;
                    chart.Size = new Size(500, 250);
                    chart.ChartAreas[0].AxisX.ScaleView.ZoomReset();
                    break;
                case "WELCH_SPECTOGRAM":
                    chart = (Chart)control;
                    chart.Size = new Size(550, 250);
                    break;
                case "LOMB_SPECTOGRAM":
                    chart = (Chart)control;
                    chart.Size = new Size(550, 250);
                    break;
                case "AR_SPECTOGRAM":
                    chart = (Chart)control;
                    chart.Size = new Size(550, 250);
                    break;
                case "POINCARE_SCATTERGRAM":
                    chart = (Chart)control;
                    chart.Size = new Size(550, 250);
                    break;
                case "AUTOCORRELATION_FUNCTION":
                    chart = (Chart)control;
                    chart.Size = new Size(550, 200);
                    break;
            }
        }
    }
}
