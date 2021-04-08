using CP_2021.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace CP_2021.Data
{
    internal class TestDBContext : DbContext
    {
        public TestDBContext()
        {

        }

        public TestDBContext(DbContextOptions<TestDBContext> options):base(options)
        {

        }

        public DbSet<DBPerson> Persons { get; set; }

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
