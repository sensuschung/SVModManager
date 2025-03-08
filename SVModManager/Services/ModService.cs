using SVModManager.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json.Linq;
using Microsoft.EntityFrameworkCore;

namespace SVModManager.Services
{
    public class ModService
    {
        private readonly DbService _dbService;
        private readonly FileHandler _fileHandler;
        private readonly JsonService _jsonService;

        public ModService(DbService dbService, FileHandler fileHandler, JsonService jsonService)
        {
            _dbService = dbService;
            _fileHandler = fileHandler;
            _jsonService = jsonService;
        }

        public void ProcessModsInDirectory(string path)
        {
            TraverseDirectories(path);
        }

        private void TraverseDirectories(string directoryPath)
        {
            var subDirectories = _fileHandler.GetDirectoriesInDirectory(directoryPath);
            if (subDirectories == null) return;

            foreach (var subDirectory in subDirectories)
            {
                string manifestPath = Path.Combine(subDirectory, "manifest.json");

                if (_fileHandler.FileExists(manifestPath))
                {
                    ProcessMod(subDirectory,manifestPath);
                }
                else
                {
                    TraverseDirectories(subDirectory);
                }
            }
        }

        private int? GetNexusID()
        {
            string? updateKeysValue = _jsonService.GetValue("UpdateKeys");
            if (!string.IsNullOrEmpty(updateKeysValue))
            {
                JArray? updateKeysArray = JArray.Parse(updateKeysValue);
                if (updateKeysArray != null)
                {
                    foreach (var key in updateKeysArray)
                    {
                        string keyString = key.ToString();
                        if (keyString.StartsWith("Nexus:"))
                        {
                            if (int.TryParse(keyString.Substring(6), out int nexusId))
                            {
                                return nexusId;
                            }
                        }
                    }
                }
            }
            return null;
        }

        private bool IsModInUse(string directoryPath)
        {
            string folderName = Path.GetFileName(directoryPath);
            return !folderName.StartsWith(".");
        }


        private void ProcessMod(string directoryPath, string manifestPath)
        {
            if (!_jsonService.LoadJsonFromFile(manifestPath)) return;

            string? modName = _jsonService.GetValue("Name");
            string? modAuthor = _jsonService.GetValue("Author");
            string? modVersion = _jsonService.GetValue("Version");
            string? modDescription = _jsonService.GetValue("Description");
            int? nexusId = GetNexusID();
            bool isEnabled = IsModInUse(directoryPath);
            DateTime createOn = _fileHandler.GetDirectoryCreationTime(directoryPath);
            DateTime lastModified = _fileHandler.GetDirectoryLastModified(directoryPath);

            if (modName != null)
            {
                var mod = _dbService.QueryItem<Mod>(m => m.Name == modName);

                if (mod == null)
                {
                    mod = new Mod
                    {
                        Name = modName,
                        Path = directoryPath,
                        NexusId = nexusId,
                        CreateOn = createOn,
                        LastModified = lastModified,
                        Description = modDescription,
                        Author = modAuthor,
                        Version = modVersion,
                        IsEnabled = isEnabled
                    };
                    _dbService.InsertItem(mod);
                }
                else
                {
                    mod.Author = modAuthor;
                    mod.Path = directoryPath;
                    mod.NexusId = nexusId;
                    mod.LastModified = lastModified;
                    mod.Description = modDescription;
                    mod.Name = modName;
                    mod.IsEnabled = isEnabled;
                    mod.Version = modVersion;
                    _dbService.UpdateItem(mod);
                }
            }
        }

        public List<Mod> getAllMods()
        {
            return _dbService.QueryItems<Mod>();
        }

        public Mod? GetModByName(string name)
        {
            return _dbService.QueryItem<Mod>(m => m.Name == name);
        }

        public void AddMod(Mod mod)
        {
            var existingMod = _dbService.QueryItem<Mod>(m => m.Name == mod.Name);
            if (existingMod == null)
            {
                _dbService.InsertItem(mod);
            }
        }

        public void updateNexusId(string modName, int nexusId)
        {
            var mod = _dbService.QueryItem<Mod>(m => m.Name == modName);
            if (mod != null)
            {
                mod.NexusId = nexusId;
                _dbService.UpdateItem(mod);
            }
        }

        public void updateMod(Mod mod)
        {
            _dbService.UpdateItem(mod);
        }

        public void enableMod(string modName)
        {
            var mod = _dbService.QueryItem<Mod>(m => m.Name == modName);
            if (mod != null && mod.Path != null)
            {
                string directoryName = Path.GetFileName(mod.Path);
                if (directoryName.StartsWith("."))
                {
                    string newPath = Path.Combine(Path.GetDirectoryName(mod.Path), directoryName.Substring(1));
                    _fileHandler.RenameDirectory(mod.Path, newPath);
                    mod.Path = newPath;
                    mod.IsEnabled = true;
                    _dbService.UpdateItem(mod);
                }
            }
        }

        public void disableMod(string modName)
        {
            var mod = _dbService.QueryItem<Mod>(m => m.Name == modName);
            if (mod != null && mod.Path != null)
            {
                string directoryName = Path.GetFileName(mod.Path);
                if (!directoryName.StartsWith("."))
                {
                    string newPath = Path.Combine(Path.GetDirectoryName(mod.Path), "." + directoryName);
                    _fileHandler.RenameDirectory(mod.Path, newPath);
                    mod.Path = newPath;
                    mod.IsEnabled = false;
                    _dbService.UpdateItem(mod);
                }
            }
        }


        public void updateTagsToMod(string modName,List<Tag> tags)
        {
            var mod = _dbService.QueryItem<Mod>(m => m.Name == modName);
            if (mod != null)
            {
                mod.Tags = tags;
                _dbService.UpdateItem(mod);
            }
        }

        public bool isModEnabled(string modName)
        {
            var mod = _dbService.QueryItem<Mod>(m => m.Name == modName);
            return mod != null && mod.IsEnabled;
        }

        public bool isModHasTag(string modName, string tagName)
        {
            var mod = _dbService.QueryItem<Mod>(m => m.Name == modName);
            return mod != null && mod.Tags != null && mod.Tags.Any(t => t.Name == tagName);
        }

        public void ExportModsToFile(string filePath)
        {
            var mods = getAllMods();
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                foreach (var mod in mods)
                {
                    writer.WriteLine($"Name: {mod.Name}");
                    writer.WriteLine($"Path: {mod.Path}");
                    writer.WriteLine($"NexusId: {mod.NexusId}");
                    writer.WriteLine($"CreateOn: {mod.CreateOn}");
                    writer.WriteLine($"LastModified: {mod.LastModified}");
                    writer.WriteLine($"IsEnabled: {mod.IsEnabled}");
                    writer.WriteLine($"Tags: {string.Join(", ", mod.Tags.Select(t => t.Name))}");
                    writer.WriteLine("---------------------------------------------------");
                }
            }
        }

        public void clearAllMods()
        {
            _dbService.ClearTable<Mod>();
        }

    }
}
