using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using pulse.collection;

namespace pulse
{
    public partial class LineAnnotation1 : Form
    {

        /* Variable definition  */
        int dol;
        int sch;
        int x = 0;

        string msg;
        string ud;
        string s;

        Patient patient;
        Record record;
        StreamWriter wr;

        SerialPort port = new SerialPort("COM", 9600);

        public LineAnnotation1()
        {
            InitializeComponent();
            chart1.ChartAreas[0].AxisX.ScaleView.Zoom(0, 200);
            chart1.ChartAreas[0].CursorX.IsUserEnabled = true;
            chart1.ChartAreas[0].CursorX.IsUserSelectionEnabled = true;
            chart1.ChartAreas[0].AxisX.ScaleView.Zoomable = true;
            chart1.ChartAreas[0].AxisX.ScrollBar.IsPositionedInside = true;

            chart1.ChartAreas[0].CursorY.IsUserEnabled = true;
            chart1.ChartAreas[0].CursorY.IsUserSelectionEnabled = true;
            chart1.ChartAreas[0].AxisY.ScaleView.Zoomable = true;
            LineAnnotation annotaton1 = new LineAnnotation();

            chart1.Series[0].Points.AddXY(0, 0);


            chart1.ChartAreas[0].AxisY.ScrollBar.IsPositionedInside = true;
            string[] ports = SerialPort.GetPortNames();

            comboBox1.Items.AddRange(ports);
            comboBox2.Items.AddRange(new string[] { "300", "1200", "2400", "4800", 
                "9600", "19200", "38400", "57600", "74880", "115200", "230400", "250000" });

            comboBox2.SelectedItem = "9600";
            chart1.Series[0].ToolTip = "X = #VALX, Y = #VALY";
            button2.Enabled = false;
        }

        public void button1_Click(object sender, EventArgs e)
        {
            if (port.IsOpen == true)
            {
                timer1.Stop();
                port.Close();
                button1.Text = "Старт";
                button2.Enabled = false;
            }
            else
            {
                try
                {
                    port.PortName = comboBox1.Text.ToString();
                    port.BaudRate = Int32.Parse(this.comboBox2.SelectedItem.ToString());

                    port.Open();
                    timer1.Start();
                    button1.Text = "Стоп";
                    button2.Enabled = true;
                }
                catch { MessageBox.Show("Указанный COM порт не подключен!"); }

            }
        }

        public void timer1_Tick(object sender, EventArgs e)
        {
            while (port.BytesToRead > 0)
            {
                msg = port.ReadLine();
                Console.WriteLine(port.BytesToRead);
                dol = msg.IndexOf('$');
                if (dol != -1)
                {
                    ud = msg.Trim('$');
                    Console.WriteLine(msg);
                    msg.Trim('$');
                    label4.Text = "Частота: " + ud + "(уд/мин)";

                    dol = 0;
                }
                else
                {
                    chart1.Series[0].Points.AddXY(x, msg);
                    chart1.ChartAreas[0].AxisX.ScaleView.Zoom(x - hScrollBar1.Value, x);
                    label3.Text = "Видимый диапазон по шкале Х:" + hScrollBar1.Value;
                    x++;
                }

            }

        }

        private void chart1_Click(object sender, EventArgs e)
        {
            this.MouseWheel += new MouseEventHandler(this_MouseWheel);
        }

        void this_MouseWheel(object sender, MouseEventArgs e)
        {
            if (e.Delta > 0)
            {
                hScrollBar1.Value = hScrollBar1.Value + 10;
                chart1.ChartAreas[0].AxisX.ScaleView.Zoom(x - hScrollBar1.Value, x);
                label3.Text = "Видимый диапазон по шкале Х:" + hScrollBar1.Value;
            }

            else
            {
                if (hScrollBar1.Value > 60)
                {
                    hScrollBar1.Value = hScrollBar1.Value - 10;
                    chart1.ChartAreas[0].AxisX.ScaleView.Zoom(x - hScrollBar1.Value, x);
                    label3.Text = "Видимый диапазон по шкале Х:" + hScrollBar1.Value;
                }
            }

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            { chart1.Series[0].IsValueShownAsLabel = true; }
            else { chart1.Series[0].IsValueShownAsLabel = false; }
        }

        private void hScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            chart1.ChartAreas[0].AxisX.ScaleView.Zoom(x - hScrollBar1.Value, x);
            label3.Text = "Видимый диапазон по шкале Х:" + hScrollBar1.Value;
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked == true) timer2.Start();
            else timer2.Stop();
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            int start = x - hScrollBar1.Value;
            int end = x;
            Console.WriteLine(start + " " + end);
            List<double> allNumbers = new List<double>();

