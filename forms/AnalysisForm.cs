using pulse.collection;
using pulse.graphics;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace pulse.forms
{
    public partial class AnalysisForm : Form
    {
        Signal _signal;
        public Dictionary<string, string> charts = new Dictionary<string, string>();
        List<Page> pages = new List<Page>() { new Page(1, 1) };

        bool mouse_is_down = false;
        int _page = 0;
        
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
            charts.Add("Гисторграмма распределение сигнала", "DISTRIBUTION_HISTOGRAM_SIGNAL");
            charts.Add("Гисторграмма распределение RR", "DISTRIBUTION_HISTOGRAM_RR");
            charts.Add("Спектограмма Welch", "WELCH_SPECTOGRAM");
            charts.Add("Спектограмма Lomb-Scargle", "LOMB_SPECTOGRAM");
            charts.Add("Спектограмма Autoregressive", "AR_SPECTOGRAM");
            charts.Add("Скатерограмма", "POINCARE_SCATTERGRAM");
            charts.Add("Автокорреляционная функция", "AUTOCORRELATION_FUNCTION");
            charts.Add("Оценка ПАРС", "PARS_RATING");

            foreach(DictionaryEntry script in CustomScript.readScripts()) {
                charts.Add((string)script.Key, script.Key.ToString().ToUpper());
            }

            foreach (KeyValuePair<string, string> kvp in charts) {
                var chart = new ListViewItem(kvp.Key) { Tag = kvp.Value };
                listView1.Items.Add(chart);
            }
        }

        public Control GetChart(string chartname)
        {
            Control chart = null;
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
                case "AUTOCORRELATION_FUNCTION":
                    chart = new ACFChart(_signal).chart;
                    break;
                case "PARS_RATING":
                    chart = new ParsRating(_signal).main;
                    break;
                default:
                    var key = charts.FirstOrDefault(x => x.Value == chartname).Key;
                    var path = Properties.Settings.Default.scriptsFile;
                    var scripts = CustomScript.readScripts();
                    if (!scripts.ContainsKey(key)) break;
                    var form = new CustomChart(_signal, scripts[key]);
                    if (form.isGraphical) chart = form.chart;
                    else chart = form.data;
                    break;
            }

            return chart;
        }

        private void workspaceSetup()
        {
            var page = pages[_page];
            ColumnsCount.Value = pages[_page].columns;
            RowsCount.Value = pages[_page].rows;

            workspace.ColumnCount = page.columns;
            workspace.RowCount = page.rows;

            workspace.ColumnStyles.Clear();
            workspace.RowStyles.Clear();
            workspace.Controls.Clear();

            for (int i = 0; i < page.columns; i++) {
                workspace.ColumnStyles.Add(
                    new ColumnStyle(SizeType.Percent, 100 / page.columns));
            }
            for (int i = 0; i < page.rows; i++) {
                workspace.RowStyles.Add(
                    new RowStyle(SizeType.Percent, 100 / page.rows));
            }

            foreach(var columns in page.panels) {
                foreach (var graphic in columns) {

                    graphic.panel.Dock = DockStyle.Fill;
                    graphic.panel.AllowDrop = true;
                    graphic.panel.DragDrop += workspace_DragDrop;
                    graphic.panel.DragEnter += workspace_DragEnter;

                    workspace.Controls.Add(graphic.panel);
                }
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
        public void workspace_DragDrop(object sender, DragEventArgs e)
        {
            var chart = GetChart(e.Data.GetData(DataFormats.Text).ToString());
            ((Control)sender).Controls.Clear();
            ((Control)sender).Controls.Add(chart);
            pages[_page].MarkAsCharted((Panel)sender);
                        
            mouse_is_down = false;
        }
        public void workspace_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.Text)) e.Effect = DragDropEffects.Copy;
            else e.Effect = DragDropEffects.None;
        }
        private void RowsCount_ValueChanged(object sender, EventArgs e)
        {
            if (pages[_page].rows == (int)RowsCount.Value) return;
            pages[_page].onRowsChanged((int)RowsCount.Value);
            workspaceSetup();
        }
        private void ColumnsCount_ValueChanged(object sender, EventArgs e)
        {
            if (pages[_page].columns == (int)ColumnsCount.Value) return;
            pages[_page].onColumnChanged((int)ColumnsCount.Value);
            workspaceSetup();
        }
        private void pageScrollBar_Scroll(object sender, ScrollEventArgs e)
        {
            if (e.NewValue >= pages.Count) {
                pages.Add(new Page(1, 1));
                ((ScrollBar)sender).Maximum = pages.Count;
            }
            if(e.NewValue != _page) {
                _page = e.NewValue;
                workspaceSetup();
            }
        }

    }
    public class Graphic
    {
        public Panel panel;
        public bool charted;
        public Graphic(Panel panel, bool charted)
        {
            this.panel = panel;
            this.charted = charted;
        }
    }
    public class Page
    {
        public int columns;
        public int rows;
        public List<List<Graphic>> panels = new List<List<Graphic>>();
        public Page(int columns, int rows)
        {
            this.columns = columns;
            this.rows = rows;

            for (int i = 0; i < columns; i++) {
                var col = new List<Graphic>();
                for (int j = 0; j < rows; j++) col.Add(new Graphic(new Panel(), false));
                panels.Add(col);
            }
        }
        public void MarkAsCharted(Panel panel)
        {
            foreach(var cols in panels) {
                foreach(var graph in cols) {
                    if (graph.panel == panel) graph.charted = true;
                }
            }
        }
        public void onColumnChanged(int columns)
        {
            if(this.columns < columns) {
                var col = new List<Graphic>();
                for (int j = 0; j < rows; j++) col.Add(new Graphic(new Panel(), false));
                panels.Add(col);
            } 
            else panels.RemoveAt(panels.Count - 1);
            this.columns = columns;
        }
        public void onRowsChanged(int rows)
        {
            foreach (var col in panels) {
                if (this.rows < rows) col.Add(new Graphic(new Panel(), false));
                else col.RemoveAt(col.Count - 1);
            }
            this.rows = rows;
        }
 
    }
}
