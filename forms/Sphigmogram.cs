using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using pulse.collection;
using pulse.core;

namespace pulse
{
    public partial class Form2 : Form
    {
        Record _record;
        bool zoom = false;

        public void GraphicInstalization(Record record)
        {
            _record = record;

            String rl;
            int tm = 0;
            int dol2 = 0;
            int x2 = 0;

            string filename = record.getFileName();

            using (StreamReader f = new StreamReader(filename))
            {
                while (!f.EndOfStream)
                {
                    rl = f.ReadLine();
                    dol2 = rl.IndexOf('$');
                    if (dol2 != -1)
                    {
                        rl = rl.Trim('$');
                        Console.WriteLine(rl);
                        rl.Trim('$');
                        chart2.Series[0].Points.AddXY(x2, rl);
                        dol2 = 0;
                        x2++;
                    }
                    else
                    {
                        if (rl != "")
                        {
                            chart1.Series[0].Points.AddXY(tm, rl);
                            tm++;
                        }
                    }
                }
                f.Close();
                x2 = 0;
            }
        }

        public Form2(Record record = null) { 
            InitializeComponent();
            if(record != null) GraphicInstalization(record);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            chart1.ChartAreas[0].AxisX.ScaleView.Zoom(0, 200);
            chart1.ChartAreas[0].CursorX.IsUserEnabled = true;
            chart1.ChartAreas[0].CursorX.IsUserSelectionEnabled = true;
            chart1.ChartAreas[0].AxisX.ScaleView.Zoomable = true;
            chart1.ChartAreas[0].AxisX.ScrollBar.IsPositionedInside = true;
            chart1.ChartAreas[0].AxisY.ScaleView.Zoom(0, 30000);
            chart1.ChartAreas[0].CursorY.IsUserEnabled = true;
            chart1.ChartAreas[0].CursorY.IsUserSelectionEnabled = true;
            chart1.ChartAreas[0].AxisY.ScaleView.Zoomable = true;
            chart1.ChartAreas[0].AxisY.ScrollBar.IsPositionedInside = true;
        }
        
        private void chart1_AxisViewChanging(object sender, ViewEventArgs e)
        {
            if ((e.Axis.AxisName == AxisName.X) && (zoom == true))

            {
                int start = (int)e.Axis.ScaleView.ViewMinimum;
                int end = (int)e.Axis.ScaleView.ViewMaximum;


                Console.WriteLine(start + " " + end);
                List<double> allNumbers = new List<double>();

                foreach (Series item in chart1.Series)
                {
                    allNumbers
                        .AddRange(item.Points.Where((x, i) => i >= start && i <= end)
                        .Select(x => x.YValues[0]).ToList());
                }

                double ymin = allNumbers.Min();
                double ymax = allNumbers.Max();

                chart1.ChartAreas[0].AxisY.ScaleView.Position = ymin;
                chart1.ChartAreas[0].AxisY.ScaleView.Size = ymax - ymin;
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            chart1.Series[0].IsValueShownAsLabel = checkBox1.Checked;
            chart2.Series[0].IsValueShownAsLabel = checkBox1.Checked;
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e) { zoom = checkBox2.Checked; }

        private void Form2_Load(object sender, EventArgs e)
        {
            chart1.ChartAreas[0].AxisX.ScaleView.Zoom(0, 200);
            chart1.ChartAreas[0].CursorX.IsUserEnabled = true;
            chart1.ChartAreas[0].CursorX.IsUserSelectionEnabled = true;
            chart1.ChartAreas[0].AxisX.ScaleView.Zoomable = true;
            chart1.ChartAreas[0].AxisX.ScrollBar.IsPositionedInside = true;
            chart1.ChartAreas[0].AxisY.ScaleView.Zoom(0, 30000);
            chart1.ChartAreas[0].CursorY.IsUserEnabled = true;
            chart1.ChartAreas[0].CursorY.IsUserSelectionEnabled = true;
            chart1.ChartAreas[0].AxisY.ScaleView.Zoomable = true;
            chart1.ChartAreas[0].AxisY.ScrollBar.IsPositionedInside = true;
        }

        private void вСРToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PythonUtils pyhton = new PythonUtils(_record);
            pyhton.Excute(PythonUtils.SCRIPT_VSRSTATS);
        }
    }
}
