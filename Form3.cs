using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Windows.Forms;

namespace pulse
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }
        public SqlConnection sqlConnection = null;
        public SqlCommandBuilder sqlBuilder = null;
        public SqlDataAdapter sqlDataAdapter = null;
        public DataSet dataSet = null;

        public void LoadData()
        {
            try
            {
                sqlDataAdapter = new SqlDataAdapter("SELECT *, 'Delete' AS [Удалить], 'Update' AS [Изменить], 'Data' AS [Данные] FROM [Table]", sqlConnection);
                sqlBuilder = new SqlCommandBuilder(sqlDataAdapter);
                dataSet = new DataSet();
                sqlDataAdapter.Fill(dataSet, "Table");
                dataGridView1.DataSource = dataSet.Tables["Table"];

                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    DataGridViewLinkCell linkCell = new DataGridViewLinkCell();

                    dataGridView1[8, i] = linkCell;

                }
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    DataGridViewLinkCell linkCell = new DataGridViewLinkCell();

                    dataGridView1[9, i] = linkCell;

                }
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    DataGridViewLinkCell linkCell = new DataGridViewLinkCell();

                    dataGridView1[10, i] = linkCell;

                }

            }
            catch
            {
                MessageBox.Show("Ошибка подключения к базе данных!");
            }
        }
        bool f = false;
        public void ReloadData()
        {
            try
            {
                dataGridView1.Columns.Clear();
                sqlDataAdapter = new SqlDataAdapter("SELECT *, 'Delete' AS [Удалить], 'Update' AS [Изменить], 'Data' AS [Данные] FROM [Table]" , sqlConnection);
                sqlBuilder = new SqlCommandBuilder(sqlDataAdapter);
                dataSet = new DataSet();
                sqlDataAdapter.Fill(dataSet, "Table");
                dataGridView1.DataSource = dataSet.Tables["Table"];

                

                dataSet.Tables["Table"].Clear();
                sqlDataAdapter.Fill(dataSet, "Table");
                dataGridView1.DataSource = dataSet.Tables["Table"];
                обновитьToolStripMenuItem.Text = "Обновить";
                фИОToolStripMenuItem.Visible = false;
                f = false;

                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    DataGridViewLinkCell linkCell = new DataGridViewLinkCell();

                    dataGridView1[8, i] = linkCell;
                }

                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    DataGridViewLinkCell linkCell = new DataGridViewLinkCell();

                    dataGridView1[9, i] = linkCell;

                }
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    DataGridViewLinkCell linkCell = new DataGridViewLinkCell();

                    dataGridView1[10, i] = linkCell;

                }

            }
            catch
            {
                MessageBox.Show("Ошибка подключения к базе данных!");
            }
        }
        private void Form3_Load(object sender, EventArgs e)
        {

            sqlConnection = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename='|DataDirectory|\Database1.mdf';Integrated Security=True");
            //sqlConnection = new SqlConnection(@" Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename = C:\Users\Иван\Desktop\ДипР\14.05\pulse_2.0\Database1.mdf; Integrated Security = True");

            LoadData();

        }

        private void обновитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ReloadData();
        }

        private void dataGridView1_UserAddedRow(object sender, DataGridViewRowEventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.ColumnIndex == 8)
                {
                    if (MessageBox.Show("Удалить строку?", "Удаление", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                         == DialogResult.Yes)
                    {
                        try
                        {
                            String task = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
                            Console.WriteLine(task);

                            System.IO.File.Delete(@"saves/" + task + ".txt");

                            task = "";
                        }
                        catch { MessageBox.Show("Ошибка"); }
                        int rowIndex = e.RowIndex;
                        dataGridView1.Rows.RemoveAt(rowIndex);
                        dataSet.Tables["Table"].Rows[rowIndex].Delete();
                        sqlDataAdapter.Update(dataSet, "Table");
                        ReloadData();
                    }

                }
                 if ((e.ColumnIndex == 0)&&(f == true))
                 {
                     String task = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();

                     Form2 f2 = new Form2();

                     var fileContent = string.Empty;
                     var filePath = string.Empty;
                     String rl;
                     int tm = 0;
                     int dol2 = 0;
                     int x2 = 0;

                     // openFileDialog.InitialDirectory = "c:\\pulse_saves";


                     using (StreamReader f = new StreamReader("saves/" + task + ".txt"))
                     {
                         //fileContent = reader.ReadToEnd();
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
                                     // f2.chart1.ChartAreas[0].AxisX.ScaleView.Zoom(x - 300, x);
                                     // w = f2.chart1.Series[0].Points.Count;
                                     tm++;
                                 }
                             }
                         }
                         f.Close();
                         x2 = 0;
                         f2.ShowDialog();
                     }




                 }
                if ((e.ColumnIndex == 1) && (f == true))
                {
                   
                    if (MessageBox.Show("Удалить строку?", "Удаление", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                         == DialogResult.Yes)
                    {
                        try
                        {
                            String task = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
                            Console.WriteLine(task);

                            System.IO.File.Delete(@"saves/" + task + ".txt");

                            task = "";
                        }
                        catch { MessageBox.Show("Ошибка"); }
                        int rowIndex = e.RowIndex;
                        dataGridView1.Rows.RemoveAt(rowIndex);
                        dataSet.Tables["Data"].Rows[rowIndex].Delete();
                        sqlDataAdapter.Update(dataSet, "Data");
                        
                    }

                }
                if (e.ColumnIndex == 9)
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
                    Console.WriteLine(task);
                    f4.ShowDialog();
                    ReloadData();
                }
                if (e.ColumnIndex == 10)
                {
                    фИОToolStripMenuItem.Text = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString()+ " " + dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString() +" "+ dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();
                    int idp = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[0].Value);
                    SqlCommand comm = new SqlCommand("SELECT *  FROM [Data] WHERE Пациент = @idp", sqlConnection);
                    comm.Parameters.AddWithValue("idp", idp);
                    sqlDataAdapter = new SqlDataAdapter(comm);
                    
                    dataGridView1.Columns.Add("Открыть", "Открыть");

                    dataGridView1.Columns.Add("Удалить", "Удалить");

                    sqlBuilder = new SqlCommandBuilder(sqlDataAdapter);
                    dataSet = new DataSet();

                    sqlDataAdapter.Fill(dataSet, "Data");
                    dataGridView1.DataSource = dataSet.Tables["Data"];
                    обновитьToolStripMenuItem.Text = "Назад";
                    фИОToolStripMenuItem.Visible = true;
                     f = true;
                    // dataGridView1.Columns.Remove(dataGridView1.Columns[6]);

                    dataGridView1.Columns["Пациент"].Visible = false;
                    dataGridView1.Columns["Id"].DisplayIndex = 0;
                    dataGridView1.Columns["Время"].DisplayIndex = 1;
                    dataGridView1.Columns["Длительность"].DisplayIndex = 2;
                    dataGridView1.Columns["Примечание"].DisplayIndex = 3;
                    dataGridView1.Columns["Открыть"].DisplayIndex = 4;
                    dataGridView1.Columns["Удалить"].DisplayIndex = 5;

                    for (int i = 0; i < dataGridView1.Rows.Count; i++)
                    {
                        DataGridViewLinkCell linkCell = new DataGridViewLinkCell();

                        dataGridView1[0, i] = linkCell;                       
                        dataGridView1[0, i].Value = "Открыть";
                       
                    }
                    for (int i = 0; i < dataGridView1.Rows.Count; i++)
                    {
                        DataGridViewLinkCell linkCell = new DataGridViewLinkCell();

                        
                        dataGridView1[1, i] = linkCell;
                        dataGridView1[1, i].Value = "Удалить";
                    }

                }
               
               
            }
            catch
            {

            }
           // ReloadData();
        }

        private void сохранитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
          /*  //Фамилия, Имя, Отчество, Дата_рождения, Рост, Вес, Пол
            DataRow row = dataSet.Tables["Table"].NewRow();
           // row["Id"] = dataGridView1.Rows[0].Cells["Id"].Value;
            row["Фамилия"] = dataGridView1.Rows[0].Cells["Фамилия"].Value;
            row["Имя"] = dataGridView1.Rows[0].Cells["Имя"].Value;
            row["Отчество"] = dataGridView1.Rows[0].Cells["Отчество"].Value;
            row["Рост"] = dataGridView1.Rows[0].Cells["Рост"].Value;
            row["Дата_рождения"] = dataGridView1.Rows[0].Cells["Дата_рождения"].Value;
            row["Вес"] = dataGridView1.Rows[0].Cells["Вес"].Value;
            row["Пол"] = dataGridView1.Rows[0].Cells["Пол"].Value;
            dataSet.Tables["Table"].Rows.Add(row);
            dataSet.Tables["Table"].Rows.RemoveAt(dataSet.Tables["Table"].Rows.Count - 1);
            sqlDataAdapter.Update(dataSet, "Table");*/
        }
    }
}
