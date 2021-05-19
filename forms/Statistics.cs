using Newtonsoft.Json.Linq;
using pulse.collection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace pulse.forms
{
    enum Type
    {
        BASIC = 0,
        ADDITIONAL = 1,
        GEOMETRICAL = 2
    }

    public partial class VSRStatistics : Form
    {
        // Private variables
        private static string _mapPath = Path.GetFullPath(AppDomain.CurrentDomain.BaseDirectory + "files/map.json");

        // Private functions
        private List<Statistic> assertMap(JToken jToken) {
            
            JObject map = JObject.Parse(File.ReadAllText(_mapPath));
            var list = new List<Statistic>();

            foreach (var token in jToken) {

                string value;
                var key = ((JProperty)token).Name.ToLower();
                var target = map.SelectToken(String.Format("$.map[?(@.key == '{0}')]", key));
                var type = ((JProperty)token).Value.Type;

                if (JTokenType.Float == type) value = token.ToObject<double>().ToString("0.000");
                else if (JTokenType.Integer == type) value = token.ToObject<int>().ToString();
                else value = token.ToObject<string>().ToString();

                Type stat_type = Type.BASIC;
                if (target["type"].ToString() == "additional") stat_type = Type.ADDITIONAL;
                else if (target["type"].ToString() == "geometrical") stat_type = Type.GEOMETRICAL;

                list.Add(
                    new Statistic(
                        key, target["name"].ToString(),
                        value, target["description"].ToString(),
                        stat_type
                    )
                );
            }

            return list;
        }

        private void Initialize(Patient patient, Signal signal)
        {
            var jToken = signal.ComputeStatistics();
            patientName.Text = patient != null ?
                String.Format("Пациент: {0}", patient.fullName()) :
                "Пациент: -";

            List<Statistic> list = assertMap(jToken);

            foreach (var stat in list) {
                if(stat.type == Type.BASIC) basic.Rows.Add(stat.key, stat.value, stat.name);
                else if(stat.type == Type.ADDITIONAL) additional.Rows.Add(stat.key, stat.value, stat.name);
                else if(stat.type == Type.GEOMETRICAL) geometrical.Rows.Add(stat.key, stat.value, stat.name);
            }

        }

        // Main constructor
        public VSRStatistics(Patient patient, Signal signal)
        {
            InitializeComponent();
            Initialize(patient, signal);
        }

    }
    class Statistic
    {
        public string key { get; set; }
        public string name { get; set; }
        public string value { get; set; }
        public string interpretation { get; set; }
        public Type type { get; set; }

        public Statistic(string key, string name, string value, string interpretation, Type type)
        {
            this.key = key;
            this.name = name;
            this.interpretation = interpretation;
            this.value = value;
            this.type = type;
        }
    }

}
