using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SVModManager.Data;
using SVModManager.Model;
using System.Linq.Expressions;

// 基础数据库操作

namespace SVModManager.Services
{
    public class DbService
    {

        private AppDbContext context;

        //初始化数据库
        public void InitializeDatabase()
        {
            context = new AppDbContext();
            context.Database.EnsureCreated();
        }

        //------------------------ 默认配置----------------------------------------------------

        //插入数据项
        public void InsertItem<T>(T item) where T : class
        {
            context.Set<T>().Add(item);
            context.SaveChanges();
        }

        //删除数据项
        public void DeleteItem<T>(T item) where T : class
        {
            context.Set<T>().Remove(item);
            context.SaveChanges();
        }

        //更新数据项
        public void UpdateItem<T>(T item) where T : class
        {
            context.Set<T>().Update(item);
            context.SaveChanges();
        }

        //查询数据项
        public List<T> 
            QueryItems<T>() where T : class
        {
            return context.Set<T>().ToList();
        }

        //筛选数据项
        public List<T> QueryItems<T>(Func<T, bool> predicate) where T : class
        {
            return context.Set<T>().Where(predicate).ToList();
        }

        //查询单个数据项(可能为空)
        public T? QueryItem<T>(Func<T, bool> predicate) where T : class
        {
            return context.Set<T>().FirstOrDefault(predicate);
        }

        //清空数据表
        public void ClearTable<T>() where T : class
        {
            context.Set<T>().RemoveRange(context.Set<T>());
            context.SaveChanges();
        }

        public List<Tag> GetTagsForMod(string modName)
        {
            var mod = context.Mods.Include(m => m.Tags).FirstOrDefault(m => m.Name == modName);
            return mod?.Tags.ToList() ?? new List<Tag>();
        }

        public List<Mod> GetModsForTag(string tagName)
        {
            var tag = context.Tags.Include(t => t.Mods).FirstOrDefault(t => t.Name == tagName);
            return tag?.Mods.ToList() ?? new List<Mod>();
        }

        public List<Mod> QueryMods()
        {
            return context.Mods.Include(m => m.Tags).ToList(); 
        }

        public List<Mod> QueryMods(Func<Mod, bool> predicate)
        {
            return context.Mods.Include(m => m.Tags).Where(predicate).ToList();
        }

        public Mod QueryMod(Expression<Func<Mod, bool>> predicate)
        {
            var mod = context.Mods.Include(m => m.Tags).Where(predicate).FirstOrDefault();
            return mod;
        }

        public List<Tag> QueryTags()
        {
            return context.Tags.Include( t => t.Mods).ToList();
        }

        public Tag QueryTag(Expression<Func<Tag, bool>> predicate)
        {
            var tag = context.Tags.Include(m => m.Mods).Where(predicate).FirstOrDefault();
            return tag;
        }

        public void updateDataContext()
        {
            context = new AppDbContext();
            context.Database.EnsureCreated();
        }

    }
}
