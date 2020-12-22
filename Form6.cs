using pulse.collection;
using System;
using System.Windows.Forms;

namespace pulse
{
    public partial class Form6 : Form
    {
        private Record _record;
        public Record record { get => _record; set => _record = value; }

        public Form6(Record record)
        {
            _record = record;
            InitializeComponent();
        }    
        
        private void button1_Click(object sender, EventArgs e)
        {
            _record.comments = textBox1.Text;
            _record.create();
            this.Close();
        }
    }
}
