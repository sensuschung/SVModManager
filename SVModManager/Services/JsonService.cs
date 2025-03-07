using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace SVModManager.Services
{
    public class JsonService
    {
        private JObject? _jsonObject;
        private readonly FileHandler _fileHandler;

        public JsonService()
        {
            _fileHandler = new FileHandler();
        }

        public bool LoadJsonFromFile(string path)
        {
            string? jsonContent = _fileHandler.ReadAllText(path);
            if (jsonContent == null) return false;

            try
            {
                _jsonObject = JObject.Parse(jsonContent);
                return true;
            }
            catch
            {
                _jsonObject = null;
                return false;
            }
        }

        public bool ContainsKey(string key)
        {
            if (_jsonObject == null) return false;
            return _jsonObject.ContainsKey(key);
        }

        public string? GetValue(string key)
        {
            if (_jsonObject == null) return null;

            JToken? value;
            if (_jsonObject.TryGetValue(key, out value))
            {
                return value?.ToString();
            }

            return null;
        }
    }
}
