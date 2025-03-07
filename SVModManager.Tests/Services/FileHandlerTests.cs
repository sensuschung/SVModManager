using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using SVModManager.Services;

namespace SVModManager.Tests
{
    [TestClass]
    public class FileHandlerTests
    {
        private FileHandler _fileHandler;
        private string testDirectory;
        private string testFile;

        [TestInitialize]
        public void Setup()
        {
            _fileHandler = new FileHandler();
            testDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestDirectory");  // 使用当前项目路径
            Console.WriteLine(testDirectory);
            //testFile = Path.Combine(testDirectory, "testfile.txt");

            if (_fileHandler.DirectoryExists(testDirectory))
            {
                _fileHandler.DeleteDirectory(testDirectory, true);
            }

            _fileHandler.CreateDirectory(testDirectory);
        }

        [TestCleanup]
        public void Cleanup()
        {
            if (_fileHandler.DirectoryExists(testDirectory))
            {
                _fileHandler.DeleteDirectory(testDirectory, true);
            }
        }

        [TestMethod]
        public void TestCreateFile()
        {
            testFile = Path.Combine(testDirectory, "testfilecreate.txt");
            _fileHandler.CreateFile(testFile);
            Assert.IsTrue(_fileHandler.FileExists(testFile));
        }

        [TestMethod]
        public void TestDeleteFile()
        {
            testFile = Path.Combine(testDirectory, "testfiledelete.txt");
            _fileHandler.CreateFile(testFile);
            _fileHandler.DeleteFile(testFile);
            Assert.IsFalse(_fileHandler.FileExists(testFile));
        }

        [TestMethod]
        public void TestRenameFile()
        {
            testFile = Path.Combine(testDirectory, "testfilerename.txt");
            _fileHandler.CreateFile(testFile);
            string newFilePath = Path.Combine(testDirectory, "renamedfile.txt");
            _fileHandler.RenameFile(testFile, newFilePath);
            Assert.IsFalse(_fileHandler.FileExists(testFile));
            Assert.IsTrue(_fileHandler.FileExists(newFilePath));
        }

        [TestMethod]
        public void TestReadWriteFile()
        {
            testFile = Path.Combine(testDirectory, "testfilereadwrite.txt");
            string content = "Hello, World!";
            _fileHandler.WriteAllText(testFile, content);

            string readContent = _fileHandler.ReadAllText(testFile);
            Assert.AreEqual(content, readContent);
        }

        [TestMethod]
        public void TestGetFileSize()
        {
            testFile = Path.Combine(testDirectory, "testfilesize.txt");
            string content = "Hello, World!";
            _fileHandler.WriteAllText(testFile, content);

            long size = _fileHandler.GetFileSize(testFile);
            Assert.AreEqual(content.Length, size);
        }

        [TestMethod]
        public void TestFileTimestamps()
        {
            testFile = Path.Combine(testDirectory, "testfilets.txt");
            _fileHandler.CreateFile(testFile);

            DateTime creationTime = _fileHandler.GetFileCreationTime(testFile);
            DateTime lastModifiedTime = _fileHandler.GetFileLastModified(testFile);

            Assert.AreNotEqual(DateTime.MinValue, creationTime);
            Assert.AreNotEqual(DateTime.MinValue, lastModifiedTime);
        }

        [TestMethod]
        public void TestCreateDirectory()
        {
            Assert.IsTrue(_fileHandler.DirectoryExists(testDirectory));
        }

        [TestMethod]
        public void TestDeleteDirectory()
        {
            _fileHandler.DeleteDirectory(testDirectory, true);
            Assert.IsFalse(_fileHandler.DirectoryExists(testDirectory));
        }

        [TestMethod]
        public void TestRenameDirectory()
        {
            string newDirPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "RenamedDirectory");
            _fileHandler.RenameDirectory(testDirectory, newDirPath);

            Assert.IsFalse(_fileHandler.DirectoryExists(testDirectory));
            Assert.IsTrue(_fileHandler.DirectoryExists(newDirPath));

            _fileHandler.DeleteDirectory(newDirPath, true);
        }

        [TestMethod]
        public void TestGetFilesInDirectory()
        {
            testFile = Path.Combine(testDirectory, "testfileindirectory.txt");
            _fileHandler.CreateFile(testFile);
            string[] files = _fileHandler.GetFilesInDirectory(testDirectory);

            Assert.IsNotNull(files);
            Assert.AreEqual(1, files.Length);
            Assert.AreEqual(testFile, files[0]);
        }

        [TestMethod]
        public void TestGetDirectoriesInDirectory()
        {
            string subDirectory = Path.Combine(testDirectory, "SubDirectory");
            _fileHandler.CreateDirectory(subDirectory);

            string[] directories = _fileHandler.GetDirectoriesInDirectory(testDirectory);

            Assert.IsNotNull(directories);
            Assert.AreEqual(1, directories.Length);
            Assert.AreEqual(subDirectory, directories[0]);
        }
    }
}
