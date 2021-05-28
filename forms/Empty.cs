using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace pulse.forms
{
    public partial class Empty : Form
    {
        public Empty()
        {
            InitializeComponent();
        }

        private void Empty_SizeChanged(object sender, EventArgs e)
        {
            Console.WriteLine(this.Size);
        }
    }
}
