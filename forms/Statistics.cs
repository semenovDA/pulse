using Newtonsoft.Json.Linq;
using pulse.collection;
using System;
using System.Windows.Forms;

namespace pulse.forms
{
    public partial class VSRStatistics : Form
    {
        // Private variables
        private JObject _jObject;
        private Patient _patient;

        // Private functions
        private void Initialize()
        {
            patientName.Text = String.Format("Пациент: {0}", _patient.fullName());
            nni_counter.Text = _jObject.GetValue("nni_counter").ToObject<int>().ToString();
            nni_mean.Text = _jObject.GetValue("nni_mean").ToObject<double>().ToString("0.000");
            nni_min.Text = _jObject.GetValue("nni_min").ToObject<int>().ToString();
            nni_max.Text = _jObject.GetValue("nni_max").ToObject<int>().ToString();

            hr_std.Text = _jObject.GetValue("hr_mean").ToObject<double>().ToString("0.000");
            hr_mean.Text = _jObject.GetValue("hr_std").ToObject<double>().ToString("0.000");
            hr_min.Text = _jObject.GetValue("hr_min").ToObject<int>().ToString();
            hr_max.Text = _jObject.GetValue("hr_max").ToObject<int>().ToString();

            nni_diff_mean.Text = _jObject.GetValue("nni_diff_mean").ToObject<double>().ToString("0.000");
            nni_diff_max.Text = _jObject.GetValue("nni_diff_max").ToObject<int>().ToString();
            nni_diff_min.Text = _jObject.GetValue("nni_diff_min").ToObject<int>().ToString();

            sdnn.Text = _jObject.GetValue("sdnn").ToObject<double>().ToString("0.0000");
            rmssd.Text = _jObject.GetValue("rmssd").ToObject<double>().ToString("0.0000");
            sdsd.Text = _jObject.GetValue("sdsd").ToObject<double>().ToString("0.0000");

            nn50.Text = _jObject.GetValue("nn50").ToObject<int>().ToString();
            pnn50.Text = _jObject.GetValue("pnn50").ToObject<double>().ToString("0.000");
            nn20.Text = _jObject.GetValue("nn20").ToObject<int>().ToString();
            pnn20.Text = _jObject.GetValue("pnn20").ToObject<double>().ToString("0.000");

            tinn_n.Text = _jObject.GetValue("tinn_n").ToObject<double>().ToString("0.00");
            tinn_m.Text = _jObject.GetValue("tinn_m").ToObject<double>().ToString("0.00");
            tinn.Text = _jObject.GetValue("tinn").ToObject<double>().ToString("0.00");

            tri_index.Text = _jObject.GetValue("tri_index").ToObject<double>().ToString("0.00");
        }

        // Main constructor
        public VSRStatistics(Patient patient, JObject jObject)
        {
            _jObject = jObject;
            _patient = patient;

            InitializeComponent();
            Initialize();
        }
    }
}
