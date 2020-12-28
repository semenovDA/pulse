using pulse.collection;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;


namespace pulse
{
    public partial class PatientCreate : Form
    {
        private Patient _patient;
        public Patient patient { get => _patient; set => _patient = value; }

        public PatientCreate(Patient patient = null)
        {
            InitializeComponent();
            _patient = patient;
        }

        private void textBox4_KeyPress(object sender, KeyPressEventArgs e)
        {   // цифры и клавиша BackSpace (8)
            if (!Char.IsDigit(e.KeyChar) && e.KeyChar != 8) { e.Handled = true; }
        }

        private void Form4_Load(object sender, EventArgs e)
        {
            if(_patient != null)
            {
                textBox1.Text = _patient.surname;
                textBox2.Text = _patient.name;
                textBox3.Text = _patient.middleName;
                maskedTextBox1.Text = _patient.birthdayDate.ToString();
                textBox5.Text = _patient.height.ToString();
                textBox6.Text = _patient.weight.ToString();
                comboBox1.Text = _patient.genderName();
            }
            comboBox1.Items.AddRange(new string[] { "Мужской", "Женский" });
            label4.Text = "Дата рождения"+ "\r" + "(ММ.ДД.ГГГГ)";
        }
        private void button1_Click(object sender, EventArgs e)
        {

            if (!string.IsNullOrEmpty(textBox1.Text) && !string.IsNullOrWhiteSpace(textBox1.Text) &&
                 !string.IsNullOrEmpty(textBox2.Text) && !string.IsNullOrWhiteSpace(textBox2.Text) &&
                 !string.IsNullOrEmpty(textBox3.Text) && !string.IsNullOrWhiteSpace(textBox3.Text) &&
                 !string.IsNullOrEmpty(textBox5.Text) && !string.IsNullOrWhiteSpace(textBox5.Text) &&
                 !string.IsNullOrEmpty(maskedTextBox1.Text) && !string.IsNullOrWhiteSpace(maskedTextBox1.Text) &&
                 !string.IsNullOrEmpty(textBox6.Text) && !string.IsNullOrWhiteSpace(textBox6.Text) &&
                 !string.IsNullOrEmpty(comboBox1.Text) && !string.IsNullOrWhiteSpace(comboBox1.Text))
            {
                if (_patient == null)
                {
                    try
                    {
                        _patient = new Patient(
                            textBox1.Text,
                            textBox2.Text,
                            textBox3.Text,
                            comboBox1.Text == "Мужской",
                            DateTime.Parse(maskedTextBox1.Text),
                            Convert.ToInt32(textBox5.Text),
                            Convert.ToInt32(textBox6.Text)
                        );

                        _patient.create();
                    }
                    catch { MessageBox.Show("Ошибка, проверьте данные!"); }
                    finally {
                        MessageBox.Show("Запись сохранена.");
                        this.Close();
                    }
                }
                else
                {

                    _patient.surname = textBox1.Text;
                    _patient.name = textBox2.Text;
                    _patient.middleName = textBox3.Text;
                    _patient.birthdayDate = DateTime.Parse(maskedTextBox1.Text);
                    _patient.gender = comboBox1.Text == "Мужской";
                    _patient.height = Convert.ToInt32(textBox5.Text);
                    _patient.weight = Convert.ToInt32(textBox6.Text);

                    _patient.update();

                    MessageBox.Show("Запись сохранена.");
                    this.Close();
                }  
            }
            else { MessageBox.Show("Не должно быть пустых полей!"); }
        }

    }
}
