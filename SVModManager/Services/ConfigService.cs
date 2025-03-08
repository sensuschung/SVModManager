using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;
using SVModManager.Model;

namespace SVModManager.Services
{
    public class ConfigService
    {
        private readonly DbService _dbService;

        public ConfigService(DbService dbService)
        {
            _dbService = dbService;
        }

        public void InitConfig()
        {
            Config stardewModPath = new Config { Name = "StardewModPath", Value = "" };
            Config nexusAPI = new Config { Name = "NexusAPI", Value = "" };
            if (_dbService.QueryItem<Config>(m => m.Name == "StardewModPath") == null)
            {
                _dbService.InsertItem<Config>(stardewModPath);
            }
            if (_dbService.QueryItem<Config>(m => m.Name == "NexusAPI") == null)
            {
                _dbService.InsertItem<Config>(nexusAPI);
            }
        }

        public void setStardewModPath(string path)
        {
            Config stardewModPath = _dbService.QueryItem<Config>(m => m.Name == "StardewModPath");
            stardewModPath.Value = path;
            _dbService.UpdateItem(stardewModPath);
        }

        public string getStardewModPath()
        {
            Config stardewModPath = _dbService.QueryItem<Config>(m => m.Name == "StardewModPath");
            return stardewModPath.Value;
        }

        public void setNexusAPI(string api)
        {
            Config nexusAPI = _dbService.QueryItem<Config>(m => m.Name == "NexusAPI");
            nexusAPI.Value = api;
            _dbService.UpdateItem(nexusAPI);
        }

        public string getNexusAPI()
        {
            Config nexusAPI = _dbService.QueryItem<Config>(m => m.Name == "NexusAPI");
            return nexusAPI.Value;
        }


        private static string GetSteamInstallationPath()
        {
            string registryKey = @"Software\Valve\Steam";
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(registryKey))
            {
                if (key != null)
                {
                    object installLocation = key.GetValue("SteamPath");
                    if (installLocation != null)
                    {
                        return installLocation.ToString();
                    }
                }
            }
            using (RegistryKey key = Registry.LocalMachine.OpenSubKey(registryKey))
            {
                if (key != null)
                {
                    object installLocation = key.GetValue("SteamPath");
                    if (installLocation != null)
                    {
                        return installLocation.ToString();
                    }
                }
            }
            return null;
        }

        private string? detectGetModPath()
        {
            string steamPath = GetSteamInstallationPath();
            string subfolder = @"/steamapps/common/Stardew Valley/Mods";
            if (!string.IsNullOrEmpty(steamPath))
            {
                //Console.WriteLine("Steam 安装路径: " + steamPath);
                string modPath = steamPath + subfolder;
                if (Directory.Exists(modPath))
                {
                    return modPath;
                }
            }
            return null;
        }

        public bool autoGetModPath()
        {
            string? modPath = detectGetModPath();
            if (modPath != null)
            {
                setStardewModPath(modPath);
                return true;
            }
            return false;
        }

    }
}
