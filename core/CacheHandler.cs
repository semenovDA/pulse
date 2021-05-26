using Newtonsoft.Json.Linq;
using pulse.collection;
using System;
using System.IO;

namespace pulse.core
{
    public class CacheHandler
    {
        /*   Variables defenition    */
        private Record _record;
        private JObject _cache;

        private string appdata = Path.Combine(Environment.GetFolderPath(
                Environment.SpecialFolder.ApplicationData), "pulse");

        /*  Getters & Setters    */
        public JObject Cache { get => _cache; }

        /*  Main constructor    */
        public CacheHandler(Record record)
        {
            this._record = record;

            if (cacheExists(_record)) Upload();
            else Create(_record);
        }

        /*  Main functions   */
        private void Create(Record record)
        {
            _cache = new JObject(new JProperty("data", new JObject()));
            Update();
        }
        public void Upload() => _cache = JObject.Parse(File.ReadAllText(cachePath(_record)));
        public void Write(JToken obj)
        {
            var key = obj.Path;
            var path = obj.First.ToString();
            try
            {
                path = Path.GetFullPath(path);
                var result = JObject.Parse(File.ReadAllText(path));
                _cache["data"][key] = result;
                File.Delete(path);
            }
            catch (Exception) { _cache["data"][key] = obj.First; }
            Update();
        }
        public void Update() => File.WriteAllText(cachePath(_record), _cache.ToString());
        
        /* Utils functions*/
        private string cachePath(Record record)
        {
            if (!Directory.Exists(appdata)) Directory.CreateDirectory(appdata);
            return Path.Combine(appdata, record.getCacheName());
        }
        public bool cacheExists(Record record)
        {
            var fileName = cachePath(record);
            return File.Exists(fileName);
        }

    }
}
