using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Windows.Forms;
using pulse.core;
using pulse.collection;

namespace pulse
{
    public partial class DBReview : Form
    {
        public DBReview() { InitializeComponent(); }

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
            catch(Exception e) { MessageBox.Show("Ошибка. Невозможно подключится к БД.\n Подробнее: " + e.Message); }
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
            catch (Exception e) { MessageBox.Show("Ошибка. Невозможно подключится к БД.\n Подробнее: " + e.Message); }
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
                                int id = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[0].Value);
                                Patient patient = new Patient(id);
                                patient.get();

                                PatientCreate f4 = new PatientCreate(patient);
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
                                Form2 f2 = new Form2(new Record(task));
                                f2.ShowDialog();
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
                                        new Record(id).delete();
                                    }
                                    catch { MessageBox.Show("Ошибка"); }
                                    finally { dataGridView1.Rows.RemoveAt(e.RowIndex); }
                                }
                                break;
                            }
                    }
                }

            }
            catch (Exception exp) { MessageBox.Show("Ошибка. Невозможно подключится к БД.\n Подробнее: " + exp.Message); }
        }

    }
}
