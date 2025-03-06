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

            // 使用 InMemoryDatabase 配置 DbContextOptions
            _contextOptions = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: uniqueDatabaseName)
                .Options;

            // 确保测试开始时数据库是干净的
            using var context = new AppDbContext(_contextOptions);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
        }

        [TestMethod]
        public void CanInsertModIntoDatabase()
        {
            using var context = new AppDbContext(_contextOptions);

            // 创建并插入 Mod 数据
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

            // 验证数据是否成功插入
            Assert.AreEqual(1, context.Mods.Count());
            var insertedMod = context.Mods.FirstOrDefault(m => m.Name == "TestMod");
            Assert.IsNotNull(insertedMod);
            Assert.AreEqual("C:\\TestPath", insertedMod.Path);
        }

        [TestMethod]
        public void CanInsertAndRetrieveTags()
        {
            using var context = new AppDbContext(_contextOptions);

            // 插入 Mod 和 Tag
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

            // 验证 Tag 是否正确插入
            var retrievedMod = context.Mods.Include(m => m.Tags).FirstOrDefault(m => m.Name == "TestMod");
            Assert.IsNotNull(retrievedMod);
            Assert.AreEqual(2, retrievedMod.Tags.Count);
        }

        [TestMethod]
        public void CanDeleteModFromDatabase()
        {
            using var context = new AppDbContext(_contextOptions);

            // 插入一个 Mod
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

            // 删除 Mod
            var modToDelete = context.Mods.First(m => m.Name == "TestMod");
            Assert.IsNotNull(modToDelete);
            context.Mods.Remove(modToDelete);
            context.SaveChanges();

            // 验证是否成功删除
            Assert.AreEqual(0, context.Mods.Count());
        }

        [TestMethod]
        public void CanUpdateModInDatabase()
        {
            using var context = new AppDbContext(_contextOptions);

            // 插入一个 Mod
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

            // 更新 Mod
            var modToUpdate = context.Mods.First(m => m.Name == "TestMod");
            modToUpdate.Path = "C:\\NewPath";
            modToUpdate.IsEnabled = true;
            context.Mods.Update(modToUpdate);
            context.SaveChanges();

            // 验证是否成功更新
            var updatedMod = context.Mods.First(m => m.Name == "TestMod");
            Assert.AreEqual("C:\\NewPath", updatedMod.Path);
            Assert.IsTrue(updatedMod.IsEnabled);
        }

    }
}
