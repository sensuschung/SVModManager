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
            var keys = key.Split('.');
            JToken? currentToken = _jsonObject;
            foreach (var k in keys)
            {
                if (currentToken is JObject currentObject && currentObject.TryGetValue(k, out var nextToken))
                {
                    currentToken = nextToken;
                }
                else
                {
                    return false;
                }
            }
            return true;
        }

        public string? GetValue(string key)
        {
            if (_jsonObject == null) return null;
            var keys = key.Split('.');
            JToken? currentToken = _jsonObject;
            foreach (var k in keys)
            {
                if (currentToken is JObject currentObj && currentObj.TryGetValue(k, out var nextToken))
                {
                    currentToken = nextToken;
                }
                else
                {
                    return null;
                }
            }
            return currentToken?.ToString();
        }

    }
}
