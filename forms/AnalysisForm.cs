using pulse.collection;
using pulse.core;
using pulse.graphics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace pulse.forms
{
    public partial class AnalysisForm : Form
    {
        collection.Signal _signal;
        Dictionary<string, string> charts = new Dictionary<string, string>();

        bool mouse_is_down = false;
        int _rows = 1;
        int _columns = 1;

        public AnalysisForm(collection.Signal signal)
        {
            InitializeComponent();
            _signal = signal;

            Initialize();
            workspaceSetup();
        }

        private void Initialize()
        {
            charts.Add("Сигнал", "SIGNAL");
            charts.Add("Гисторграмма распределение", "DISTRIBUTION_HISTOGRAM");
            charts.Add("Спектограмма Welch", "WELCH_SPECTOGRAM");
            charts.Add("Спектограмма Lomb-Scargle", "LOMB_SPECTOGRAM");
            charts.Add("Спектограмма Autoregressive", "AR_SPECTOGRAM");

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
                    chart = new graphics.Signal(_signal).chart;
                    break;
                case "DISTRIBUTION_HISTOGRAM":
                    chart = new Histogram(_signal).chart;
                    break;
                case "WELCH_SPECTOGRAM":
                    chart = new Spectogram(_signal, Spectogram.Method.Welch).chart;
                    break;
                case "LOMB_SPECTOGRAM":
                    chart = new Spectogram(_signal, Spectogram.Method.Lomb).chart;
                    break;
                case "AR_SPECTOGRAM":
                    chart = new Spectogram(_signal, Spectogram.Method.Autoregressive).chart;
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
        private void listView1_MouseDown(object sender, MouseEventArgs e)
        {
            mouse_is_down = true;
        }
        private void listView1_MouseMove(object sender, MouseEventArgs e)
        {
            if (!mouse_is_down || e.Button == MouseButtons.Right) return;
            var selected_items = ((ListView)sender).SelectedItems;
            if (selected_items.Count <= 0) return;
            var next = selected_items.GetEnumerator();
            next.MoveNext();
            var str = ((ListViewItem)next.Current).Tag;
            listView1.DoDragDrop(str, DragDropEffects.Copy | DragDropEffects.Move);
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