            foreach (Series item in chart1.Series)
                allNumbers.AddRange(item.Points.Where((x, i) => i >= start && i <= end).Select(x => x.YValues[0]).ToList());
            try
            {
                double ymin = allNumbers.Min();
                double ymax = allNumbers.Max();
                chart1.ChartAreas[0].AxisY.ScaleView.Position = ymin;
                chart1.ChartAreas[0].AxisY.ScaleView.Size = ymax - ymin;
            }
            catch (Exception)
            {

                // throw;
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if(patient == null) { MessageBox.Show("Выберите пациента"); }
            else if (patient.id != 0)
            {
                if (timer3.Enabled != true)
                {
                    DateTime time = DateTime.Now;
                    String stime = (time.ToString("hh:mm:ss.f"));
                    s = DateTime.Now.ToString("yyyyMMddhhmmss");

                    if (textBox1.Text != "")
                    {
                        sch = Convert.ToInt32(textBox1.Text) * 60;

                        timer4.Start();
                        timer1.Stop();
                        timer3.Start();

                        record = new Record(sch, patient);
                        string filename = "saves/" + record.id + ".txt";
                        wr = new StreamWriter(filename);
                        Console.WriteLine(String.Format("Writing to {0} ...", filename)); // DEBUG

                        button2.Text = "Идет запись";
                        textBox1.ReadOnly = true;
                        button2.ForeColor = System.Drawing.Color.Red;

                    }
                    else { MessageBox.Show("Введите время записи (в минутах)!"); }

                }
                else
                {
                    timer3.Stop();
                    wr.Close();
                    button2.Text = "Записать";
                    textBox1.ReadOnly = false;
                    button2.ForeColor = System.Drawing.Color.Black;
                    button1.PerformClick();

                    Console.WriteLine(s);
                    Form6 f6 = new Form6(record);
                    if (timer4.Enabled == true) { timer4.Stop(); label5.Text = "Время записи (мин): "; }
                    f6.ShowDialog();
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            chart1.Series[0].Points.Clear();
            x = 0;

            if (timer3.Enabled == true)
            {
                timer3.Stop();
                wr.Close();

                string filename = "saves/" + record.id + ".txt";
                System.IO.File.Delete(filename);
                sch = Convert.ToInt32(textBox1.Text) * 60;
                wr = new StreamWriter(filename);
                timer3.Start();
            }
        }

        private void timer3_Tick(object sender, EventArgs e)
        {
            try
            {
                while (port.BytesToRead > 0)
                {
                    msg = port.ReadLine();
                    wr.WriteLine(msg);
                    dol = msg.IndexOf('$');
                    if (dol != -1)
                    {
                        ud = msg.Trim('$');
                        msg.Trim('$');
                        label4.Text = "Частота: " + ud + "(уд/мин)";
                        dol = 0;
                    }
                    else
                    {
                        chart1.Series[0].Points.AddXY(x, msg);
                        chart1.ChartAreas[0].AxisX.ScaleView.Zoom(x - hScrollBar1.Value, x);
                        label3.Text = "Видимый диапазон по шкале Х:" + hScrollBar1.Value;
                        x++;
                    }

                }
            }
            catch
            {
                timer3.Stop();
                MessageBox.Show("Ошибка. Возможно закрыт порт.");
                button2.Text = "Записать";
                textBox1.ReadOnly = false;
                button2.ForeColor = System.Drawing.Color.Black;

            }



        }

        private void базаДанныхToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form3 f3 = new Form3();
            f3.ShowDialog();
        }

        private void цветФонаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            colorDialog1.AllowFullOpen = true;
            colorDialog1.ShowHelp = false;
            if (colorDialog1.ShowDialog() == DialogResult.OK)

                chart1.ChartAreas["ChartArea1"].BackColor = colorDialog1.Color;
        }

        private void цветСеткиToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            colorDialog1.AllowFullOpen = true;
            colorDialog1.ShowHelp = false;
            if (colorDialog1.ShowDialog() == DialogResult.OK)
                chart1.ChartAreas["ChartArea1"].AxisX.MajorGrid.LineColor = colorDialog1.Color;
            chart1.ChartAreas["ChartArea1"].AxisY.MajorGrid.LineColor = colorDialog1.Color;
        }

        private void цветГрафикаToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            colorDialog1.AllowFullOpen = true;
            colorDialog1.ShowHelp = false;
            if (colorDialog1.ShowDialog() == DialogResult.OK)

                chart1.Series["Series1"].Color = colorDialog1.Color;
        }

        private void открытьФайлыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form2 f2 = new Form2();

            var fileContent = string.Empty;
            var filePath = string.Empty;
            String rl;
            int tm = 0;
            int dol2 = 0;
            int x2 = 0;

            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
                openFileDialog.FilterIndex = 2;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    //Get the path of specified file
                    filePath = openFileDialog.FileName;

                    //Read the contents of the file into a stream
                    var fileStream = openFileDialog.OpenFile();

                    using (StreamReader f = new StreamReader(fileStream))
                    {
                        while (!f.EndOfStream)
                        {
                            rl = f.ReadLine();
                            dol2 = rl.IndexOf('$');
                            if (dol2 != -1)
                            {
                                rl = rl.Trim('$');
                                Console.WriteLine(rl);
                                f2.chart2.Series[0].Points.AddXY(x2, rl);
                                dol2 = 0;
                                x2++;
                            }
                            else
                            {
                                if (rl != "")
                                {
                                    f2.chart1.Series[0].Points.AddXY(tm, rl);
                                    f2.chart1.ChartAreas[0].AxisX.ScaleView.Zoom(x - 300, x);
                                    tm++;
                                }
                            }
                        }
                        f.Close();
                        x2 = 0;
                        f2.ShowDialog();
                    }
                }

            }

        }
        
        private void timer4_Tick(object sender, EventArgs e)
        {
            if (sch > 0) { sch--; label5.Text = "Осталось (сек): " + sch;  }
            if ((sch == 0) && (timer3.Enabled == true))
            {
                button2.PerformClick();
                timer4.Stop();
                label5.Text = "Время записи: ";
            }
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            char number = e.KeyChar;
            if (!Char.IsDigit(number) && number != 8) // цифры и клавиша BackSpace
            {
                e.Handled = true;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Form5 f5 = new Form5();
            f5.ShowDialog();
            if(f5.patient != null) {
                patient = f5.patient;
                label6.Text = "Пациент: " + patient.fullName();
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Form4 f4 = new Form4();
            f4.ShowDialog();
        }  

        private void comboBox1_DropDown(object sender, EventArgs e)
        {
            comboBox1.Items.Clear();
            string[] portes = SerialPort.GetPortNames();
            comboBox1.Items.AddRange(portes);
        }
    }

}
