﻿using Newtonsoft.Json.Linq;
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
        public double[] norm_signal { get; set; }
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
            norm_signal = normalizeSignal();
        }
        public List<int> readSignal(Record record)
        {
            var filename = record.getFileName();
            return File.ReadLines(filename)
                       .Select(s => int.Parse(s))
                       .ToList();
        }
        public int[] computePeaks()
        {
            string[] args = { string.Format("-hz {0:0}", 220) }; // TODO: fix HZ amount
            var result = base.Excute(SCRIPT_VSRSIGNAL);
            return result["peaks"].Select(jv => (int)jv).ToArray();
        }
        public double[] normalizeSignal()
        {
            var result = base.Excute(SCRIPT_VSRSIGNAL);
            return result["normalized"].Select(jv => (double)jv).ToArray();
        }
        public double[] computeFiltredSignal()
        {
            string[] args = { string.Format("-hz {0:0}", 220) };
            var result = base.Excute(SCRIPT_VSRSIGNAL);
            return result["filtered"].Select(jv => (double)jv).ToArray();
        }
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
        public JToken ComputeFrequency(Spectrogram.Method method)
        {
            string[] args = { string.Format("-peaks {0}", string.Join(";", peaks)) };
            var obj = base.Excute(SCRIPT_VSRFREQUENCY, true, args);
            if (method == Spectrogram.Method.Welch) return obj["welch"];
            else if (method == Spectrogram.Method.Lomb) return obj["lomb"];
            else if (method == Spectrogram.Method.Autoregressive) return obj["ar"];
            else return obj;
        }
        public JToken ComputePoincare() {
            string[] args = { string.Format("-peaks {0}", string.Join(";", peaks)) };
            return base.Excute(SCRIPT_VSRNONLINEAR, true, args)["poincare"];
        }
        public JToken ComputeACF() {
            string[] args = { string.Format("-peaks {0}", string.Join(";", peaks)) };
            return base.Excute(SCRIPT_VSRNONLINEAR, true, args)["ACF"];
        }
        public JToken ComputePars()
        {
            string[] args = { string.Format("-peaks {0}", string.Join(";", peaks)) };
            return base.Excute(SCRIPT_VSRSTATS, true, args)["pars"];
        }
        public JToken ComputeStatistics() {
            string[] args = { string.Format("-peaks {0}", string.Join(";", peaks)) };
            return base.Excute(SCRIPT_VSRSTATS, true, args)["stats"];
        }
        public void RecomputeAnalysis()
        {
            var cacheHandler = new CacheHandler(record);
            var signalPath = formatPropretyName(SCRIPT_VSRSIGNAL);
            cacheHandler.Cache["data"][signalPath]["peaks"] = new JArray(peaks);
            
            if(cacheHandler.Cache["data"][formatPropretyName(SCRIPT_VSRNONLINEAR)] != null)
                cacheHandler.Cache["data"][formatPropretyName(SCRIPT_VSRNONLINEAR)]["update"] = "true";

            if (cacheHandler.Cache["data"][formatPropretyName(SCRIPT_VSRSTATS)] != null)
                cacheHandler.Cache["data"][formatPropretyName(SCRIPT_VSRSTATS)]["update"] = "true";

            if (cacheHandler.Cache["data"][formatPropretyName(SCRIPT_VSRFREQUENCY)] != null)
                cacheHandler.Cache["data"][formatPropretyName(SCRIPT_VSRFREQUENCY)]["update"] = "true";
            
            cacheHandler.Update();
        }
        public JToken ComputeCustomScript(string path)
        {
            var jToken = base.Excute(path, false);
            return JObject.Parse(File.ReadAllText(jToken.ToString()));
        }
    }
}
