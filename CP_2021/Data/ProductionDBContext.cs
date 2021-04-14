using CP_2021.Models.DBModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CP_2021.Data
{
    class ProductionDBContext : DbContext
    {
        #region DBSets

        public DbSet<GivingDB> Givings { get; set; }

        public DbSet<ProductionTaskDB> ProductionTasks { get; set; }

        public DbSet<ComplectationDB> Complectations { get; set; }

        public DbSet<InProductionDB> InProductions { get; set; }

        public DbSet<ManufactureDB> Manufactures { get; set; }

        //public DbSet<UserDB> Users { get; set; }

        //public DbSet<TaskDB> Tasks { get; set; }

        //public DbSet<ReportDB> Reports { get; set; }

        #endregion

        public ProductionDBContext() {
            //Database.EnsureDeleted();
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region HasDefaultValue
            modelBuilder.Entity<ProductionTaskDB>().Property(t => t.Completion).HasDefaultValue(false);
            modelBuilder.Entity<GivingDB>().Property(g => g.State).HasDefaultValue(false);
            modelBuilder.Entity<ComplectationDB>().Property(c => c.Percentage).HasDefaultValue(0f);
            #endregion
            #region IsUnique
            modelBuilder.Entity<ComplectationDB>().HasIndex(c => c.ProductionTaskId).IsUnique();
            modelBuilder.Entity<GivingDB>().HasIndex(g => g.ProductionTaskId).IsUnique();
            modelBuilder.Entity<InProductionDB>().HasIndex(i => i.ProductionTaskId).IsUnique();
            modelBuilder.Entity<ManufactureDB>().HasIndex(m => m.ProductionTaskId).IsUnique();
            #endregion

        }

        //public ProductionDBContext(DbContextOptions<TestDBContext> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var builder = new ConfigurationBuilder();
            builder.SetBasePath(Directory.GetCurrentDirectory() + "\\Data\\Configs");
            builder.AddJsonFile("appsettings.json");
            var config = builder.Build();
            optionsBuilder.UseSqlServer(config.GetConnectionString("DefaultConnection"));
        }
    }
}
