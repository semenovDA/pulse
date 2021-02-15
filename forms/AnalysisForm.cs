using pulse.collection;
using pulse.graphics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace pulse.forms
{
    public partial class AnalysisForm : Form
    {
        Signal _signal;
        Dictionary<string, string> charts = new Dictionary<string, string>();

        bool mouse_is_down = false;
        int _rows = 1;
        int _columns = 1;

        public AnalysisForm(Signal signal)
        {
            InitializeComponent();
            _signal = signal;

            Initialize();
            workspaceSetup();
        }

        private void Initialize()
        {
            charts.Add("Сигнал", "SIGNAL");
            charts.Add("Гисторграмма распределение RR", "DISTRIBUTION_HISTOGRAM_RR");
            charts.Add("Гисторграмма распределение сигнала", "DISTRIBUTION_HISTOGRAM_SIGNAL");
            charts.Add("Спектограмма Welch", "WELCH_SPECTOGRAM");
            charts.Add("Спектограмма Lomb-Scargle", "LOMB_SPECTOGRAM");
            charts.Add("Спектограмма Autoregressive", "AR_SPECTOGRAM");
            charts.Add("Скатерограмма", "POINCARE_SCATTERGRAM");

            foreach (KeyValuePair<string, string> kvp in charts) {
                var chart = new ListViewItem(kvp.Key) { Tag = kvp.Value };
                listView1.Items.Add(chart);
            }
        }

        private Chart GetChart(string chartname)
        {
            Chart chart = null;
            switch(chartname)
            {
                case "SIGNAL":
                    chart = new SignalChart(_signal).chart;
                    break;
                case "DISTRIBUTION_HISTOGRAM_RR":
                    chart = new Histogram(_signal).chart;
                    break;
                case "DISTRIBUTION_HISTOGRAM_SIGNAL":
                    chart = new Histogram(_signal, false).chart;
                    break;
                case "WELCH_SPECTOGRAM":
                    chart = new Spectrogram(_signal, Spectrogram.Method.Welch).chart;
                    break;
                case "LOMB_SPECTOGRAM":
                    chart = new Spectrogram(_signal, Spectrogram.Method.Lomb).chart;
                    break;
                case "AR_SPECTOGRAM":
                    chart = new Spectrogram(_signal, Spectrogram.Method.Autoregressive).chart;
                    break;
                case "POINCARE_SCATTERGRAM":
                    chart = new Scatterogram(_signal).chart;
                    break;
            }

            return chart;
        }
        private Panel GetSegment()
        {
            Panel panel = new Panel();
            panel.Dock = DockStyle.Fill;
            panel.AllowDrop = true;
            panel.DragDrop += workspace_DragDrop;
            panel.DragEnter += workspace_DragEnter;
            return panel;
        }
        private void workspaceSetup()
        {
            workspace.ColumnCount = _columns;
            workspace.RowCount = _rows;

            workspace.ColumnStyles.Clear();
            workspace.RowStyles.Clear();
            workspace.Controls.Clear();

            workspace.BackColor = Color.White;
            workspace.AutoSize = true;

            for (int i = 0; i < _columns; i++) {
                workspace.ColumnStyles.Add(
                    new ColumnStyle(SizeType.Percent, 100 / _columns));
            }
            for (int i = 0; i < _rows; i++) {
                workspace.RowStyles.Add(
                    new RowStyle(SizeType.Percent, 100 / _rows));
            }

            for (int i = 0; i < _rows * _columns; i++) {
                workspace.Controls.Add(GetSegment());
            }
        }
        private void listView1_MouseDown(object sender, MouseEventArgs e) => mouse_is_down = true;
        private void listView1_MouseMove(object sender, MouseEventArgs e)
        {
            if (!mouse_is_down || e.Button != MouseButtons.Left) return;
            var selected_items = ((ListView)sender).SelectedItems;
            if (selected_items.Count <= 0) return;
            listView1.DoDragDrop(selected_items[0].Tag, DragDropEffects.Copy | DragDropEffects.Move);
        }
        private void workspace_DragDrop(object sender, DragEventArgs e)
        {
            var chart = GetChart(e.Data.GetData(DataFormats.Text).ToString());
            ((Control)sender).Controls.Add(chart);
            mouse_is_down = false;
        }
        private void workspace_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.Text)) e.Effect = DragDropEffects.Copy;
            else e.Effect = DragDropEffects.None;
        }
        private void RowsCount_ValueChanged(object sender, EventArgs e)
        {
            _rows = (int)RowsCount.Value;
            workspaceSetup();
        }
        private void ColumnsCount_ValueChanged(object sender, EventArgs e)
        {
            _columns = (int)ColumnsCount.Value;
            workspaceSetup();
        }
    }
}
