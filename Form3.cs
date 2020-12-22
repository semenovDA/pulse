using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Windows.Forms;
using pulse.core;
using pulse.collection;

namespace pulse
{
    public partial class Form3 : Form
    {
        public Form3() { InitializeComponent(); }

        public static int PATIENT_DIALOG = 0;
        public static int RECORDS_DIALOG = 1;

        int _state = PATIENT_DIALOG;

        public DBconnection _connection = new DBconnection();

        public void LoadData()
        {
            try
            {
                DataSet dataSet = _connection.get_patients(true);
                dataGridView1.DataSource = dataSet.Tables["Table"];

                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    dataGridView1[8, i] = new DataGridViewLinkCell();
                    dataGridView1[9, i] = new DataGridViewLinkCell();
                    dataGridView1[10, i] = new DataGridViewLinkCell();
                }
            }
            catch { MessageBox.Show("Ошибка подключения к базе данных!"); }
        }
        
        public void ReloadData()
        {
            try
            {
                dataGridView1.Columns.Clear();
                LoadData();
                обновитьToolStripMenuItem.Text = "Обновить";
                фИОToolStripMenuItem.Visible = false;
                _state = PATIENT_DIALOG;
            }
            catch {  MessageBox.Show("Ошибка подключения к базе данных!"); }
        }

        private void Form3_Load(object sender, EventArgs e) { LoadData(); }
        private void обновитьToolStripMenuItem_Click(object sender, EventArgs e) { ReloadData(); }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if(_state == PATIENT_DIALOG)
                {
                    switch (e.ColumnIndex) {
                        case 8:
                            {
                                if (MessageBox.Show("Удалить строку?", "Удаление",
                                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                                {
                                    try
                                    {
                                        int id = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[0].Value);
                                        Patient patient = new Patient(id);
                                        patient.get();

                                        // TODO: Delete all *.txt files of atient records

                                        patient.delete();
                                    }
                                    catch { MessageBox.Show("Ошибка"); }
                                    finally
                                    {
                                        dataGridView1.Rows.RemoveAt(e.RowIndex);
                                        ReloadData();
                                    }
                                }
                            }
                            break;

                        case 9:
                            {
                                String task = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
                                Form4 f4 = new Form4();
                                f4.textBox1.Text = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
                                f4.textBox2.Text = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
                                f4.textBox3.Text = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();
                                f4.maskedTextBox1.Text = dataGridView1.Rows[e.RowIndex].Cells[5].Value.ToString();
                                f4.textBox5.Text = dataGridView1.Rows[e.RowIndex].Cells[6].Value.ToString();
                                f4.textBox6.Text = dataGridView1.Rows[e.RowIndex].Cells[7].Value.ToString();
                                f4.comboBox1.Text = dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString();
                                f4.label5.Text = task;
                                f4.ShowDialog();
                                ReloadData();
                            }
                            break;

                        case 10:
                            {
                                int id = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[0].Value);
                                Patient patient = new Patient(id);
                                patient.get();

                                фИОToolStripMenuItem.Text = patient.fullName();

                                dataGridView1.Columns.Add("Открыть", "Открыть");
                                dataGridView1.Columns.Add("Удалить", "Удалить");

                                DataSet dataSet = _connection.get_records(patient);

                                dataGridView1.DataSource = dataSet.Tables["Data"];
                                обновитьToolStripMenuItem.Text = "Назад";
                                фИОToolStripMenuItem.Visible = true;
                                _state = RECORDS_DIALOG;

                                dataGridView1.Columns["Пациент"].Visible = false;
                                dataGridView1.Columns["Id"].DisplayIndex = 0;
                                dataGridView1.Columns["Время"].DisplayIndex = 1;
                                dataGridView1.Columns["Длительность"].DisplayIndex = 2;
                                dataGridView1.Columns["Примечание"].DisplayIndex = 3;
                                dataGridView1.Columns["Открыть"].DisplayIndex = 4;
                                dataGridView1.Columns["Удалить"].DisplayIndex = 5;

                                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                                {
                                    dataGridView1[0, i] = new DataGridViewLinkCell();
                                    dataGridView1[0, i].Value = "Открыть";

                                    dataGridView1[1, i] = new DataGridViewLinkCell();
                                    dataGridView1[1, i].Value = "Удалить";
                                }
                            }
                            break;

                    }
                }
                else if (_state == RECORDS_DIALOG)
                {
                    switch(e.ColumnIndex)
                    {
                        case 0:
                            {
                                String task = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();

                                Form2 f2 = new Form2();

                                var fileContent = string.Empty;
                                var filePath = string.Empty;

                                String rl;
                                int tm = 0;
                                int dol2 = 0;
                                int x2 = 0;

                                using (StreamReader f = new StreamReader("saves/" + task + ".txt"))
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
                                            f2.chart2.Series[0].Points.AddXY(x2, rl);
                                            dol2 = 0;
                                            x2++;
                                        }
                                        else
                                        {
                                            if (rl != "")
                                            {
                                                f2.chart1.Series[0].Points.AddXY(tm, rl);
                                                tm++;
                                            }
                                        }
                                    }
                                    f.Close();
                                    x2 = 0;
                                    f2.ShowDialog();
                                }
                                break;
                            }
                        case 1:
                            {
                                if (MessageBox.Show("Удалить строку?", "Удаление", 
                                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                                {
                                    try
                                    {
                                        String id = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
                                        Record record = new Record(id);
                                        System.IO.File.Delete(@"saves/" + id + ".txt");
                                        record.delete();
                                    }
                                    catch { MessageBox.Show("Ошибка"); }
                                    finally { dataGridView1.Rows.RemoveAt(e.RowIndex); }
                                }
                                break;
                            }
                    }
                }

            }
            catch(Exception exp)
            {
                Console.WriteLine(exp);
            }
        }

    }
}
