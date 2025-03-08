using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SVModManager.Model;

namespace SVModManager.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Mod> Mods { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Config> Configs { get; set; }

        //依赖注入初始化
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // 自动初始化
        public AppDbContext()
        {
        }

        // 配置sqlite数据库
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // --------------------------- 配置测试环境数据库路径 ---------------------------
                string databasePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "test.db");
                optionsBuilder.UseSqlite($"Data Source={databasePath}");
                //optionsBuilder.UseSqlite("Data Source=app.db");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //配置表名
            modelBuilder.Entity<Mod>().ToTable("Mods");
            modelBuilder.Entity<Tag>().ToTable("Tags");
            modelBuilder.Entity<Config>().ToTable("Configs");

            //配置主键
            modelBuilder.Entity<Mod>().HasKey(m => m.Name);
            modelBuilder.Entity<Tag>().HasKey(t => t.Name);
            modelBuilder.Entity<Config>().HasKey(c => c.Name);

            //一对多关系
            modelBuilder.Entity<Mod>()
                .HasMany(m => m.Tags)
                .WithOne()
                .OnDelete(DeleteBehavior.SetNull);

            base.OnModelCreating(modelBuilder);
        }
    }
}
