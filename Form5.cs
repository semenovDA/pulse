using System;
using System.Data;
using System.Windows.Forms;
using pulse_2._0.core;
using pulse_2._0.collection;

namespace pulse_2._0
{
    public partial class Form5 : Form
    {
        public Form5() { InitializeComponent(); }
        private DBconnection _connection = new DBconnection();
        public Patient _patient;

        public Patient patient { get => _patient; set => _patient = value; }

        private void Form5_Load(object sender, EventArgs e)
        {
            try
            {
                DataSet dataSet = _connection.get_patients();
                dataGridView1.DataSource = dataSet.Tables["Table"];

                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    DataGridViewLinkCell linkCell = new DataGridViewLinkCell();
                    dataGridView1[8, i] = linkCell;
                }
            }

            catch (Exception execption)
            {
                MessageBox.Show("Ошибка подключения к базе данных!");
                Console.WriteLine(execption);
            }
        }

        public void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.ColumnIndex != 8) return;
                var data = dataGridView1.Rows[e.RowIndex].Cells;

                _patient = new Patient(
                    Convert.ToInt32(data[0].Value), // Id
                    Convert.ToString(data[1].Value), // surname
                    Convert.ToString(data[2].Value), // name
                    Convert.ToString(data[3].Value), // middle name
                    Convert.ToBoolean(data[4].Value), // gender
                    Convert.ToDateTime(data[5].Value), // birthday
                    Convert.ToInt32(data[6].Value), // Height
                    Convert.ToInt32(data[7].Value) // Weight
                );
                this.Close();

            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                throw exception;
            }
           
        }

    }
}
