using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using pulse.collection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pulse.core
{
    class CacheHandler
    {
        private Record _record;
        private JObject _cache;

        private string appdata = Path.Combine(Environment.GetFolderPath(
                Environment.SpecialFolder.ApplicationData), "pulse");

        public JObject Cache { get => _cache; }

        public CacheHandler(Record record)
        {
            this._record = record;

            if (cacheExists(_record)) Upload(_record);
            else Create(_record);
        }

        private string cachePath(Record record)
        {
            if (!Directory.Exists(appdata)) Directory.CreateDirectory(appdata);
            return Path.Combine(appdata, record.getCacheName());
        }

        public bool cacheExists(Record record) {
            var fileName = cachePath(record);
            return File.Exists(fileName);
        }
        
        private void Create(Record record)
        {
            _cache = new JObject(
                        new JProperty("updated", DateTime.Now),
                        new JProperty("data", new JObject()));

            Update(record);
        }

        private void Upload(Record record) => _cache = JObject.Parse(File.ReadAllText(cachePath(record)));

        public void Write(JToken obj)
        {
            var key = obj.Path;
            _cache["data"][key] = obj.First;
            Update(_record);
        }
        public void Update(Record record) => File.WriteAllText(cachePath(record), _cache.ToString());
    }
}
