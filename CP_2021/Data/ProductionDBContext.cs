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

        #endregion

        public ProductionDBContext() {
            Database.EnsureDeleted();
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProductionTaskDB>().Property(t => t.Completion).HasDefaultValue(false);
            modelBuilder.Entity<GivingDB>().Property(g => g.State).HasDefaultValue(false);
            modelBuilder.Entity<ComplectationDB>().Property(c => c.Percentage).HasDefaultValue(0f);
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
