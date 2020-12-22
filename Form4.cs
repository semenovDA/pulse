using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;


namespace pulse_2._0
{
    public partial class Form4 : Form
    {
        public Form4()
        {
            InitializeComponent();
        }
        public String id;
        public string ident
        {
            get { return id; }
            set { id = value; }
        }

        private void textBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            char number = e.KeyChar;
            if (!Char.IsDigit(number) && number != 8) // цифры и клавиша BackSpace
            {
                e.Handled = true;
            }
        }

        private void Form4_Load(object sender, EventArgs e)
        {
            //String id = ident.Value;
            Console.WriteLine(id);
            // label5.Text = "Id = " + id;
              sqlConnection = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename='|DataDirectory|\Database1.mdf';Integrated Security=True");
           // sqlConnection = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\Database1.mdf;Integrated Security=True");
           // sqlConnection = new SqlConnection(@" Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename = C:\Users\Иван\Desktop\ДипР\14.05\pulse_2.0\Database1.mdf; Integrated Security = True");
            comboBox1.Items.AddRange(new string[] { "Мужской", "Женский" });
            label4.Text = "Дата рождения"+ "\r" + "(ММ.ДД.ГГГГ)";
        }
        public SqlConnection sqlConnection = null;
        private void button1_Click(object sender, EventArgs e)
        {
            //  sqlConnection = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename='|DataDirectory|\Database1.mdf';Integrated Security=True");

            //  sqlConnection = new SqlConnection(@" Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename = C:\Users\Иван\Desktop\ДипР\14.05\pulse_2.0\Database1.mdf; Integrated Security = True");
            if (!string.IsNullOrEmpty(textBox1.Text) && !string.IsNullOrWhiteSpace(textBox1.Text) &&
                 !string.IsNullOrEmpty(textBox2.Text) && !string.IsNullOrWhiteSpace(textBox2.Text) &&
                 !string.IsNullOrEmpty(textBox3.Text) && !string.IsNullOrWhiteSpace(textBox3.Text) &&
                 !string.IsNullOrEmpty(textBox5.Text) && !string.IsNullOrWhiteSpace(textBox5.Text) &&
                 !string.IsNullOrEmpty(maskedTextBox1.Text) && !string.IsNullOrWhiteSpace(maskedTextBox1.Text) &&
                 !string.IsNullOrEmpty(textBox6.Text) && !string.IsNullOrWhiteSpace(textBox6.Text) &&
                 !string.IsNullOrEmpty(comboBox1.Text) && !string.IsNullOrWhiteSpace(comboBox1.Text))
            {
                if (label5.Text == "")
                {
                    sqlConnection.Open();
                    SqlCommand command = new SqlCommand("INSERT INTO [TABLE] (Фамилия, Имя, Отчество, Дата_рождения, Рост, Вес, Пол) VALUES(@Фамилия, @Имя, @Отчество, @Дата_рождения, @Рост, @Вес, @Пол)", sqlConnection);
                    command.Parameters.AddWithValue("Фамилия", textBox1.Text);
                    command.Parameters.AddWithValue("Имя", textBox2.Text);
                    command.Parameters.AddWithValue("Отчество", textBox3.Text);
                    command.Parameters.AddWithValue("Дата_рождения", maskedTextBox1.Text);
                    command.Parameters.AddWithValue("Рост", textBox5.Text);
                    command.Parameters.AddWithValue("Вес", textBox6.Text);
                    command.Parameters.AddWithValue("Пол", comboBox1.Text);
                    string sqlFormattedDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                    try
                    {
                        command.ExecuteNonQuery();
                        sqlConnection.Close();
                        MessageBox.Show("Запись сохранена.");
                        this.Close();
                    }
                    catch { MessageBox.Show("Ошибка, проверьте данные!");
                        sqlConnection.Close();
                    }
                }
                else
                {
                    sqlConnection.Open();
                    int r = Convert.ToInt32(label5.Text);
                    SqlCommand update = new SqlCommand("UPDATE [TABLE] SET [Фамилия] = @Фамилия, [Имя] = @Имя, [Отчество] = @Отчество, [Дата_рождения] = @Дата_рождения, [Рост] = @Рост, [Вес] = @Вес, [Пол] = @Пол WHERE Id = @r", sqlConnection);
                    //  SqlCommand update = new SqlCommand("UPDATE [TABLE] SET [Имя] = @Имя WHERE Id = 19", sqlConnection);
                    // var cmd = new SqlCommand("UPDATE Table SET [Фамилия]=@Фамилия WHERE id =19");
                    update.Parameters.AddWithValue("r", r);
                    update.Parameters.AddWithValue("Фамилия", textBox1.Text);
                    update.Parameters.AddWithValue("Имя", textBox2.Text);
                    update.Parameters.AddWithValue("Отчество", textBox3.Text);
                    update.Parameters.AddWithValue("Дата_рождения", maskedTextBox1.Text);
                    update.Parameters.AddWithValue("Рост", textBox5.Text);
                    update.Parameters.AddWithValue("Вес", textBox6.Text);
                    update.Parameters.AddWithValue("Пол", comboBox1.Text);

                    update.ExecuteNonQuery();
                    sqlConnection.Close();

                    MessageBox.Show("Запись сохранена.");
                    this.Close();
                }  
            }
            else { MessageBox.Show("Не должно быть пустых полей!"); }
        }

    }
}
