using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SVModManager.Data;

// 基础数据库操作

namespace SVModManager.Services
{
    public class DbService
    {
        //初始化数据库
        public void InitializeDatabase()
        {
            using var context = new AppDbContext();
            context.Database.EnsureCreated();
        }

        //------------------------ 默认配置----------------------------------------------------

        //插入数据项
        public void InsertItem<T>(T item) where T : class
        {
            using var context = new AppDbContext();
            context.Set<T>().Add(item);
            context.SaveChanges();
        }

        //删除数据项
        public void DeleteItem<T>(T item) where T : class
        {
            using var context = new AppDbContext();
            context.Set<T>().Remove(item);
            context.SaveChanges();
        }

        //更新数据项
        public void UpdateItem<T>(T item) where T : class
        {
            using var context = new AppDbContext();
            context.Set<T>().Update(item);
            context.SaveChanges();
        }

        //查询数据项
        public List<T> 
            QueryItems<T>() where T : class
        {
            using var context = new AppDbContext();
            return context.Set<T>().ToList();
        }

        //筛选数据项
        public List<T> QueryItems<T>(Func<T, bool> predicate) where T : class
        {
            using var context = new AppDbContext();
            return context.Set<T>().Where(predicate).ToList();
        }

        //查询单个数据项(可能为空)
        public T? QueryItem<T>(Func<T, bool> predicate) where T : class
        {
            using var context = new AppDbContext();
            return context.Set<T>().FirstOrDefault(predicate);
        }

        //清空数据表
        public void ClearTable<T>() where T : class
        {
            using var context = new AppDbContext();
            context.Set<T>().RemoveRange(context.Set<T>());
            context.SaveChanges();
        }

        //------------------------------------自定义配置----------------------------------
        public void InsertItem<T>(T item, DbContextOptions<AppDbContext> options) where T : class
        {
            using var context = new AppDbContext(options);
            context.Set<T>().Add(item);
            context.SaveChanges();
        }

        // 删除数据项
        public void DeleteItem<T>(T item, DbContextOptions<AppDbContext> options) where T : class
        {
            using var context = new AppDbContext(options);
            context.Set<T>().Remove(item);
            context.SaveChanges();
        }

        // 更新数据项
        public void UpdateItem<T>(T item, DbContextOptions<AppDbContext> options) where T : class
        {
            using var context = new AppDbContext(options);
            context.Set<T>().Update(item);
            context.SaveChanges();
        }

        // 查询数据项
        public List<T> QueryItems<T>(DbContextOptions<AppDbContext> options) where T : class
        {
            using var context = new AppDbContext(options);
            return context.Set<T>().ToList();
        }

        // 筛选数据项
        public List<T> QueryItems<T>(Func<T, bool> predicate, DbContextOptions<AppDbContext> options) where T : class
        {
            using var context = new AppDbContext(options);
            return context.Set<T>().Where(predicate).ToList();
        }

        // 查询单个数据项(可能为空)
        public T? QueryItem<T>(Func<T, bool> predicate, DbContextOptions<AppDbContext> options) where T : class
        {
            using var context = new AppDbContext(options);
            return context.Set<T>().FirstOrDefault(predicate);
        }

        // 清空数据表
        public void ClearTable<T>(DbContextOptions<AppDbContext> options) where T : class
        {
            using var context = new AppDbContext(options);
            context.Set<T>().RemoveRange(context.Set<T>());
            context.SaveChanges();
        }
    }
}
