using CP_2021.Models.DataWindowEntities;
using CP_2021.Models.DBModels;
using CP_2021.Models.ProcedureResuts;
using CP_2021.Models.ProcedureResuts.Plan;
using CP_2021.Models.ViewEntities;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.IO;

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
        public virtual DbSet<ActDB> Act { get; set; }

        public virtual DbSet<Task_Hierarchy_Formatting> Task_Hierarchy_Formatting { get; set; }
        public virtual DbSet<TaskReport> TaskReport { get; set; }
        public virtual DbSet<ComplectationWindowEntity> ComplectationData { get; set; }
        public virtual DbSet<UserNames> UserNames { get; set; }

        //public virtual DbSet<ManufactureNames> ManufactureNames { get; set; }
        //public virtual DbSet<HeadTasks> HeadTasks { get; set; }
        //public virtual DbSet<NoSpecifications> NoSpecifications { get; set; }
        //public virtual DbSet<SpecificationsOnControl> SpecOnControl { get; set; }
        //public virtual DbSet<SpecificationsInVipisk> SpecInVipisk { get; set; }
        //public virtual DbSet<CoopWork> CoopWork { get; set; }
        //public virtual DbSet<InProgressView> InProductionView { get; set; }
        //public virtual DbSet<DocumentationView> DocumentationView { get; set; }
        //public virtual DbSet<SKBCheck> SKBCkecks {  get; set; }
        //public virtual DbSet<OETSStorage> OETSStorage { get; set; }
        //public virtual DbSet<GivingStorage> GivingStorage { get; set; }
        //public virtual DbSet<GivingReports> GivingReports { get; set; }
        //public virtual DbSet<ActForm> ActForm { get; set; }

        //public virtual DbSet<SearchProcResult> SearchResults { get; set; }
        //public virtual DbSet<ByAct> ByAct { get; set; }
        //public virtual DbSet<PaymentFirstPart> PaymentFirstParts { get; set; }

        public SqlConnection connection;

        public ApplicationContext() : base() {
            //Database.EnsureDeleted();
            //Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //#region HasDefaultValue
            //modelBuilder.Entity<ProductionTaskDB>().Property(t => t.Completion).HasDefaultValue(0);
            //modelBuilder.Entity<GivingDB>().Property(g => g.State).HasDefaultValue(false);
            //modelBuilder.Entity<ComplectationDB>().Property(c => c.Percentage).HasDefaultValue(0f);
            //modelBuilder.Entity<TaskDB>().Property(c => c.Completion).HasDefaultValue(false);
            //modelBuilder.Entity<ReportDB>().Property(p => p.State).HasDefaultValue(false);
            //modelBuilder.Entity<UserDB>().Property(u => u.Position).HasDefaultValue(0);
            //modelBuilder.Entity<FormattingDB>().Property(t => t.IsBold).HasDefaultValue(false);
            //#endregion
            //#region IsUnique
            //modelBuilder.Entity<ComplectationDB>().HasIndex(c => c.ProductionTaskId).IsUnique();
            //modelBuilder.Entity<GivingDB>().HasIndex(g => g.ProductionTaskId).IsUnique();
            //modelBuilder.Entity<InProductionDB>().HasIndex(i => i.ProductionTaskId).IsUnique();
            //modelBuilder.Entity<ManufactureDB>().HasIndex(m => m.ProductionTaskId).IsUnique();
            //modelBuilder.Entity<ReportDB>().HasIndex(r => r.TaskId).IsUnique();
            //modelBuilder.Entity<FormattingDB>().HasIndex(f => f.ProductionTaskId).IsUnique();
            //modelBuilder.Entity<ActDB>().HasIndex(f => f.ProductionTaskId).IsUnique();
            //#endregion
            //string passwordHash = PasswordHashing.CreateHash("8558286");
            //modelBuilder.Entity<UserDB>().HasData(new UserDB("grishkevichai", passwordHash, "Алексей", "Гришкевич") { Id = Guid.NewGuid(), Position = 2 });
            modelBuilder.Entity<HierarchyDB>().HasOne<ProductionTaskDB>(h => h.Parent).WithMany(t => t.ParentTo).HasForeignKey(h => h.ParentId).OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<HierarchyDB>().HasOne<ProductionTaskDB>(h => h.Child).WithOne(t => t.MyParent);
            modelBuilder.Entity<FormattingDB>().HasOne<ProductionTaskDB>(t => t.ProductionTask).WithOne(t => t.Formatting);
            modelBuilder.Entity<ActDB>().HasOne<ProductionTaskDB>(t => t.ProductionTask).WithOne(t => t.Act);

            //modelBuilder.Entity<PaymentDB>().HasIndex(t => t.ProductionTaskId).IsUnique();
            //modelBuilder.Entity<PaymentDB>().Property(t => t.IsFirstPayment).HasDefaultValue(false);
            //modelBuilder.Entity<PaymentDB>().Property(t => t.IsSecondPayment).HasDefaultValue(false);
            //modelBuilder.Entity<PaymentDB>().Property(t => t.IsFullPayment).HasDefaultValue(false);
            modelBuilder.Entity<PaymentDB>().HasOne<ProductionTaskDB>(t => t.ProductionTask).WithOne(t => t.Payment);

            //modelBuilder.Entity<LaborCostsDB>().HasIndex(t => t.ProductionTaskId).IsUnique();
            modelBuilder.Entity<LaborCostsDB>().HasOne<ProductionTaskDB>(t => t.ProductionTask).WithOne(t => t.LaborCosts);

            #region Views
            modelBuilder.Entity<HeadTasks>(c =>
            {
                c.HasNoKey();
                c.ToView("HeadTasks");
            });
            modelBuilder.Entity<ManufactureNames>(c =>
            {
                c.HasNoKey();
                c.ToView("ManufactureNames");
            });
            modelBuilder.Entity<NoSpecifications>(ns =>
            {
                ns.HasNoKey();
                ns.ToView("NoSpecifications");

            });
            modelBuilder.Entity<SpecificationsOnControl>(c =>
            {
                c.HasNoKey();
                c.ToView("SpecificationsOnControl");
            });
            modelBuilder.Entity<SpecificationsInVipisk>(s =>
            {
                s.HasNoKey();
                s.ToView("SpecificationsInVipisk");
            });
            modelBuilder.Entity<CoopWork>(c =>
            {
                c.HasNoKey();
                c.ToView("CoopWork");
            });
            modelBuilder.Entity<InProgressView>(i =>
            {
                i.HasNoKey();
                i.ToView("InProgress");
            });
            modelBuilder.Entity<DocumentationView>(i =>
            {
                i.HasNoKey();
                i.ToView("Documentation");
            });
            modelBuilder.Entity<SKBCheck>(i =>
            {
                i.HasNoKey();
                i.ToView("SKBCheck");
            });
            modelBuilder.Entity<OETSStorage>(i =>
            {
                i.HasNoKey();
                i.ToView("OETSStorage");
            });
            modelBuilder.Entity<GivingStorage>(i =>
            {
                i.HasNoKey();
                i.ToView("GivingStorage");
            });
            modelBuilder.Entity<GivingReports>(i =>
            {
                i.HasNoKey();
                i.ToView("GivingReports");
            });
            modelBuilder.Entity<ActForm>(i =>
            {
                i.HasNoKey();
                i.ToView("ActForm");
            });
            #endregion
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var builder = new ConfigurationBuilder();
            builder.SetBasePath(Directory.GetCurrentDirectory() + "\\Data\\Configs");
            builder.AddJsonFile("appsettings.json");
            var config = builder.Build();
            optionsBuilder./*UseLazyLoadingProxies().*/UseSqlServer(config.GetConnectionString("DefaultConnection"));
            connection = new SqlConnection(config.GetConnectionString("DefaultConnection"));
            connection.Open();
        }
    }
}
