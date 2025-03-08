using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SVModManager.Services;

namespace SVModManager.Tests
{
    [TestClass]
    public class JsonServiceTests
    {
        private readonly string _jsonFilePath = AppDomain.CurrentDomain.BaseDirectory + "test.json";
        private JsonService _jsonService;

        [TestInitialize]
        public void Setup()
        {
            //FileHandler fileHandler = new FileHandler();
            _jsonService = new JsonService();

            // 创建测试的 JSON 文件
        //    string jsonContent = @"{
        //      'Name': 'Test Mod',
        //      'Author': 'Test Author',
        //      'Version': '1.0.0',
        //      'Description': 'This is a test mod',
        //      'ContentPackFor': {
        //        'UniqueID': 'TestMod'
        //        }
        //    }";

        //    fileHandler.WriteAllText(_jsonFilePath, jsonContent);
        //
        }

        [TestMethod]
        public void Test_LoadJsonFromFile_ReturnsTrue()
        {
            bool result = _jsonService.LoadJsonFromFile(_jsonFilePath);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Test_ContainsKey_ReturnsTrue()
        {
            _jsonService.LoadJsonFromFile(_jsonFilePath);
            bool result = _jsonService.ContainsKey("ContentPackFor.UniqueID");
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Test_GetValue_ReturnsCorrectValue()
        {
            _jsonService.LoadJsonFromFile(_jsonFilePath);
            string? value = _jsonService.GetValue("Name");
            Assert.AreEqual("Valley Girls", value);
        }

        [TestCleanup]
        public void Cleanup()
        {
            // 删除测试的 JSON 文件
            if (System.IO.File.Exists(_jsonFilePath))
            {
                System.IO.File.Delete(_jsonFilePath);
            }
        }
    }
}
