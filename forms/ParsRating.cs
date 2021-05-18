using pulse.collection;
using System.Windows.Forms;

namespace pulse.forms
{
    public partial class ParsRating : Form
    {
        Signal _signal;

        public ParsRating(Signal signal)
        {
            InitializeComponent();
            _signal = signal;
        }
    }
}
