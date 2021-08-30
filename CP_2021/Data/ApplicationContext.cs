using CP_2021.Infrastructure;
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
        public virtual DbSet<ProductionTaskDB> Production_Plan { get; set; }
        public virtual DbSet<ComplectationDB> Complectation { get; set; }
        public virtual DbSet<GivingDB> Giving { get; set; }
        public virtual DbSet<InProductionDB> In_Production { get; set; }
        public virtual DbSet<HierarchyDB> HierarchyDB { get; set; }
        public virtual DbSet<UserDB> Users { get; set; }
        public virtual DbSet<FormattingDB> Formatting { get; set; }
        public virtual DbSet<LaborCostsDB> LaborCosts { get; set; }
        public virtual DbSet<ManufactureDB> Manufacture { get; set; }
        public virtual DbSet<PaymentDB> Payment { get; set; }
        public virtual DbSet<ReportDB> Reports { get; set; }
        public virtual DbSet<TaskDB> Tasks { get; set; }

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
            modelBuilder.Entity<FormattingDB>().Property(t => t.IsBold).HasDefaultValue(false);
            #endregion
            #region IsUnique
            modelBuilder.Entity<ComplectationDB>().HasIndex(c => c.ProductionTaskId).IsUnique();
            modelBuilder.Entity<GivingDB>().HasIndex(g => g.ProductionTaskId).IsUnique();
            modelBuilder.Entity<InProductionDB>().HasIndex(i => i.ProductionTaskId).IsUnique();
            modelBuilder.Entity<ManufactureDB>().HasIndex(m => m.ProductionTaskId).IsUnique();
            modelBuilder.Entity<ReportDB>().HasIndex(r => r.TaskId).IsUnique();
            modelBuilder.Entity<FormattingDB>().HasIndex(f => f.ProductionTaskId).IsUnique();
            #endregion
            string passwordHash = PasswordHashing.CreateHash("8558286");
            modelBuilder.Entity<UserDB>().HasData(new UserDB("grishkevichai", passwordHash, "Алексей", "Гришкевич") {Id = Guid.NewGuid(), Position = 2 });
            modelBuilder.Entity<HierarchyDB>().HasOne<ProductionTaskDB>(h => h.Parent).WithMany(t => t.ParentTo).HasForeignKey(h => h.ParentId).OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<HierarchyDB>().HasOne<ProductionTaskDB>(h => h.Child).WithOne(t => t.MyParent);
            modelBuilder.Entity<FormattingDB>().HasOne<ProductionTaskDB>(t => t.ProductionTask).WithOne(t => t.Formatting);

            modelBuilder.Entity<PaymentDB>().HasIndex(t => t.ProductionTaskId).IsUnique();
            modelBuilder.Entity<PaymentDB>().Property(t => t.IsFirstPayment).HasDefaultValue(false);
            modelBuilder.Entity<PaymentDB>().Property(t => t.IsSecondPayment).HasDefaultValue(false);
            modelBuilder.Entity<PaymentDB>().Property(t => t.IsFullPayment).HasDefaultValue(false);
            modelBuilder.Entity<PaymentDB>().HasOne<ProductionTaskDB>(t => t.ProductionTask).WithOne(t => t.Payment);

            modelBuilder.Entity<LaborCostsDB>().HasIndex(t => t.ProductionTaskId).IsUnique();
            modelBuilder.Entity<LaborCostsDB>().HasOne<ProductionTaskDB>(t => t.ProductionTask).WithOne(t => t.LaborCosts);
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
