﻿using CP_2021.Infrastructure;
using CP_2021.Models.DBModels;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CP_2021.Data
{
    class ApplicationContext : DbContext
    {

        public ApplicationContext() : base() {
            //Database.EnsureDeleted();
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region HasDefaultValue
            modelBuilder.Entity<ProductionTaskDB>().Property(t => t.Completion).HasDefaultValue(0);
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
            modelBuilder.Entity<ReportDB>().HasIndex(r => r.TaskId).IsUnique();
            #endregion
            string passwordHash = PasswordHashing.CreateHash("8558286");
            modelBuilder.Entity<UserDB>().HasData(new UserDB("grishkevichai", passwordHash, "Алексей", "Гришкевич") {Id = Guid.NewGuid(), Position = 2 });
            modelBuilder.Entity<HierarchyDB>().HasOne<ProductionTaskDB>(h => h.Parent).WithMany(t => t.ParentTo).HasForeignKey(h => h.ParentId).OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<HierarchyDB>().HasOne<ProductionTaskDB>(h => h.Child).WithOne(t => t.MyParent);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var builder = new ConfigurationBuilder();
            builder.SetBasePath(Directory.GetCurrentDirectory() + "\\Data\\Configs");
            builder.AddJsonFile("appsettings.json");
            var config = builder.Build();
            optionsBuilder.UseLazyLoadingProxies().UseSqlServer(config.GetConnectionString("DefaultConnection"));
        }
    }
}
