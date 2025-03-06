using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.EntityFrameworkCore;
using SVModManager.Data;
using SVModManager.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SVModManager.Tests
{
    [TestClass]
    public class TestAppDbContext
    {

        private DbContextOptions<AppDbContext> _contextOptions = null!;

        [TestInitialize]
        public void Setup()
        {
            string uniqueDatabaseName = "TestDatabase_" + Guid.NewGuid().ToString();

            // ʹ�� InMemoryDatabase ���� DbContextOptions
            _contextOptions = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: uniqueDatabaseName)
                .Options;

            // ȷ�����Կ�ʼʱ���ݿ��Ǹɾ���
            using var context = new AppDbContext(_contextOptions);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
        }

        [TestMethod]
        public void CanInsertModIntoDatabase()
        {
            using var context = new AppDbContext(_contextOptions);

            // ���������� Mod ����
            var mod = new Mod
            {
                Name = "TestMod",
                Path = "C:\\TestPath",
                NexusId = 123,
                CreateOn = DateTime.Now,
                LastModified = DateTime.Now,
                IsEnabled = true
            };
            context.Mods.Add(mod);
            var result = context.SaveChanges();
            Assert.AreEqual(1, result);

            // ��֤�����Ƿ�ɹ�����
            Assert.AreEqual(1, context.Mods.Count());
            var insertedMod = context.Mods.FirstOrDefault(m => m.Name == "TestMod");
            Assert.IsNotNull(insertedMod);
            Assert.AreEqual("C:\\TestPath", insertedMod.Path);
        }

        [TestMethod]
        public void CanInsertAndRetrieveTags()
        {
            using var context = new AppDbContext(_contextOptions);

            // ���� Mod �� Tag
            var mod = new Mod
            {
                Name = "TestMod",
                Path = "C:\\TestPath",
                NexusId = 123,
                CreateOn = DateTime.Now,
                LastModified = DateTime.Now,
                IsEnabled = true
            };
            mod.Tags = new List<Tag>
            {
                new Tag(name:"111",color:"#ab1233"),
                new Tag(name:"222",color:"#ab1233"),
            };
            context.Mods.Add(mod);
            context.SaveChanges();

            // ��֤ Tag �Ƿ���ȷ����
            var retrievedMod = context.Mods.Include(m => m.Tags).FirstOrDefault(m => m.Name == "TestMod");
            Assert.IsNotNull(retrievedMod);
            Assert.AreEqual(2, retrievedMod.Tags.Count);
        }

        [TestMethod]
        public void CanDeleteModFromDatabase()
        {
            using var context = new AppDbContext(_contextOptions);

            // ����һ�� Mod
            var mod = new Mod
            {
                Name = "TestMod",
                Path = "C:\\TestPath",
                NexusId = 123,
                CreateOn = DateTime.Now,
                LastModified = DateTime.Now,
                IsEnabled = true
            };
            context.Mods.Add(mod);
            context.SaveChanges();

            // ɾ�� Mod
            var modToDelete = context.Mods.First(m => m.Name == "TestMod");
            Assert.IsNotNull(modToDelete);
            context.Mods.Remove(modToDelete);
            context.SaveChanges();

            // ��֤�Ƿ�ɹ�ɾ��
            Assert.AreEqual(0, context.Mods.Count());
        }

        [TestMethod]
        public void CanUpdateModInDatabase()
        {
            using var context = new AppDbContext(_contextOptions);

            // ����һ�� Mod
            var mod = new Mod
            {
                Name = "TestMod",
                Path = "C:\\TestPath",
                NexusId = 123,
                CreateOn = DateTime.Now,
                LastModified = DateTime.Now,
                IsEnabled = true
            };
            context.Mods.Add(mod);
            context.SaveChanges();

            // ���� Mod
            var modToUpdate = context.Mods.First(m => m.Name == "TestMod");
            modToUpdate.Path = "C:\\NewPath";
            modToUpdate.IsEnabled = true;
            context.Mods.Update(modToUpdate);
            context.SaveChanges();

            // ��֤�Ƿ�ɹ�����
            var updatedMod = context.Mods.First(m => m.Name == "TestMod");
            Assert.AreEqual("C:\\NewPath", updatedMod.Path);
            Assert.IsTrue(updatedMod.IsEnabled);
        }

    }
}
