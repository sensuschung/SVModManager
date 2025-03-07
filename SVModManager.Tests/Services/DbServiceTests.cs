using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.EntityFrameworkCore;
using SVModManager.Data;
using SVModManager.Services;
using SVModManager.Model;
using System;
using System.Collections.Generic;
using System.Linq;

//���Է�װ�����ݿ������
 namespace SVModManager.Tests
{
    [TestClass]
    public class DbServiceTests
    {
        private DbService _dbService;
        private DbContextOptions<AppDbContext> _contextOptions;

        [TestInitialize]
        public void Setup()
        {
            // ָ�����ݿ��ļ�·��
            string databasePath = @"...";
            _contextOptions = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlite($"Data Source={databasePath}")
                .Options;
            _dbService = new DbService();

            // ��ʼ�����ݿ�
            using var context = new AppDbContext(_contextOptions);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated(); 
        }

        //���Բ���
        [TestMethod]
        public void CanInsertMod()
        {
            var mod = new Mod
            {
                Name = "TestMod",
                Path = "C:\\TestPath",
                NexusId = 123,
                CreateOn = DateTime.Now,
                LastModified = DateTime.Now,
                IsEnabled = true
            };

            _dbService.InsertItem(mod);

            using var context = new AppDbContext(_contextOptions);
            Assert.AreEqual(1, context.Mods.Count());
            var insertedMod = context.Mods.FirstOrDefault(m => m.Name == "TestMod");
            Assert.IsNotNull(insertedMod);
            Assert.AreEqual("C:\\TestPath", insertedMod.Path);
        }

        //���Դ���
        [TestMethod]
        public void CanDeleteMod()
        {
            var mod = new Mod
            {
                Name = "TestMod",
                Path = "C:\\TestPath",
                NexusId = 123,
                CreateOn = DateTime.Now,
                LastModified = DateTime.Now,
                IsEnabled = true
            };
            _dbService.InsertItem(mod);

            _dbService.DeleteItem(mod);

            using var context = new AppDbContext(_contextOptions);
            Assert.AreEqual(0, context.Mods.Count());
        }

        //���Ը���
        [TestMethod]
        public void CanUpdateMod()
        {
            var mod = new Mod
            {
                Name = "TestMod",
                Path = "C:\\TestPath",
                NexusId = 123,
                CreateOn = DateTime.Now,
                LastModified = DateTime.Now,
                IsEnabled = true
            };
            _dbService.InsertItem(mod);

            mod.Path = "D:\\UpdatedPath";
            _dbService.UpdateItem(mod);

            using var context = new AppDbContext(_contextOptions);
            var updatedMod = context.Mods.FirstOrDefault(m => m.Name == "TestMod");
            Assert.IsNotNull(updatedMod);
            Assert.AreEqual("D:\\UpdatedPath", updatedMod.Path);
        }

        //���Բ�ѯ
        [TestMethod]
        public void CanQueryMods()
        {
            var mod1 = new Mod { Name = "TestMod1", Path = "C:\\TestPath1", IsEnabled = true };
            var mod2 = new Mod { Name = "TestMod2", Path = "C:\\TestPath2", IsEnabled = false };
            _dbService.InsertItem(mod1);
            _dbService.InsertItem(mod2);

            var allMods = _dbService.QueryItems<Mod>();
            Assert.AreEqual(2, allMods.Count);

            var enabledMods = _dbService.QueryItems<Mod>(m => m.IsEnabled);
            Assert.AreEqual(1, enabledMods.Count);
            Assert.AreEqual("TestMod1", enabledMods.First().Name);
        }

        //�������
        [TestMethod]
        public void CanClearMods()
        {
            var mod1 = new Mod { Name = "TestMod1", Path = "C:\\TestPath1" };
            var mod2 = new Mod { Name = "TestMod2", Path = "C:\\TestPath2" };
            _dbService.InsertItem(mod1);
            _dbService.InsertItem(mod2);

            _dbService.ClearTable<Mod>();

            using var context = new AppDbContext(_contextOptions);
            Assert.AreEqual(0, context.Mods.Count());
        }
    }
}
