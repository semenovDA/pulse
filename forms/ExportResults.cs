using pulse.collection;
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
        public Dictionary<string, string> charts = new Dictionary<string, string>();

        public ExportResults(Signal signal)
        {
            InitializeComponent();

            charts.Add("Гисторграмма распределение RR", "DISTRIBUTION_HISTOGRAM_RR");
            charts.Add("Спектограмма Welch", "WELCH_SPECTOGRAM");
            charts.Add("Спектограмма Lomb-Scargle", "LOMB_SPECTOGRAM");
            charts.Add("Спектограмма Autoregressive", "AR_SPECTOGRAM");
            charts.Add("Скатерограмма", "POINCARE_SCATTERGRAM");
            charts.Add("Автокорреляционная функция", "AUTOCORRELATION_FUNCTION");

            this.signal = signal;
            Initialize();
        }
        public void Initialize()
        {
            chartList.Items.AddRange(charts.Select(s => s.Key).ToArray());
            for (var i = 0; i < chartList.Items.Count; i++) chartList.SetItemChecked(i, true);
        }

        // Events
        private void saveBtn_Click(object sender, EventArgs e)
        {
            List<Data> data = new List<Data>();
            // var path = saveToFileDialog();
            foreach(var checkedItem in chartList.CheckedItems) {

                var pair = charts.Where(
                    s => s.Key == checkedItem.ToString())
                    .First();

                var control = new AnalysisForm(signal).GetChart(pair.Value);

                fixChartView(pair.Value, control);
                if(pair.Value != "PARS_RATING") data.Add(new Data(pair, control, ""));
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
                case "DISTRIBUTION_HISTOGRAM_RR":
                    chart = (Chart)control;
                    chart.Size = new Size(500, 170);
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
