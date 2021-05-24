using Newtonsoft.Json.Linq;
using pulse.core;
using pulse.graphics;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace pulse.collection
{
    public class Signal : PythonUtils
    {
        public static int ms = 1000;

        public Record record { get; set; }
        public List<int> signal { get; set; }
        public int[] peaks { get; set; }
        public double Hz { get; set; }
        public double HZstep { get; set; }
        public double timestep { get; set; }

        public Signal(Record _record) : base(_record)
        {
            record = _record;
            signal = readSignal(_record);
            Hz = signal.Count() / _record.duration;
            HZstep = ms / Hz;
            timestep = _record.duration * ms;
            peaks = computePeaks();
        }
        public List<int> readSignal(Record record)
        {
            var filename = record.getFileName();
            return File.ReadLines(filename)
                       .Select(s => int.Parse(s))
                       .ToList();
        }
        public int[] computePeaks() => base.Excute(PythonUtils.SCRIPT_VSRPEAKS)
                                           .Select(jv => (int)jv)
                                           .ToArray();
        public List<double> computeRR(bool timesteped = true)
        {
            List<double> points = new List<double>();
            for (int i = 1; i < peaks.Length; i++) {
                points.Add(timesteped ?
                    ((peaks[i] * HZstep) - (peaks[i - 1] * HZstep)) / ms :
                    (peaks[i] - peaks[i - 1]));
            }
            return points;
        }
        public JObject ComputeFrequency()
        {
            var jToken = base.Excute(PythonUtils.SCRIPT_VSRFREQUENCY);
            return JObject.Parse(File.ReadAllText(jToken.ToString()));
        }
        public JToken ComputeFrequency(Spectrogram.Method method)
        {
            var obj = ComputeFrequency();
            if (method == Spectrogram.Method.Welch) return obj["welch"];
            else if (method == Spectrogram.Method.Lomb) return obj["lomb"];
            else if (method == Spectrogram.Method.Autoregressive) return obj["ar"];
            else return obj;
        }
        public JToken ComputePoincare()
        {
            var jToken = base.Excute(PythonUtils.SCRIPT_VSRNONLINEAR);
            return JObject.Parse(File.ReadAllText(jToken.ToString()))["poincare"];
        }
        public JToken ComputeACF()
        {
            var jToken = base.Excute(PythonUtils.SCRIPT_VSRNONLINEAR);
            return JObject.Parse(File.ReadAllText(jToken.ToString()))["ACF"];
        }

        public JToken ComputePars() => base.Excute(PythonUtils.SCRIPT_VSRPARS);
        public JToken ComputeStatistics() => base.Excute(PythonUtils.SCRIPT_VSRSTATS);
        public JToken ComputeCustomScript(string path)
        {
            var jToken = base.Excute(path, false);
            return JObject.Parse(File.ReadAllText(jToken.ToString()));
        }
    }
}
