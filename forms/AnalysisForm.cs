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
        Signal _signal;

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
            var histogram = new ListViewItem("Гисторграмма распределение");
            histogram.Tag = "DISTRIBUTION_HISTOGRAM";
            listView1.Items.Add(histogram);
        }

        private Chart GetChart(string chartname)
        {
            Chart chart = null;
            switch(chartname)
            {
                case "DISTRIBUTION_HISTOGRAM":
                    chart = new Histogram(_signal).chart;
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
