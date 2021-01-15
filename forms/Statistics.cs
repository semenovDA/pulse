using Newtonsoft.Json.Linq;
using pulse.collection;
using System;
using System.Windows.Forms;

namespace pulse.forms
{
    public partial class VSRStatistics : Form
    {
        // Private variables
        // TODO: resolve JObject inside jproprety issue

        private JToken _jToken;
        private Patient _patient;

        // Private functions
        private void Initialize()
        {
            patientName.Text = String.Format("Пациент: {0}", _patient.fullName());
            nni_counter.Text = _jToken["nni_counter"].ToObject<int>().ToString();
            nni_mean.Text = _jToken["nni_mean"].ToObject<double>().ToString("0.000");
            nni_min.Text = _jToken["nni_min"].ToObject<int>().ToString();
            nni_max.Text = _jToken["nni_max"].ToObject<int>().ToString();

            hr_std.Text = _jToken["hr_mean"].ToObject<double>().ToString("0.000");
            hr_mean.Text = _jToken["hr_std"].ToObject<double>().ToString("0.000");
            hr_min.Text = _jToken["hr_min"].ToObject<int>().ToString();
            hr_max.Text = _jToken["hr_max"].ToObject<int>().ToString();

            nni_diff_mean.Text = _jToken["nni_diff_mean"].ToObject<double>().ToString("0.000");
            nni_diff_max.Text = _jToken["nni_diff_max"].ToObject<int>().ToString();
            nni_diff_min.Text = _jToken["nni_diff_min"].ToObject<int>().ToString();

            sdnn.Text = _jToken["sdnn"].ToObject<double>().ToString("0.0000");
            rmssd.Text = _jToken["rmssd"].ToObject<double>().ToString("0.0000");
            sdsd.Text = _jToken["sdsd"].ToObject<double>().ToString("0.0000");

            nn50.Text = _jToken["nn50"].ToObject<int>().ToString();
            pnn50.Text = _jToken["pnn50"].ToObject<double>().ToString("0.000");
            nn20.Text = _jToken["nn20"].ToObject<int>().ToString();
            pnn20.Text = _jToken["pnn20"].ToObject<double>().ToString("0.000");

            tinn_n.Text = _jToken["tinn_n"].ToObject<double>().ToString("0.00");
            tinn_m.Text = _jToken["tinn_m"].ToObject<double>().ToString("0.00");
            tinn.Text = _jToken["tinn"].ToObject<double>().ToString("0.00");

            tri_index.Text = _jToken["tri_index"].ToObject<double>().ToString("0.00");
        }

        // Main constructor
        public VSRStatistics(Patient patient, JToken jToken)
        {
            _jToken = jToken;
            _patient = patient;

            InitializeComponent();
            Initialize();
        }
    }
}
