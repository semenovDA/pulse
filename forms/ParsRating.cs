using pulse.collection;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace pulse.forms
{
    public partial class ParsRating : Form
    {
        public ParsRating(Signal signal)
        {
            InitializeComponent();
            Initialize(signal);
        }

        public void Initialize(Signal signal)
        {
            var number = (int)signal.ComputePars();
            foreach(var label in GetAllControls(this).OfType<Label>()) {
                var isNumeric = int.TryParse(label.Text, out int n);
                if (isNumeric && n != number) label.Font = new Font(DefaultFont, FontStyle.Regular);
                else label.BorderStyle = BorderStyle.FixedSingle;
            }
            pointer.Controls.Add(new Label { Text = "↓" }, number - 1, 0);
        }

        /* Utils */
        public static IEnumerable<Control> GetAllControls(Control root)
        {
            var stack = new Stack<Control>();
            stack.Push(root);

            while (stack.Any())
            {
                var next = stack.Pop();
                foreach (Control child in next.Controls)
                    stack.Push(child);

                yield return next;
            }
        }
    }
}
