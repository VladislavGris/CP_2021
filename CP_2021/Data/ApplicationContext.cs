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
    class ApplicationContext : DbContext
    {
        #region DBSets

        //public DbSet<GivingDB> Givings { get; set; }

        //public DbSet<ProductionTaskDB> ProductionTasks { get; set; }

        //public DbSet<ComplectationDB> Complectations { get; set; }

        //public DbSet<InProductionDB> InProductions { get; set; }

        //public DbSet<ManufactureDB> Manufactures { get; set; }

        //public DbSet<UserDB> Users { get; set; }

        //public DbSet<TaskDB> Tasks { get; set; }

        //public DbSet<ReportDB> Reports { get; set; }

        #endregion

        public ApplicationContext() : base() {
            Database.EnsureDeleted();
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region HasDefaultValue
            modelBuilder.Entity<ProductionTaskDB>().Property(t => t.Completion).HasDefaultValue(false);
            modelBuilder.Entity<GivingDB>().Property(g => g.State).HasDefaultValue(false);
            modelBuilder.Entity<ComplectationDB>().Property(c => c.Percentage).HasDefaultValue(0f);
            modelBuilder.Entity<TaskDB>().Property(c => c.Completion).HasDefaultValue(false);
            modelBuilder.Entity<ReportDB>().Property(p => p.State).HasDefaultValue(false);
            modelBuilder.Entity<UserDB>().Property(u => u.Position).HasDefaultValue(0);
            #endregion
            #region IsUnique
            modelBuilder.Entity<ComplectationDB>().HasIndex(c => c.ProductionTaskId).IsUnique();
            modelBuilder.Entity<GivingDB>().HasIndex(g => g.ProductionTaskId).IsUnique();
            modelBuilder.Entity<InProductionDB>().HasIndex(i => i.ProductionTaskId).IsUnique();
            modelBuilder.Entity<ManufactureDB>().HasIndex(m => m.ProductionTaskId).IsUnique();
            #endregion
            modelBuilder.Entity<UserDB>().HasData(new UserDB("vlad", "1337", "Владислав", "Гришкевич") { Id = -1, Position = 2 });
        }

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
