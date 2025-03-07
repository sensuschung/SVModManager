using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SVModManager.Services
{
    public class FileHandler
    {
        public bool FileExists(string path)
        {
            return File.Exists(path);
        }

        public void CreateFile(string path)
        {
            File.Create(path).Close();
        }

        public void DeleteFile(string path)
        {
            if (FileExists(path))
            {
                File.Delete(path);
            }
        }

        public void RenameFile(string oldPath, string newPath)
        {
            File.Move(oldPath, newPath);
        }

        public string? ReadAllText(string path)
        {
            if (!FileExists(path))
            {
                return null;
            }
            return File.ReadAllText(path);
        }

        public void WriteAllText(string path, string content)
        {
            File.WriteAllText(path, content);
        }

        public void AppendText(string path, string content)
        {
            File.AppendAllText(path, content);
        }

        public long GetFileSize(string path)
        {
            return FileExists(path) ? new FileInfo(path).Length : 0;
        }

        public DateTime GetFileLastModified(string path)
        {
            return FileExists(path) ? File.GetLastWriteTime(path) : DateTime.MinValue;
        }

        public DateTime GetFileCreationTime(string path)
        {
            return FileExists(path) ? File.GetCreationTime(path) : DateTime.MinValue;
        }

        public bool DirectoryExists(string path)
        {
            return Directory.Exists(path);
        }

        public void CreateDirectory(string path)
        {
            Directory.CreateDirectory(path);
        }

        public void DeleteDirectory(string path, bool recursive = true)
        {
            if (DirectoryExists(path))
            {
                Directory.Delete(path, recursive);
            }
        }

        public void RenameDirectory(string oldPath, string newPath)
        {
            Directory.Move(oldPath, newPath);
        }

        public string[]? GetFilesInDirectory(string path)
        {
            if(!DirectoryExists(path))
            {
                return null;
            }
            return Directory.GetFiles(path);
        }

        public string[]? GetDirectoriesInDirectory(string path)
        {
            if(!DirectoryExists(path))
            {
                return null;
            }
            return Directory.GetDirectories(path);
        }

        public DateTime GetDirectoryLastModified(string path)
        {
            return DirectoryExists(path) ? Directory.GetLastWriteTime(path) : DateTime.MinValue;
        }

        public DateTime GetDirectoryCreationTime(string path)
        {
            return DirectoryExists(path) ? Directory.GetCreationTime(path) : DateTime.MinValue;
        }
    }
}
